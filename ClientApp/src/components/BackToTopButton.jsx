import React, { useEffect, useState } from 'react';
import "../styles/App.css"
import "../styles/backtotopbutton.css"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowUp } from '@fortawesome/free-solid-svg-icons';
function BackToTopButton() {
    const [backToTopButton, setBackToTopButton] = useState(false);

    useEffect(() => {
        window.addEventListener("scroll", () => {
            if (window.scrollY > 100) {
                setBackToTopButton(true);
            } else {
                setBackToTopButton(false);
            }
        });

        // Clean up event listener on unmount
        return () => {
            window.removeEventListener("scroll", () => { });
        };
    }, []);

    const scrollup = () => {
        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    };

    return (
        <div className="App">
            {backToTopButton && (
                <button className ="backtotopbutton" onClick={scrollup}>
                    <FontAwesomeIcon icon={faArrowUp} className="fontawe" /></button>
            )}
        </div>
    );
}

export default BackToTopButton;
