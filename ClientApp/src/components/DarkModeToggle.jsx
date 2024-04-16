import React, { useState, useEffect } from "react";
import { ReactComponent as Sun } from '../assets/Sun.svg';
import { ReactComponent as Moon } from '../assets/Moon.svg';
import '../styles/NavMenu.css';

const DarkModeToggle = ({ darkMode, toggleDarkMode }) => {
    // Sử dụng useEffect để cập nhật style của body khi darkMode thay đổi
    useEffect(() => {
        // Nếu darkMode được bật, thiết lập màu nền của body thành #212121
        if (darkMode) {
            document.body.style.backgroundColor = "#212121";
        } else {
            document.body.style.backgroundColor = "";
// Xóa lớp dark-mode khỏi body
        }

    }, [darkMode]); // Theo dõi sự thay đổi của darkMode

    return (
        <div>
            <input type="checkbox" id="darkmode-toggle" checked={darkMode} onChange={toggleDarkMode} />
            <label className="darkmode-label" htmlFor="darkmode-toggle">
                <Sun />
                <Moon />
            </label>
        </div>
    );
};

export default DarkModeToggle;