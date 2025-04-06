import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
    const [query, setQuery] = useState('');
    const [url, setUrl] = useState('');
    const [selectedEngine, setSelectedEngine] = useState("Google"); 
    const [result, setResult] = useState(null);
    const [error, setError] = useState('');
    const [searchHistory, setSearchHistory] = useState([]);
    const [showHistory, setShowHistory] = useState(false);


    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setResult(null);

        try {
            const response = await axios.post('/api/search', {
                 query, url, searchEngine: selectedEngine 
            });
            setResult(response.data);
        } catch (err) {
            console.error(err);
            setError('Error retrieving search results.');
        }
    };

    const fetchSearchHistory = async () => {
        try {
            const response = await axios.get('/api/search/history');
            setSearchHistory(response.data);
            setShowHistory(true);
        } catch (err) {
            console.error('Error fetching search history:', err);
            setError('Could not load search history.');
        }
    };

    return (
        <div className="wrapper">
            <div className="content-container">
                <h1 className="title">SEO Rank Tracker</h1>
                <p className="subtitle">
                    Enter your search keywords and domain to see where your site ranks in Google.
                </p>

                <form onSubmit={handleSubmit} className="search-form">
                    <div className="mb-4">
                        <label htmlFor="query" className="form-label fs-5 fw-bold">
                            Search keywords:
                        </label>
                        <input
                            type="text"
                            id="query"
                            className="form-control form-control-lg"
                            placeholder='e.g. "land registry searches"'
                            value={query}
                            onChange={(e) => setQuery(e.target.value)}
                            required
                        />
                    </div>

                    <div className="mb-4">
                        <label htmlFor="url" className="form-label fs-5 fw-bold">
                            Website domain (without http/https):
                        </label>
                        <div className="input-group input-group-lg">
                            <span className="input-group-text">https://</span>
                            <input
                                type="text"
                                id="url"
                                className="form-control"
                                placeholder="infotrack.co.uk"
                                value={url}
                                onChange={(e) => setUrl(e.target.value)}
                                required
                            />
                        </div>
                    </div>

                 
                    <div className="mb-4">
                        <label className="form-label fs-5 fw-bold">Select search engine:</label>
                        <div className="d-flex">
                            <div className="form-check me-4">
                                <input
                                    className="form-check-input"
                                    type="radio"
                                    name="searchEngine"
                                    id="googleRadio"
                                    value="Google"
                                    checked={selectedEngine === "Google"}
                                    onChange={() => setSelectedEngine("Google")}
                                />
                                <label className="form-check-label" htmlFor="googleRadio">
                                    <img
                                        src="/images/google_icon-icons.webp"
                                        alt="Google"
                                        style={{ width: "32px", height: "32px", marginRight: "8px" }}
                                    />
                                    Google
                                </label>
                            </div>
                            <div className="form-check">
                                <input
                                    className="form-check-input"
                                    type="radio"
                                    name="searchEngine"
                                    id="bingRadio"
                                    value="Bing"
                                    checked={selectedEngine === "Bing"}
                                    onChange={() => setSelectedEngine("Bing")}
                                />
                                <label className="form-check-label" htmlFor="bingRadio">
                                    <img
                                        src="/images/bing_icon.webp"
                                        alt="Bing"
                                        style={{ width: "32px", height: "32px", marginRight: "8px" }}
                                    />
                                    Bing
                                </label>
                            </div>
                        </div>
                    </div>

                    <button type="submit" className="btn-submit">
                        Search url positions
                    </button>
                    <button type="button" className="btn-read-history" onClick={fetchSearchHistory}>
                        Read past queries
                    </button>                   
                </form>

                {error && (
                    <div className="alert alert-error" role="alert">
                        {error}
                    </div>
                )}

                {result && (
                    <div className="results-card">
                        <h3>Search Results</h3>
                        {result.Positions && result.Positions.length > 0 ? (
                            <p>
                                Found at positions: <strong>{result.Positions.join(', ')}</strong>
                            </p>
                        ) : (
                            <p>No results found within the top 100.</p>
                        )}
                    </div>
                )}
                {showHistory && (
                    <div className="results-card mt-4">
                        <h3>Past Queries</h3>
                        {searchHistory.length > 0 ? (
                            <ul className="list-group">
                                {searchHistory.map((item, index) => (
                                    <li key={index}>
                                        <strong>Id:</strong> {item.Id},<strong>Query:</strong> {item.Query}, <strong>URL:</strong> {item.Url}, <strong>Engine:</strong> {item.SearchEngine}, <strong>Positions:</strong> {item.Positions},
                                        <strong>CreatedAt:</strong> {item.CreatedAt}
                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p>No search history available.</p>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}

export default App;
