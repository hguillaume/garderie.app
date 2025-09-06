import { useEffect, useState, useRef } from 'react';
import axios from "axios";
import './App.css';
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

    const nameRef = useRef();
    const emailRef = useRef();

    const handleButtonClickShowUserForm = () => {
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    useEffect(() => {
        populateUsersData();
    }, []);

    const contents = users === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
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
            <h1 id="tableLabel">Users</h1>
            <ButtonAddAutoUser />
            <ButtonShowFormUser />
            {contents}
        </div>
    );

    async function populateUsersData() {
        const response = await fetch('/api/users');
        if (response.ok) {
            const data = await response.json();
            setUsers(data);
        }
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

    function RemoveUser({ id }) {
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
        async function handleClick(event) {
            //alert("test");
            event.preventDefault();
            axios
                .post("/api/users", {
                    name: nameRef.current.value,
                    email: emailRef.current.value,
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