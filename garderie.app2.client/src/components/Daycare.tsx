import { useEffect, useState, useRef } from 'react';
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

interface Kid {
    id: number;
    daycareId: number;
    name: string;
}

function App() {
    const { daycareId } = useParams(); // Access the `id` parameter from the route
    const [user, setUser] = useState<User>();
    const [daycare, setDaycare] = useState<Daycare>();
    const [kids, setKids] = useState<Kid[]>();
    const [showForm, setShowForm] = useState(false);

    const nameRef = useRef();

    const handleButtonClickShowForm = () => {
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    useEffect(() => {
        populateDaycareData();
        populateKidsData();
    }, []);

    const contents = kids === undefined
        //? <i className="pi pi-spin pi-spinner"></i>
        ? <div>No Kids</div>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>DaycareId</th>
                    <th>Name</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                {kids.map(kid =>
                    <tr key={kid.id}>
                        <td>{kid.id}</td>
                        <td>{kid.daycareId}</td>
                        <td><Link to={"/daycare/" + kid.id}> {kid.name} </Link></td>
                        <td> <RemoveKid id={kid.id} /> </td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            {/*Hello {id}*/}
            {/*Hello {user ? user.name : <i className='pi pi-spin pi-spinner'></i>}*/}
            Daycare {daycare ? daycare.name : <i className='pi pi-spin pi-spinner'></i>}
            <br /><ButtonAddAutoKid />
            <br /><ButtonShowFormKid />
            {contents}
        </div>
    );

    async function populateDaycareData() {
        axios
            .get("/api/daycares/" + daycareId, {
                //id: 2
            })
            .then((response) => {
                console.log("Request:", response.request);
                console.log("User data:", response.data);
                setDaycare(response.data);
            })
            .catch((error) => {
                console.error("Error getting user data:", error);
            });
    }

    async function populateKidsData() {
        axios
            .get("/api/kids/", {
                daycareId: daycareId
            })
            .then((response) => {
                console.log("Request:", response.request);
                console.log("User data:", response.data);
                setKids(response.data);
            })
            .catch((error) => {
                console.error("Error getting user data:", error);
            });
    }

    function ButtonAddAutoKid() {
        async function handleClick() {
            axios
                .post("/api/kids", {
                    name: "test",
                    daycareId: daycareId
                })
                .then((response) => {
                    console.log("Daycare created:", response.data);
                    populateKidsData();
                })
                .catch((error) => {
                    console.error("Error creating daycare:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                Add auto Kid
            </button>
        );
    }

    function RemoveKid({ id }) {
        async function handleClick() {
            axios
                .delete("/api/kids/" + id, {
                    //id: 2
                })
                .then((response) => {
                    console.log("Daycare deleted:", response.data);
                    populateKidsData();
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

    function ButtonShowFormKid() {
        async function handleClick(event) {
            event.preventDefault();
            axios
                .post("/api/kids", {
                    name: nameRef.current.value,
                    daycareId: daycareId
                })
                .then((response) => {
                    console.log("User created:", response.data);
                    populateKidsData();
                    setShowForm(false);
                })
                .catch((error) => {
                    console.error("Error creating user:", error);
                });
        }

        return (
            <div>
                <button onClick={handleButtonClickShowForm}
                    className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
                >Show Form to add Kid</button>
                {showForm && (
                    <form>
                        <label>
                            Name:
                            <input type="text" name="name" defaultValue="test" ref={nameRef}
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