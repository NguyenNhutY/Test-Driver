import React, { useEffect } from 'react';
import { Possibility, Features, Info, Header } from '../containers';
import { CTA, BackToTopButton } from '../components';
import { motion, useAnimation } from "framer-motion";
import { useInView } from "react-intersection-observer";

// Thêm CSS cho chế độ tối
import '../styles/App.css';

const boxVariant = {
    visible: { opacity: 1, scale: 1, transition: { duration: 0.5 } },
    hidden: { opacity: 0, scale: 0 }
};

const Box = ({ children }) => {
    const control = useAnimation();
    const [ref, inView] = useInView()
    useEffect(() => {
        if (inView) {
            control.start("visible");
        } else {
            control.start("hidden");
        }
    }, [control, inView]);
    return (
        <motion.div
            className="box"
            ref={ref}
            variants={boxVariant}
            initial="hidden"
            animate={control}
        >
            {children}
        </motion.div>
    );
};

export function Home({ darkMode }) {
    return (
        <div className={` ${darkMode ? 'darkMode' : ''}`}>
   
            <div className="gradient__bg">
                <Header />
            </div>   
            <div >
                <BackToTopButton />
                <Box >
                    <Info />
                </Box>
                <Box>
                    <Features />
                </Box>
                <Box>
                    <Possibility />
                </Box>

                <Box>
                    <CTA />
                    </Box>
                </div>
        </div>
    );
}
