import { useEffect, useState, useRef } from 'react';
import axios from "axios";
import './App.css';
import { useParams } from 'react-router-dom';

interface User {
    id: number;
    name: string;
    email: string;
    password: string;
}

function App() {
    const { id } = useParams(); // Access the `id` parameter from the route
    const [user, setUser] = useState<User>();

    useEffect(() => {
        populateUserData();
    }, []);

    return (
        <div>
            {/*Hello {id}*/}
            Hello {user ? user.name : <i className='pi pi-spin pi-spinner'></i>}
            <br />Todo: Create a new daycare
            <br />Todo: List all daycares
        </div>
    );

    async function populateUserData() {
        axios
            .get("/api/users/" + id, {
                //id: 2
            })
            .then((response) => {
                console.log("Request:", response.request);
                console.log("User data:", response.data);
                setUser(response.data);
            })
            .catch((error) => {
                console.error("Error getting user data:", error);
            });
    }
}

export default App;