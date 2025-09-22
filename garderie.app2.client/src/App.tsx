//import {
//    BrowserRouter as Router,
//    Routes,
//    Route
//} from "react-router-dom";
//import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from './components/Home';
import User from './components/User';
import Register from './components/Register';
import Login from './components/Login';
import Daycare from './components/Daycare';
import Kid from './components/Kid';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
const App = () => {
    return (
        <Router>
            {/*<Navbar />*/}
            {/*Router page*/}
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/user/register" element={<Register />} />
                <Route path="/user/login" element={<Login />} />
                <Route path="/user/:userId" element={<User />} />
                <Route path="/daycare/:daycareId" element={<Daycare />} />
                <Route path="/daycare/:daycareId/kid/:kidId" element={<Kid />} />
            </Routes>
        </Router>
    );
};

export default App;