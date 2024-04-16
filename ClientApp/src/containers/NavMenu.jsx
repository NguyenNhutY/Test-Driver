
import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../styles/NavMenu.css';
import logo from './../assets/logo.png';
import DarkModeToggle from "../components/DarkModeToggle";

export class NavMenu extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showModal: false,
            selectedLicenseType: {},
            licenseTypes: [],
            darkMode: false,
            check: false,
            inputcheck: "",
            collapsed: true // Thêm trạng thái cho việc ẩn hiện thanh điều hướng trên di động
        };
    }

    componentDidMount() {
        // Fetch dữ liệu về loại giấy phép lái xe
        fetch('https://localhost:7086/cauhoi/gettypeformobile')
            .then(response => response.json())
            .then(data => {
                this.setState({ licenseTypes: data });
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            });

        // Kiểm tra và lấy dữ liệu loại giấy phép đã chọn từ LocalStorage
        const selectedLicenseType = JSON.parse(localStorage.getItem("HANG"));
        if (selectedLicenseType) {
            this.setState({ selectedLicenseType: selectedLicenseType });
        }
    }

    toggleNavbar = () => {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    openModal = () => {
        this.setState({
            showModal: true
        });
    }

    // Phương thức chuyển đổi chế độ dark mode
    toggleDarkMode = () => {
        this.setState(prevState => ({
            darkMode: !prevState.darkMode,
        }));
    };


    submitForm = () => {
        const { selectedLicenseType, inputcheck } = this.state;

        if (selectedLicenseType) {
            if (inputcheck.toLowerCase() === selectedLicenseType.thongtin.toLowerCase()) {
                if (selectedLicenseType.idHang)
                    localStorage.setItem('HANG', JSON.stringify(selectedLicenseType));
                this.setState({ showModal: false });
                window.location.reload(false);
            } else {
                alert('Nội dung nhập vào không khớp với hạng đã chọn');
            }
        } else {
            alert('Vui lòng chọn một hạng GPLX');
        }
    }

    // Hàm xử lý khi người dùng nhấn OK trong modal
    checked = (e) => {
        this.setState({
            inputcheck: e.target.value
        });
    }
 

    render() {
        const { licenseTypes } = this.state;

        // Kiểm tra nếu licenseTypes chưa được thiết lập
        if (licenseTypes.length === 0) {
            return <div>Loading...</div>; // Hiển thị một thông báo tải
        }  

        return (
            <header>
                <Navbar className={`navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3 ${this.state.darkMode ? 'textwhite' : ''}`} container light>
                    <div className="navbar-links_logo">
                        <img src={logo} alt="logo" />
                    </div>
                    <NavbarBrand className = "" tag={Link} to="/">DemoGPLX</NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className=" pt-3 text-nav  text-dark" to="/thi-ly-thuyet">Thi thử</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className=" pt-3 text-nav  text-dark" to="/hoc-ly-thuyet">Học lý thuyết</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className=" pt-3 text-nav  text-dark" to="/about">Thanks</NavLink>
                            </NavItem>
                            <NavItem className=" pt-2">
                                <button className="ml-5 btn btn-primary" onClick={() => this.openModal()}>{this.state.selectedLicenseType.thongtin != undefined ? "HẠNG: " + this.state.selectedLicenseType.thongtin : "VUI LÒNG CHỌN HẠNG"} </button>
                            </NavItem>
                            <NavItem>
                                {/* Sử dụng DarkModeToggle component */}
                                <DarkModeToggle darkMode={this.state.darkMode} toggleDarkMode={this.toggleDarkMode} />
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>

                {/* Modal chọn hạng GPLX */}
                {this.state.showModal && (
                    <div className="fixed inset-0 z-50 flex items-center justify-center overflow-auto bg-gray-800 bg-opacity-75">
                        <div className="ant-modal" style={{ width: '520px', transformOrigin: '506px -69px' }}>
                            <div className="ant-modal-content bg-white rounded-lg shadow-xl">
                                {/* Header */}
                                <div className="ant-modal-header bg-gray-200 rounded-t-lg p-4">
                                    <div className="ant-modal-title" id="rcDialogTitle0">Chọn hạng GPLX</div>
                                </div>
                                {/* Body */}
                                <div className="ant-modal-body p-4">
                                    {this.state.licenseTypes.map(licenseType => (
                                        <button
                                            key={licenseType.idHang}
                                            type="button"
                                            className={`w-24 h-12 m-1 ${parseInt(licenseType.idHang) === parseInt(this.state.selectedLicenseType.idHang) ? 'bg-gray-500 text-white' : 'bg-gray-100'} hover:bg-gray-500 hover:text-white`}
                                            onClick={() => this.setState({ selectedLicenseType: licenseType })}
                                        >
                                            <span>{licenseType.thongtin}</span>
                                        </button>
                                    ))}
                                </div>
                                {/* Footer */}
                                <div className="bg-gray-200 rounded-b-lg p-4 flex justify-around">
                                    <input type="text hover:bg-gray-500 hover:text-white" onChange={this.checked} value={this.state.inputcheck}></input>
                                    <button type=" button hover:bg-gray-500 hover:text-white" onClick={this.submitForm}><span>OK</span></button>
                                </div>
                            </div>
                        </div>
                    </div>
                )}

            </header>
        );
    }
}
