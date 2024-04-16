import React, { useState, useEffect, memo } from 'react';
import axios from 'axios';
import $ from 'jquery';
import { TbPlayerTrackNextFilled, TbPlayerTrackPrevFilled } from "react-icons/tb";
import { Cursor  } from "../components"

const SwitchComponent = memo(({ handleCheckboxChange, autoSwitchEnabled }) => {
    return (
        <div className="form-check form-switch text-lg">
            <label htmlFor="auto-switch" className="form-check-label" id="iswitch"> {autoSwitchEnabled ? "Tự động chuyển câu: Bật" : "Tự động chuyển câu: Tắt"} </label>
            <input onChange={handleCheckboxChange} type="checkbox" id="auto-switch" className="form-check-input" checked={autoSwitchEnabled} />
        </div>
    );
});

export const ThiLT = () => {

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
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(-1);

    //Tự động chuyển câu (T/F)
    const [autoSwitchEnabled, setAutoSwitchEnabled] = useState(false);

    //Font size (Float)
    const [fontSize, setFontSize] = useState(1);

    //Thông tin câu hỏi (Object)
    const [ques, setQues] = useState();

    const [endTest, setEndTest] = useState(false);

    const [lsChoose, setLsChoose] = useState([]);

    const [timeLeft, setTimeLeft] = useState(() => {
        var hangJson = localStorage.getItem('HANG');

        var hang = JSON.parse(hangJson);

        return hang != null ? hang.thoigianthi * 60 : 1;

    }); // Thời gian còn lại trong bài thi (tính theo giây)
    const [timerActive, setTimerActive] = useState(false); // Trạng thái của đồng hồ đếm ngược

    // Hàm bắt đầu đếm ngược
    const startTimer = () => {
        setTimerActive(true);
    };

    // Hàm dừng đếm ngược
    const stopTimer = () => {
        setTimerActive(false);
        setTimeLeft(0);
    };

    // Hàm cập nhật thời gian còn lại
    useEffect(() => {
        let interval;
        if (timerActive) {
            interval = setInterval(() => {
                setTimeLeft(prevTime => prevTime - 1);
            }, 1000);
        } else {
            clearInterval(interval);
        }

        // Xử lý khi thời gian còn lại bằng 0
        if (timeLeft === 0) {
            setTimerActive(false);
            alert("Hết thời gian làm bài!");
            handleSubmit(); // Tự động nộp bài khi hết thời gian
        }

        return () => clearInterval(interval);
    }, [timerActive, timeLeft]);

    //Tải câu hỏi và init giá trị cho page
    useEffect(() => {
        if (classGPLX === '') {
            alert("Vui lòng chọn hạng");
            window.location.href = '/';
        }
        const fetchData = async () => {
            try {
                const response = await axios.post(`https://localhost:7086/lythuyet/laydethimoi?id=` + classGPLX);
                const questions = response.data.$values;
                setLsQuestion(questions);
                setQues(questions[0]);
                setCurrentQuestionIndex(0);
                setLsChoose(Array(questions.length).fill(-1));
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

                    if (currentQuestionIndex < lsQuestion.length - 1) {
                        selectQuestion(currentQuestionIndex + 1);
                    }
                    break;
                case 'ArrowLeft':
                case 'ArrowUp':

                    if (currentQuestionIndex > 0) {
                        selectQuestion(currentQuestionIndex - 1);
                    }
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
    }, [currentQuestionIndex, ques, lsQuestion])

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
        if (endTest == true) {
            var index = lsQuestion[currentQuestionIndex].Dapans.$values.find(e => e.Dapandung == true).IdDapan - 1;
            if (lsChoose[currentQuestionIndex] != index) {
                $(".choose-val").removeClass("bg-[blue] bg-[red] text-white");
                $(".choose-val").eq(index).addClass("bg-[blue] text-white");
                if (lsChoose[currentQuestionIndex] != -1)
                    $(".choose-val").eq(lsChoose[currentQuestionIndex]).addClass("bg-[red] text-white");

            }
            else {
                $(".choose-val").removeClass("bg-[blue] bg-[red] text-white");
                $(".choose-val").eq(index).addClass("bg-[blue] text-white");
            }

        }
        else {
            $(".choose-val").removeClass("bg-[blue] bg-[red] text-white");
            if (lsChoose[currentQuestionIndex] != -1)
                $(".choose-val").eq(lsChoose[currentQuestionIndex]).addClass("bg-[blue] text-white");

        }
        console.log(lsChoose);
    }, [lsChoose, currentQuestionIndex, endTest]);


    const handleChoose = (curr, index) => {
        if (lsChoose[curr] != index)
            setLsChoose((prevLsChoose) => {
                const updatedLsChoose = [...prevLsChoose];
                updatedLsChoose[curr] = index;
                return updatedLsChoose;
            });


        if (autoSwitchEnabled) {
            setTimeout(() => {
                if (currentQuestionIndex < lsQuestion.length - 1)
                    showQuestion(currentQuestionIndex + 1)
            }, 1000);
        }
    }

    const closeListQuestion = () => {
        $('#questionOverlay').css('display', 'none').animate({ opacity: 1 }, 300);
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
        closeListQuestion();
        setQues(lsQuestion[index]);
        setCurrentQuestionIndex(index);

    }

    const handleSubmit = () => {
        let count = 0;
        setEndTest(true);
        lsChoose.map((value, index) => {
            let ind = lsQuestion[index].Dapans.$values.find(e => e.Dapandung == true).IdDapan;
            if (value + 1 == ind) {
                count++;
            }
            else
                if (lsQuestion[index].Ttcaus.$values[0].Diemliet == true) {
                    if (value + 1 != ind) {
                        count = -100;
                    }
                    else {
                        count++;
                    }
                }
        });
        if (count < 0) {
            alert("BẠN ĐÃ THI RỚT");
        }
        else {
            alert("Bạn làm đúng: " + count + " câu");
        }
    };

    console.log("render");

    return (
        <>

            <div id="questionOverlay" className="overlay hidden">
                <button className="w-25 flex justify-center items-center" id="closeOverlayBtn" onClick={closeListQuestion}>
                    <img width="64" height="64" src="https://img.icons8.com/nolan/64/exit.png" alt="exit" />
                </button>
            </div>
            <div className="container">
                <div className="container">
                    <div className="container flex flex-col justify-center items-center">

                        <div id="selectedQuestions" className="selected-questions w-100 h-100">
                            {lsQuestion && lsChoose.map((value, index) => (
                                <button
                                    style={{ width: '50px', height: '50px' }}
                                    className={`btn btn-${value !== -1 ? 'success' : 'secondary'} m-1 text-left text-center`}
                                    onClick={() => selectQuestion(index)}
                                    key={`select_ques_${index}`}
                                >
                                    {index + 1}
                                </button>
                            ))}
                        </div>
                        <div className="container mb-4">
                            <SwitchComponent handleCheckboxChange={handleCheckboxChange} autoSwitchEnabled={autoSwitchEnabled} />
                            <span className="self-center"><i>Kích thước chữ: </i></span>
                            <button id="increase-font-size" onClick={() => { if (fontSize < 1.5) setFontSize(prev => prev + 0.1) }}>
                                <img className="w-10 h-10" src="https://img.icons8.com/ultraviolet/40/plus--v3.png" alt="plus--v3" />
                            </button>
                            <button id="decrease-font-size" className="btn-lg btn-primary" onClick={() => { if (fontSize > 0.8) setFontSize(prev => prev - 0.1) }}>
                                <img className="w-10 h-10" src="https://img.icons8.com/ultraviolet/40/minus.png" alt="minus" />
                            </button>
                        </div>

                        <div className="row mt-4">
                            <button
                                className="btn btn-secondary m-1 flex justify-center"
                                id="prevBtn"
                                onClick={() => {
                                    if (currentQuestionIndex > 0) {
                                        selectQuestion(currentQuestionIndex - 1);
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
                                        selectQuestion(currentQuestionIndex + 1)
                                    }
                                }}
                            >
                                <TbPlayerTrackNextFilled />
                            </button>
                        </div>

                        {!endTest && timerActive && (
                            <div className="row mt-4">
                                <button className="btn btn-danger" onClick={handleSubmit}>Nộp bài</button>
                            </div>
                        )}

                        {timerActive ? (
                            ques ? (
                                <div className="container mt-4" id="test_content">
                                    <div className="container sm:w-75" id="content">
                                        <div className={`question-container`}>
                                            <u><b>Câu hỏi {currentQuestionIndex + 1}</b></u>: {ques.Ttcaus.$values[0].Cauhoi}
                                            <div className="flex justify-content-center">
                                                <img
                                                    id={`image${ques.IdCau}`}
                                                    className="py-4 rounded img-fluid w-md-50 hidden"
                                                    src={ques.Ttcaus.$values[0].Hinhcauhoi ? `data:image/jpeg;base64,${ques.Ttcaus.$values[0].Hinhcauhoi}` : ""}
                                                    alt="Hình ảnh câu hỏi"
                                                />
                                            </div>
                                            <div className="options button-group flex flex-col space-y-4">
                                                {ques.Dapans.$values.map((value, index) => (
                                                    <div onClick={() => handleChoose(currentQuestionIndex, index)} key={index} className={`choose-val w-full p-2 flex items-center space-x-4 border-solid m-2 border border-2`}>
                                                        <span className="index-span">{index + 1}</span>
                                                        <input type="radio" className="btn-check" name={`question${currentQuestionIndex}`} />
                                                        <label className="ml-2 w-100" htmlFor={`choice${currentQuestionIndex}-${index + 1}`}>{value.Dapan1}</label>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>
                                    </div>
                                    {(timerActive && !endTest) && (
                                        <span className="ml-2">Thời gian còn lại: {Math.floor(timeLeft / 60)}:{(timeLeft % 60).toString().padStart(2, '0')}</span>
                                    )}
                                </div>
                            ) : (
                                <div className="loading-spinner-container">
                                    <div className="loading-spinner"></div>
                                </div>
                            )
                        ) : (
                            <div className="container flex align-center justify-center">
                                <button className="btn btn-success m-1" onClick={startTimer}>Bắt đầu làm bài</button>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
};