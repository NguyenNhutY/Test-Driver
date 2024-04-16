import React from 'react';
import '../styles/about.css';
import Cursor from '../components/Cursor';

export function About() {
    return (
        <>

            <section className="section top-section">
                <div className="content-container content-theme-dark">
                    <div className="content-inner">
                        <div className="content-center">
                            <h1>DEMO GPLX</h1>
                            <p>Thank You So Much <a href="/#" target="_blank">Demo GPLX</a></p>
                        </div>
                    </div>
                </div>
            </section>
            <section className="section bottom-section">
                <div className="content-container content-theme-light">
                    <div className="content-inner">
                        <div className="content-center">
                            <h1>DEMO GPLX</h1>
                            <p>Thank You So Much <a href="/#" target="_blank">Demo GPLX</a></p>
                        </div>
                    </div>
                </div>
            </section>
        </>
    );
}

export default About;
