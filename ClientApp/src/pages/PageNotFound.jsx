import React from "react";
import svg from "../assets/404.svg";
import '../styles/PageNotFound.css';
import { Link } from "react-router-dom";

export function PageNotFound() {

    // Kiểm tra kết nối mạng trước khi load trang
    window.addEventListener('load', () => {
        if (!navigator.onLine) {
            // Nếu mất kết nối, chuyển hướng sang trang "Page Not Found"
            window.location.href = '/*';
        }
    });


    return (
        <>
            <div className="cont-404">
                <img src={svg} alt="svg" />
                <button className="button" ><Link to="/">Please Try to F5</Link></button>
            </div>
        </>
    );
};
export default PageNotFound;
