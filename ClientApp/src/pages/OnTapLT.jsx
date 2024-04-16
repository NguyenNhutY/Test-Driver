import React, { useState, useEffect, memo } from 'react';
import axios from 'axios';
import $ from 'jquery';
import { TbPlayerTrackNextFilled, TbPlayerTrackPrevFilled } from "react-icons/tb";
import {Cursor } from '../components'


const SwitchComponent = memo(({ handleCheckboxChange, autoSwitchEnabled }) => {
    return (
        <div className="form-check form-switch text-lg">
            <label htmlFor="auto-switch" className="form-check-label" id="iswitch"> {autoSwitchEnabled ? "Tự động chuyển câu: Bật" : "Tự động chuyển câu: Tắt"} </label>
            <input onChange={handleCheckboxChange} type="checkbox" id="auto-switch" className="form-check-input" checked={autoSwitchEnabled} />
        </div>
    );
});

export const OnTapLT = () => {
    //Hạng thị GPLX
    const [classGPLX, setClassGPLX] = useState(() => {
        var hangJson = localStorage.getItem('HANG');

        var hang = JSON.parse(hangJson);
        if (hang == null)
            window.location.href = '/';
        return hang != null ? hang.idHang : '';
    });

    //Câu hỏi dành cho hạng thị(int)
    const [lsQuestion, setLsQuestion] = useState([]);

    //Câu hỏi hiện tại(int)
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);

    //Tự động chuyển câu (T/F)
    const [autoSwitchEnabled, setAutoSwitchEnabled] = useState(false);

    //Font size (Float)
    const [fontSize, setFontSize] = useState(1);

    //Thông tin câu hỏi (Object)
    const [ques, setQues] = useState();

    const [listChuong, setListChuong] = useState();

    const [lsChoose, setLsChoose] = useState();

    //Tải câu hỏi và init giá trị cho page
    useEffect(() => {


        const fetchData = async () => {
            try {
                //Get question

                const ques_response = await axios.post(`https://localhost:7086/lythuyet/laycauhoi?id=${classGPLX}`);

                const questions = ques_response.data.$values;
                setLsQuestion(questions);
                fetch('https://localhost:7086/cauhoi/getchapterformobile')
                    .then(response => response.json())
                    .then(data => {
                        setListChuong(data);
                    })
                    .catch(error => {
                        console.error('Error fetching data:', error);
                    });

                //Get chapter

                //Get list chooses
                const storedLsChoose = localStorage.getItem("lsChoose");

                if (storedLsChoose) {
                    setLsChoose(JSON.parse(storedLsChoose)); // Sử dụng lsChoose từ localStorage nếu có
                } else {
                    setLsChoose(Array(questions.length).fill(-1));
                }
                setQues(questions[0]);
            } catch (error) {
                console.error(error);
            }
        };
        fetchData();
    }, []);


    useEffect(() => {
        const handleKeyDown = (event) => {
            console.log('Key pressed:', event.key);
            switch (event.key) {
                case 'ArrowRight':
                case 'ArrowDown':
                    selectQuestion((currentQuestionIndex + 1 + lsQuestion.length) % lsQuestion.length);
                    console.log(lsQuestion.length);
                    break;
                case 'ArrowLeft':
                case 'ArrowUp':
                    selectQuestion((currentQuestionIndex - 1 + lsQuestion.length) % lsQuestion.length);
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                    let selectedAnswer = parseInt(event.key);
                    handleChoose(currentQuestionIndex, selectedAnswer - 1);
                    console.log('Selected answer:', selectedAnswer);
                    break;
                default:
                    break;
            }
        };
        document.addEventListener('keyup', handleKeyDown);

        return () => {
            document.removeEventListener('keyup', handleKeyDown);
        };

    }, [lsQuestion, currentQuestionIndex])



    //Cập nhập font size
    useEffect(() => {
        updateFontSize();
    }, [fontSize])

    useEffect(() => {
        renderImage();
    }, [currentQuestionIndex, lsQuestion])

    function renderImage() {
        if (ques) {
            if (ques.Ttcaus.$values[0].Hinhcauhoi == "iQ==") {
                $(`#image${ques.IdCau}`).removeClass("hidden");
                axios.post(`https://localhost:7086/lythuyet/LayHinHCauHoi?id=${ques.IdCau}`)
                    .then(res => {
                        setQues((prevQues) => ({
                            ...prevQues,
                            Ttcaus: {
                                $values: [
                                    {
                                        ...prevQues.Ttcaus.$values[0],
                                        Hinhcauhoi: res.data //Hình ảnh mới
                                    }
                                ]
                            }
                        }));

                        setLsQuestion((prevLsQuestion) => {
                            const updatedLsQuestion = [
                                ...prevLsQuestion.slice(0, currentQuestionIndex),
                                {
                                    ...ques,
                                    Ttcaus: {
                                        $values: [
                                            {
                                                ...ques.Ttcaus.$values[0],
                                                Hinhcauhoi: res.data //Hình ảnh mới
                                            }
                                        ]
                                    }
                                },
                                ...prevLsQuestion.slice(currentQuestionIndex + 1)
                            ];
                            return updatedLsQuestion;
                        });
                    })
                    .catch(error => console.log(error));
                console.log(ques.Ttcaus.$values[0].Hinhcauhoi)

            }
            else if (ques.Ttcaus.$values[0].Hinhcauhoi == null) {
                $(`#image${ques.IdCau}`).addClass("hidden");
            }
            else {
                $(`#image${ques.IdCau}`).removeClass("hidden");

            }
        }
    };

    useEffect(() => {
        if (ques && lsChoose[currentQuestionIndex] !== -1) {
            var index = lsQuestion[currentQuestionIndex].Dapans.$values.find(e => e.Dapandung == true).IdDapan - 1;
            if (lsChoose[currentQuestionIndex] !== index) {
                $(".choose-val").removeClass("bg-[blue] bg-[red] text-white");
                $(".choose-val").eq(index).addClass("bg-[blue] text-white");
                $(".choose-val").eq(lsChoose[currentQuestionIndex]).addClass("bg-[red] text-white");

            }
            else {
                $(".choose-val").removeClass("bg-[blue] bg-[red] text-white");
                $(".choose-val").eq(index).addClass("bg-[blue] text-white");
            }
        }
        else {
            $(".choose-val").removeClass("bg-[blue] bg-[red] text-white");
        }
    }, [lsChoose, currentQuestionIndex]);


    const handleChoose = (curr, index) => {
        if (lsChoose[curr] !== index) {
            setLsChoose((prevLsChoose) => {
                const updatedLsChoose = [...prevLsChoose];
                updatedLsChoose[curr] = index;
                localStorage.setItem("lsChoose", JSON.stringify(updatedLsChoose)); // Lưu lsChoose vào localStorage
                return updatedLsChoose;
            });
        }

        if (autoSwitchEnabled) {
            setTimeout(() => {
                showQuestion(currentQuestionIndex + 1)
            }, 1000);
        }
    }

    const showListQuestion = () => {
        $('#questionOverlay').css('display', 'flex').animate({ opacity: 1 }, 300);
    }
    const closeListQuestion = () => {
        $('#questionOverlay').css('display', 'none');
    }

    const updateFontSize = () => {
        $('.choose-val').css('font-size', fontSize + 'em');
        $('.question-container').css('font-size', fontSize + 'em');
    };

    const showQuestion = (index) => {
        setCurrentQuestionIndex(index);
        setQues(lsQuestion[index]);
        updateFontSize();
    };


    const handleCheckboxChange = () => {
        setAutoSwitchEnabled(pre => !pre);
    };

    const selectQuestion = (index) => {
        setQues(lsQuestion[index]);
        setCurrentQuestionIndex(index);
    }

    return (
        <>

            <div id="questionOverlay" className="overlay hidden">
                <button className={"w-25 flex justify-center items-center"} id="closeOverlayBtn" onClick={closeListQuestion}>
                    <img width="64" height="64" src="https://img.icons8.com/nolan/64/exit.png" alt="exit" />
                </button>
                <div id="selectedQuestions" className="selected-questions w-75 h-100">
                    {(<div>{listChuong && lsQuestion && listChuong.find((valueChuong) => lsQuestion[0].IdChuong === valueChuong.idChuong).thongTinChuong}</div>)}
                    {
                        listChuong && lsQuestion && lsQuestion.map((value, index) => (
                            <>
                                {index > 0 && lsQuestion[index].IdChuong !== lsQuestion[index - 1].IdChuong ? (
                                    <div>{listChuong.find((valueChuong) => value.IdChuong === valueChuong.idChuong)?.thongTinChuong}</div>
                                ) : null}
                                <button
                                    style={{ width: '60px' }}
                                    className={`btn btn-${lsChoose[index] != -1 ? 'success' : 'secondary'} m-1 text-left`}
                                    onClick={() => selectQuestion(index)} // Use onClick instead of onclick
                                    key={`select_ques_${index}`} // Add a unique key for each button
                                >

                                    {index + 1}
                                </button>
                            </>
                        ))

                    }
                </div>
            </div>
            <div className="container">
                <div className="container">
                    <div className="container flex flex-col justify-center items-center">
                        <div className="container mb-4">
                            <SwitchComponent handleCheckboxChange={handleCheckboxChange} autoSwitchEnabled={autoSwitchEnabled} />
                            <div className="flex align-center">
                                <span className="self-center m-2"><i>Kích thước chữ: </i></span>
                                <button id="increase-font-size" onClick={() => { if (fontSize < 1.5) setFontSize(prev => prev + 0.1) }}>
                                    <img className="w-10 h-10" src="https://img.icons8.com/ultraviolet/40/plus--v3.png" alt="plus--v3" />
                                </button>
                                <button id="decrease-font-size" className="btn-lg btn-primary" onClick={() => { if (fontSize > 0.8) setFontSize(prev => prev - 0.1) }}>
                                    <img className="w-10 h-10" src="https://img.icons8.com/ultraviolet/40/minus.png" alt="minus" />
                                </button>
                            </div>

                        </div>

                        <div className="row m-2">
                            <button id="selectQuestionBtn" onClick={showListQuestion} className="btn btn-success">Chọn câu hỏi</button>
                        </div>

                        <div className="row" id="select_question">
                        </div>

                        <div className="row mt-4">
                            <button
                                className="btn btn-secondary m-1 flex justify-center"
                                id="prevBtn"
                                onClick={() => {
                                    if (currentQuestionIndex > 0) {
                                        setCurrentQuestionIndex(currentQuestionIndex - 1);
                                        setQues(lsQuestion[currentQuestionIndex - 1]);
                                        console.log(lsQuestion[currentQuestionIndex - 1]);
                                    }
                                }}
                            >
                                <TbPlayerTrackPrevFilled />
                            </button>

                            <button
                                className="btn btn-secondary m-1 flex justify-center"
                                id="nextBtn"
                                onClick={() => {
                                    if (currentQuestionIndex < lsQuestion.length - 1) {
                                        setCurrentQuestionIndex(currentQuestionIndex + 1);
                                        setQues(lsQuestion[currentQuestionIndex + 1]);
                                    }
                                }}
                            >
                                <TbPlayerTrackNextFilled />
                            </button>
                        </div>
                    </div>

                    {ques ? (
                        <div>
                            <div id="goiy">
                                {ques && lsChoose[currentQuestionIndex] !== -1 && ques.Ttcaus.$values[0].Goiy}
                            </div>

                            <div className="container mt-4" id="test_content">
                                <div className="container sm:w-75" id="content">
                                    <div className={`question-container ${ques.Ttcaus.$values[0].Diemliet ? 'text-danger' : ''}`}>
                                        <u><b>Câu hỏi {ques.IdCau}</b></u>: {ques.Ttcaus.$values[0].Cauhoi}
                                        <div className="flex justify-content-center">
                                            <img
                                                id={`image${ques.IdCau}`}
                                                className="py-4 rounded img-fluid w-md-50"
                                                src={ques.Ttcaus.$values[0].Hinhcauhoi ? `data:image/jpeg;base64,${ques.Ttcaus.$values[0].Hinhcauhoi}` : ""}
                                                alt="Hình ảnh câu hỏi"
                                            />
                                        </div>
                                        <div className="options button-group flex flex-col space-y-4">
                                            {ques.Dapans.$values.map((value, index) => (
                                                <div onClick={() => handleChoose(currentQuestionIndex, index)} key={index} className="choose-val w-full p-2 flex items-center space-x-4 border-solid m-2 border border-2">
                                                    <span className="index-span">{index + 1}</span>
                                                    <input type="radio" className="btn-check" name={`question${currentQuestionIndex}`} />
                                                    <label className="ml-2 w-100" htmlFor={`choice${currentQuestionIndex}-${index + 1}`}>{value.Dapan1}</label>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ) : (
                        <div className="loading-spinner-container">
                            <div className="loading-spinner"></div>
                        </div>
                    )}
                </div>
            </div>
        </>
    );
};

