import { useState, useEffect } from 'react';

// Custom hook to fetch data from an API
function useFetch(url) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        async function fetchData() {
            try {
                const response = await fetch(url);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const jsonData = await response.json();
                setData(jsonData);
                setLoading(false);
            } catch (error) {
                setError(error);
                setLoading(false);
            }
        }

        fetchData();

        return () => {
        };
    }, [url]); 
    return { data, loading, error };
}

function Hook() {
    const { data, loading, error } = useFetch('https://api.example.com/data');

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error.message}</div>;
    }

    return (
        <div>
            {data && (
                <ul>
                    {data.map(item => (
                        <li key={item.id}>{item.name}</li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default Hook;
