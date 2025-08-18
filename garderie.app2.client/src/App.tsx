//import {
//    BrowserRouter as Router,
//    Routes,
//    Route
//} from "react-router-dom";
//import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from './Home';
import User from './User';
import Daycare from './Daycare';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
const App = () => {
    return (
        <Router>
            {/*<Navbar />*/}
            {/*Router page*/}
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/user/:userId" element={<User />} />
                <Route path="/daycare/:daycareId" element={<Daycare />} />
            </Routes>
        </Router>
    );
};

export default App;