import React, { useEffect, useState, useRef } from 'react';
import axios from "axios";
import '../App.css';
import { useParams, Link } from 'react-router-dom';

interface User {
    id: number;
    name: string;
    email: string;
    password: string;
}

interface Daycare {
    id: number;
    userId: number;
    name: string;
}

function App() {
    const { userId } = useParams(); // Access the `id` parameter from the route
    const [user, setUser] = useState<User>();
    const [daycares, setDaycares] = useState<Daycare[]>();
    const [showForm, setShowForm] = useState(false);

    const nameRef = useRef<HTMLInputElement>(null);

    const handleButtonClickShowDaycareForm = () => {
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    useEffect(() => {
        populateUserData();
        populateDaycaresData();
    }, []);

    const contents = daycares === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>UserId</th>
                    <th>Name</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                {daycares.map(daycare =>
                    <tr key={daycare.id}>
                        <td>{daycare.id}</td>
                        <td>{daycare.userId}</td>
                        <td><Link to={"/daycare/" + daycare.id}> {daycare.name} </Link></td>
                        <td> <RemoveDaycare id={daycare.id} /> </td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            {/*Hello {id}*/}
            Hello {user ? user.name : <i className='pi pi-spin pi-spinner'></i>}
            <br /><ButtonAddAutoDaycare />
            <br /><ButtonShowFormDaycare />
            {contents}
        </div>
    );

    async function populateUserData() {
        axios
            .get("/api/users/" + userId, {
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

    async function populateDaycaresData() {
        axios
            .get("/api/daycares/", {
                //user_id: userId
            })
            .then((response) => {
                console.log("Request:", response.request);
                console.log("User data:", response.data);
                setDaycares(response.data);
            })
            .catch((error) => {
                console.error("Error getting user data:", error);
            });
    }

    function ButtonAddAutoDaycare() {
        async function handleClick() {
            axios
                .post("/api/daycares", {
                    name: "test",
                    userId: userId
                })
                .then((response) => {
                    console.log("Daycare created:", response.data);
                    populateDaycaresData();
                })
                .catch((error) => {
                    console.error("Error creating daycare:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                Add auto daycare
            </button>
        );
    }

    function RemoveDaycare({ id }: {id: number}) {
        async function handleClick() {
            axios
                .delete("/api/daycares/" + id, {
                    //id: 2
                })
                .then((response) => {
                    console.log("Daycare deleted:", response.data);
                    populateDaycaresData();
                })
                .catch((error) => {
                    console.error("Error deleting daycare:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                <i className="pi pi-trash red" style={{ color: 'red' }}></i>
            </button>
        );
    }

    function ButtonShowFormDaycare() {
        async function handleClick(event: React.MouseEvent) {
            //alert("test");
            event.preventDefault();
            axios
                .post("/api/daycares", {
                    name: nameRef.current?.value ?? "",
                    userId: userId
                    //email: emailRef.current.value,
                    //password: "test123"
                })
                .then((response) => {
                    console.log("User created:", response.data);
                    populateDaycaresData();
                    setShowForm(false);
                })
                .catch((error) => {
                    console.error("Error creating user:", error);
                });
        }

        return (
            <div>
                <button onClick={handleButtonClickShowDaycareForm}
                    className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
                >Show Form to add daycare</button>
                {showForm && (
                    <form>
                        <label>
                            Name:
                            <input type="text" name="name" defaultValue="test" ref={nameRef}
                                className="outline"
                            />
                        </label>
                        <br />
                        {/*<label>*/}
                        {/*    Email:*/}
                        {/*    <input type="email" name="email" defaultValue="test@test.com" ref={emailRef}*/}
                        {/*        className="outline"*/}
                        {/*    />*/}
                        {/*</label>*/}
                        {/*<input type="hidden" name="password" value="test123" />*/}
                        {/*<br />*/}
                        <button type="submit" onClick={handleClick}
                            className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
                        >Submit</button>
                    </form>
                )}
            </div>
        );
    }
}

export default App;