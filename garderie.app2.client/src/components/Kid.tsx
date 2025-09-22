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
    const { daycareId, kidId } = useParams(); // Access the `id` parameter from the route
    const [user, setUser] = useState<User>();
    const [daycare, setDaycare] = useState<Daycare>();
    const [kids, setKids] = useState<Kid[]>();
    const [answers, setAnswers] = useState<Answer[]>();
    const [showForm, setShowForm] = useState(false);

    const nameRef = useRef("");
    const nameQuestionRef = useRef("");

    const handleButtonClickShowForm = () => {
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    useEffect(() => {
        populateAnswersData();
    }, []);

    const contentsAnswers = answers === undefined
        //? <i className="pi pi-spin pi-spinner"></i>
        ? <div>No Answers</div>
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
                {answers.map(answer =>
                    <tr key={answer.id}>
                        <td>{answer.id}</td>
                        <td>{answer.daycareId}</td>
                        <td><Link to={"/answer/" + answer.id}> {answer.name} </Link></td>
                        <td> <RemoveAnswer id={answer.id} /> </td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            {/*Hello {id}*/}
            {/*Hello {user ? user.name : <i className='pi pi-spin pi-spinner'></i>}*/}
            Daycare {daycare ? daycare.name : <i className='pi pi-spin pi-spinner'></i>}
            <h1>Answers</h1>
            <br /><ButtonAddAutoAnswer />
            <br /><ButtonShowFormAnswer />
            {contentsAnswers}
        </div>
    );

    async function populateAnswersData() {
        axios
            .get("/api/answers/", {
                params: {
                    daycareId: daycareId,
                    kidId: kidId
                }
            })
            .then((response) => {
                console.log("Request:", response.request);
                console.log("Answer data:", response.data);
                setAnswers(response.data);
            })
            .catch((error) => {
                console.error("Error getting answer data:", error);
            });
    }

    function ButtonAddAutoAnswer() {
        async function handleClick() {
            axios
                .post("/api/answers", {
                    name: "test",
                    description: "test description",
                    daycareId: daycareId,
                    kidId: kidId
                })
                .then((response) => {
                    console.log("Answer created:", response.data);
                    populateAnswersData();
                })
                .catch((error) => {
                    console.error("Error creating answer:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                Add auto Answer
            </button>
        );
    }

    function RemoveAnswer({ id }) {
        async function handleClick() {
            axios
                .delete("/api/answers/" + id, {
                    //id: 2
                })
                .then((response) => {
                    console.log("Answer deleted:", response.data);
                    populateAnswersData();
                })
                .catch((error) => {
                    console.error("Error deleting answer:", error);
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

    function ButtonShowFormAnswer() {
        async function handleClick(event) {
            event.preventDefault();
            axios
                .post("/api/answers", {
                    name: nameQuestionRef.current.value,
                    daycareId: daycareId
                })
                .then((response) => {
                    console.log("Question created:", response.data);
                    populateAnswersData();
                    setShowForm(false);
                })
                .catch((error) => {
                    console.error("Error creating question:", error);
                });
        }

        return (
            <div>
                <button onClick={handleButtonClickShowForm}
                    className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
                >Show Form to add Answer</button>
                {showForm && (
                    <form>
                        <label>
                            Name:
                            <input type="text" name="name" defaultValue="test" ref={nameQuestionRef}
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

}// end of App



export default App;