import React from 'react';
import logo from './../assets/logo.png';
import "./../styles/Footer.css";

const FooterPage = () => {
    return (
        <footer className="bg-gray-800 text-white py-20 mt-5">
            <div className="new_footer_top">
                <div className="footer_bg">
                    <div className="footer_bg_one"></div>
                    <div className="footer_bg_two"></div>
                </div>
            </div>
            <div className="container mx-auto pt-10 flex flex-wrap justify-center align-center items-center">
                <div className="flex items-center mx-2">
                    <img src={logo} alt="Logo" className="h-8 mr-4" />
                    <p className="text-sm">© 2024 GPLX_STU All Rights Reserved.</p>
                </div>
                <div className="flex items-center space-x-6 mx-2">
                    <FooterLink href="#" text="Home" />
                    <FooterLink href="/hoc-ly-thuyet" text="Học lý thuyết" />
                    <FooterLink href="/thi-ly-thuyet" text="Thi thử" />
                </div>
            </div>
        </footer>
    );
};

const FooterLink = ({ href, text }) => {
    return (
        <a href={href} className="text-base leading-6 hover:text-gray-300 hover:underline active:text-red-300">{text}</a>
    );
};

export default FooterPage;
