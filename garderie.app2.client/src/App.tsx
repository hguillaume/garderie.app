//import {
//    BrowserRouter as Router,
//    Routes,
//    Route
//} from "react-router-dom";
//import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from './Home';
import User from './User';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
const App = () => {
    return (
        <Router>
            {/*<Navbar />*/}
            {/*Router page*/}
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/user/:id" element={<User />} />
            </Routes>
        </Router>
    );
};

export default App;