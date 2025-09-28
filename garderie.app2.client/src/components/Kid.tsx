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

interface Kid {
    id: number;
    daycareId: number;
    name: string;
}

interface Answer {
    id: number;
    daycareId: number;
    kidId: number;
    name: string;
    description: string;
}

function App() {
    const { daycareId, kidId } = useParams(); // Access the `id` parameter from the route
    const [user, setUser] = useState<User>();
    const [daycare, setDaycare] = useState<Daycare>();
    const [kids, setKids] = useState<Kid[]>();
    const [answers, setAnswers] = useState<Answer[]>();
    const [showForm, setShowForm] = useState(false);
    const [questionName, setQuestionName] = useState("");
    const [questionDescription, setQuestionDescription] = useState("");
    const [answerId, setAnswerId] = useState<number | null>(null); // To track if we are editing an existing answer

    const nameRef = useRef<HTMLInputElement>(null);
    const nameQuestionRef = useRef<HTMLInputElement>(null);
    const descriptionRef = useRef<HTMLInputElement>(null);
    const descriptionQuestionRef = useRef<HTMLInputElement>(null);

    const handleButtonClickShowForm = () => {
        setAnswerId(null); // Reset answerId when showing the form for a new answer
        setQuestionName(""); // Clear the question name
        setQuestionDescription(""); // Clear the question description
        setShowForm(!showForm); // Show the form when the button is clicked
    };

    const handleButtonClickEditForm = (id: number, name: string, description: string) => {
        setAnswerId(id); // Set the answerId to the id of the answer being edited
        // Toggle the form visibility
        setShowForm(true);
        // Optionally, you can set the question name to the current name of the question being edited
        const answerToEdit = answers?.find(answer => answer.id === id);
        if (answerToEdit) {
            setQuestionName(answerToEdit.name);
            setQuestionDescription(answerToEdit.description);
            //descriptionQuestionRef.current!.value = answerToEdit.description;
        }
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
                    <th>Description</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                {answers.map(answer =>
                    <tr key={answer.id}>
                        <td>{answer.id}</td>
                        <td>{answer.daycareId}</td>
                        <td>{answer.name}</td>
                        <td>{answer.description}</td>
                        <td><Link to={"/answer/" + answer.id}> {answer.name} </Link></td>
                        <td> <EditAnswer id={answer.id} name={answer.name} description={answer.description} /> </td>
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
            {/*<br /><ButtonAddAutoAnswer />*/}
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

    function RemoveAnswer({ id }: {id: number}) {
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
        async function handleClick(event: React.MouseEvent) {
            event.preventDefault();
            // If answerId is set, we are editing an existing answer
            if (answerId) {
                axios
                    .put("/api/answers/" + answerId, {
                        name: nameQuestionRef.current?.value,
                        description: descriptionQuestionRef.current?.value,
                        daycareId: daycareId,
                        kidId: kidId
                    })
                    .then((response) => {
                        console.log("Answer updated:", response.data);
                        populateAnswersData();
                    })
                    .catch((error) => {
                        console.error("Error updating Answer:", error);
                    });
            } else {
                axios
                    .post("/api/answers", {
                        name: nameQuestionRef.current?.value,
                        description: descriptionQuestionRef.current?.value,
                        daycareId: daycareId,
                        kidId: kidId
                    })
                    .then((response) => {
                        console.log("Answer created:", response.data);
                        populateAnswersData();
                        setShowForm(false);
                    })
                    .catch((error) => {
                        console.error("Error creating Answer:", error);
                    });
            }
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
                                <input type="text" name="name" defaultValue={questionName} ref={nameQuestionRef}
                                    className="outline"
                                />
                            </label>
                            <br />
                            <label>
                                Description:
                                <input type="text" name="description" defaultValue={questionDescription} ref={descriptionQuestionRef}
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

    function EditAnswer({ id, name, description }: { id: number, name: string, description: string }) {
        async function handleClick() {
            handleButtonClickEditForm(id, name, description);
            // Set nameRef to name
            //nameRef.current.value = name;
            setQuestionName(name);
            setQuestionDescription(description);
            setAnswerId(id);
            //nameRef.current?.focus();

            //axios
            //    .put("/api/kids/" + id, {
            //        //id: 2
            //    })
            //    .then((response) => {
            //        console.log("Daycare deleted:", response.data);
            //        populateKidsData();
            //    })
            //    .catch((error) => {
            //        console.error("Error deleting daycare:", error);
            //    });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                <i className="pi pi-pencil yellow" style={{ color: 'yellow' }}></i>
            </button>
        );
    }

}// end of App

export default App;