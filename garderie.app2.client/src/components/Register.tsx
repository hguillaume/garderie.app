import { useEffect, useState, useRef } from 'react';
import axios from "axios";
import '../App.css';
import { Link } from 'react-router-dom';
//import { error } from 'console';

interface User {
    id: number;
    name: string;
    email: string;
    password: string;
}

function App() {
    const [users, setUsers] = useState<User[]>();
    const [showForm, setShowForm] = useState(true);
    const [registerStatus, setRegisterStatus] = useState("");
    const [listItems, setListItems] = useState([]);
    const [listItemsColor, setListItemsColor] = useState("red");

    const nameRef = useRef();
    const emailRef = useRef();
    const passwordRef = useRef();

    const handleButtonClickShowUserForm = () => {
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    useEffect(() => {
        populateUsersData();
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
            <h1 id="tableLabel">Register</h1>
            {/*<ButtonAddAutoUser />*/}
            <ButtonShowFormUser />
            {/*<BuildListFromErrors errorList="" />*/}
            <ul>
                {listItems.map(li => (
                    <li key={li.id} style={{ color: listItemsColor }} >{li.name}</li>
                ))}
            </ul>
            {/*<div id="RegisterStatus">*/}
                {/*{{ registerStatus }}*/}
            {/*</div>*/}
            {/*{contents}*/}
        </div>
    );

    async function populateUsersData() {
        const response = await fetch('/api/users');
        if (response.ok) {
            const data = await response.json();
            setUsers(data);
        }
    }

    //function ButtonAddAutoUser() {
    //    async function handleClick() {
    //        axios
    //            .post("/api/users", {
    //                name: "test",
    //                email: "test@test.com",
    //                password: "test123"
    //            })
    //            .then((response) => {
    //                console.log("User created:", response.data);
    //                populateUsersData();
    //            })
    //            .catch((error) => {
    //                console.error("Error creating user:", error);
    //            });
    //    }

    //    return (
    //        <button onClick={handleClick}
    //            className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
    //        >
    //            Add auto user
    //        </button>
    //    );
    //}

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

    function isObject(value) {
        return typeof value === 'object' && value !== null && !Array.isArray(value);
    }

    function BuildListFromErrors(errorList) {
        //console.log("isObject:" + isObject(errorList));
        //console.log("isArray:" + Array.isArray(errorList));
        //console.log("BuildListFromErrors");
        const list = [];
        Object.keys(errorList).forEach(key => {
            console.error(`${key}: ${errorList[key]}`);
            //setListItems([...listItems, "<li>test</li>"]);
            //list.push(<li>{errorList[key]}</li>);
            list.push({id: key, name: errorList[key]});
            //listItems.push(<li>test</li>);
            //setListItems(listItems.concat("<li>{errorList[key]}</li>"));
        });
        setListItemsColor("red");
        setListItems([...list]);
        //return (
        //    <div className="navigation">
        //        <ul>
        //            {list.map(li => (
        //                <li>{li}</li>
        //            ))}
        //        </ul>
        //    </div>
        //);
    }

    function ButtonShowFormUser() {
        async function handleClick(event) {
            //alert("test");
            event.preventDefault();
            axios
                .post("/register", {
                    //name: nameRef.current.value,
                    email: emailRef.current.value,
                    password: passwordRef.current.value,
                    //password: "test123456_"
                })
                .then((response) => {
                    console.log("User created:", response.data);
                    //populateUsersData();
                    //setShowForm(false);
                    setListItemsColor("green");
                    setListItems([{id: "UserCreatedSuccessfully", name: "User created successfully." }]);
                })
                .catch((error) => {
                    console.error("Error creating user: ", error);
                    console.error("Error response: " + error.response.data.title, error.response.data.errors);
                    //error.response.data.errors.map(err);
                    //const el = buildListFromErrors(error.response.data.errors);
                    //console.log(el);
                    BuildListFromErrors(error.response.data.errors);
                });
        }

        return (
            <div>
                {/*<button onClick={handleButtonClickShowUserForm}*/}
                {/*    className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"*/}
                {/*>Show Form to add user</button>*/}
                {showForm && (
                    <form>
                        {/*<label>*/}
                        {/*    Name:*/}
                        {/*    <input type="text" name="name" defaultValue="test" ref={nameRef}*/}
                        {/*    className="outline"*/}
                        {/*    />*/}
                        {/*</label>*/}
                        {/*<br />*/}
                        <label>
                            Email:
                            <input type="email" name="email" defaultValue="test@test.com" ref={emailRef}
                            className="outline"
                            />
                        </label>
                        <br />
                        <label>
                            Password:
                            <input type="password" name="password" ref={passwordRef}
                                className="outline"
                            />
                        </label>
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