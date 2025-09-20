import React, { useEffect, useState, useRef } from 'react';
import axios from "axios";
import '../App.css';
import { Link } from 'react-router-dom';

interface User {
    id: number;
    name: string;
    email: string;
    password: string;
}

function App() {
    const [users, setUsers] = useState<User[]>();
    const [showForm, setShowForm] = useState(false);
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [userName, setUserName] = useState("");
    //const [userId, setUserId] = useState("");

    const nameRef = useRef<HTMLInputElement>(null);
    const emailRef = useRef<HTMLInputElement>(null);

    const handleButtonClickShowUserForm = () => {
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    useEffect(() => {
        populateUsersData();
        isUserAuthenticated();
        //if (isLoggedIn) {
            getUserName();
            //getUserId();
        //}
    }, []);

    const contents = users === undefined
        ? <i className="pi pi-spin pi-spinner"></i>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Password</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user =>
                    <tr key={user.id}>
                        <td>{user.id}</td>
                        <td><Link to={"/user/" + user.id}> {user.name} </Link></td>
                        <td>{user.email}</td>
                        <td>{user.password}</td>
                        <td> <RemoveUser id={user.id} /> </td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            {/*{isLoggedIn == true && <> Hello {userName} {userId} <br /></>}*/}
            {isLoggedIn == false && <><a href="/user/register">Register</a><br /></>}
            {isLoggedIn == false && <><a href="/user/login">Login</a><br /></>}
            {isLoggedIn == true && <> Hello {userName} <br /></>}
            {isLoggedIn == true && <ButtonLogout />}
            <h1 id="tableLabel">Teachers</h1>
            <ButtonAddAutoUser />
            <ButtonShowFormUser />
            {contents}
        </div>
    );

    function isUserAuthenticated() {
        axios
            .get("/authentication/isuserauthenticated", {
            })
            .then((response) => {
                console.log("IsUserAuthenticated:", response.data);
                const boolValue = (response.data.toLowerCase() === "true"); // true
                setIsLoggedIn(boolValue);
            })
            .catch((error) => {
                console.error("Error IsUserAuthenticated:", error);
            });
    }

    function getUserName() {
        axios
            .get("/authentication/getusername", {
            })
            .then((response) => {
                console.log("GetUserName:", response.data);
                setUserName(response.data);
            })
            .catch((error) => {
                console.error("Error GetUserName:", error);
            });
    }

    //function getUserId() {
    //    axios
    //        .get("/authentication/getuserid", {
    //        })
    //        .then((response) => {
    //            console.log("GetUserId:", response.data);
    //            setUserId(response.data);
    //        })
    //        .catch((error) => {
    //            console.error("Error GetUserId:", error);
    //        });
    //}

    async function populateUsersData() {
        const response = await fetch('/api/users');
        if (response.ok) {
            const data = await response.json();
            setUsers(data);
        }
    }

    function refreshPage() {
        window.location.reload();
    }

    function ButtonLogout() {
        async function handleClick() {
            axios
                .post("/authentication/logout", {
                })
                .then((response) => {
                    console.log("User logged out:", response.data);
                    //isUserAuthenticated();
                    refreshPage();
                })
                .catch((error) => {
                    console.error("Error logging out user:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                Logout
            </button>
        );
    }

    function ButtonAddAutoUser() {
        async function handleClick() {
            axios
                .post("/api/users", {
                    name: "test",
                    email: "test@test.com",
                    password: "test123"
                })
                .then((response) => {
                    console.log("User created:", response.data);
                    populateUsersData();
                })
                .catch((error) => {
                    console.error("Error creating user:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                Add auto user
            </button>
        );
    }

    function RemoveUser({ id }: {id: number}) {
        async function handleClick() {
            axios
                .delete("/api/users/"+id, {
                    //id: 2
                })
                .then((response) => {
                    console.log("User deleted:", response.data);
                    populateUsersData();
                })
                .catch((error) => {
                    console.error("Error deleting user:", error);
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

    function ButtonShowFormUser() {
        async function handleClick(event: React.MouseEvent) {
            event.preventDefault();
            axios
                .post("/api/users", {
                    name: nameRef.current?.value ?? "",
                    email: emailRef.current?.value ?? "",
                    password: "test123"
                })
                .then((response) => {
                    console.log("User created:", response.data);
                    populateUsersData();
                    setShowForm(false);
                })
                .catch((error) => {
                    console.error("Error creating user:", error);
                });
        }

        return (
            <div>
                <button onClick={handleButtonClickShowUserForm}
                    className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
                >Show Form to add user</button>
                {showForm && (
                    <form>
                        <label>
                            Name:
                            <input type="text" name="name" defaultValue="test" ref={nameRef}
                            className="outline"
                            />
                        </label>
                        <br />
                        <label>
                            Email:
                            <input type="email" name="email" defaultValue="test@test.com" ref={emailRef}
                            className="outline"
                            />
                        </label>
                        {/*<input type="hidden" name="password" value="test123" />*/}
                        <br />
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