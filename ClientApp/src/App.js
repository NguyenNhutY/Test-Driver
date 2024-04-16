import React, { useState, useEffect } from 'react';
import { Routes, Route } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import {Layout} from './containers/Layout';
import './styles/custom.scss';
import Loading from "./pages/Loading";

const App = () => {
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        // Simulate an API call
        setTimeout(() => {
            setIsLoading(false);
        }, 1000);
    }, []);
    return (
        isLoading ? (
            <Loading />
        ) : (
    
                    <Layout>
                        <Routes>
                            {AppRoutes.map((route, index) => {
                                const { element, ...rest } = route;
                                return <Route key={index} {...rest} element={element} />;
                            })}
                        </Routes>
                    </Layout>
        )
    );
}
export default App;
