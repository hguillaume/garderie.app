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

interface Question {
    id: number;
    daycareId: number;
    name: string;
}

function App() {
    const { daycareId } = useParams(); // Access the `id` parameter from the route
    const [user, setUser] = useState<User>();
    const [daycare, setDaycare] = useState<Daycare>();
    const [kids, setKids] = useState<Kid[]>();
    const [questions, setQuestions] = useState<Question[]>();
    const [showForm, setShowForm] = useState(false);
    const [kidName, setKidName] = useState("");
    const [questionName, setQuestionName] = useState("");

    const nameRef = useRef<HTMLInputElement>(null);
    const kidIdRef = useRef(0);
    const nameQuestionRef = useRef<HTMLInputElement>(null);
    const questionIdRef = useRef(0);

    const handleButtonClickShowForm = () => {
        kidIdRef.current = 0;
        questionIdRef.current = 0;
        setKidName("");
        setShowForm(!showForm); // Show or hide the form when the button is clicked
        nameRef.current?.focus();
    };

    const handleButtonClickEditForm = (kidId: number) => {
        kidIdRef.current = kidId;
        setShowForm(true); // Show the form when the button is clicked
        nameRef.current?.focus();
    };

    const handleButtonClickEditQuestionForm = (questionId: number) => {
        questionIdRef.current = questionId;
        setShowForm(true); // Show the form when the button is clicked
        nameQuestionRef.current?.focus();
    };

    useEffect(() => {
        populateDaycareData();
        populateKidsData();
        populateQuestionsData();
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
                        {/*<td><Link to={"/daycare/" + kid.id}> {kid.name} </Link></td>*/}
                        <td><Link to={"/daycare/" + daycareId + '/kid/' + kid.id}> {kid.name} </Link></td>
                        <td> <EditKid id={kid.id} name={ kid.name } /> </td>
                        <td> <RemoveKid id={kid.id} /> </td>
                    </tr>
                )}
            </tbody>
        </table>;

    const contentsQuestions = questions === undefined
        //? <i className="pi pi-spin pi-spinner"></i>
        ? <div>No Questions</div>
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
                {questions.map(question =>
                    <tr key={question.id}>
                        <td>{question.id}</td>
                        <td>{question.daycareId}</td>
                        <td><Link to={"/daycare/" + question.id}> {question.name} </Link></td>
                        <td> <EditQuestion id={question.id} name={question.name} /> </td>
                        <td> <RemoveQuestion id={question.id} /> </td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            {/*Hello {id}*/}
            {/*Hello {user ? user.name : <i className='pi pi-spin pi-spinner'></i>}*/}
            Daycare {daycare ? daycare.name : <i className='pi pi-spin pi-spinner'></i>}
            <h1>Kids</h1>
            {/*<br /><ButtonAddAutoKid />*/}
            <br /><ButtonShowFormKid />
            {/*<br /><ButtonEditFormKid />*/}
            {contents}
            <h1>Questions</h1>
            {/*<br /><ButtonAddAutoQuestion />*/}
            <br /><ButtonShowFormQuestion />
            {contentsQuestions}
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
                params: {
                    daycareId: daycareId
                }
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

    async function populateQuestionsData() {
        axios
            .get("/api/questions/", {
                params: {
                    daycareId: daycareId
                }
            })
            .then((response) => {
                console.log("Request:", response.request);
                console.log("Question data:", response.data);
                setQuestions(response.data);
            })
            .catch((error) => {
                console.error("Error getting question data:", error);
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

    function RemoveKid({ id }: {id: number}) {
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
        async function handleClick(event: React.MouseEvent) {
            event.preventDefault();

            if(kidIdRef.current != 0) {
                // Edit Kid
                axios
                    .put("/api/kids/" + kidIdRef.current, {
                        name: nameRef.current?.value,
                        daycareId: daycareId
                    })
                    .then((response) => {
                        console.log("Kid updated:", response.data);
                        populateKidsData();
                        setShowForm(false);
                    })
                    .catch((error) => {
                        console.error("Error updating Kid:", error);
                    });
                return;
            }

            // Add Kid
            axios
                .post("/api/kids", {
                    name: nameRef.current?.value,
                    daycareId: daycareId
                })
                .then((response) => {
                    console.log("Kid created:", response.data);
                    populateKidsData();
                    setShowForm(false);
                })
                .catch((error) => {
                    console.error("Error creating Kid:", error);
                });
        }

        return (
            <div>
                <button onClick={handleButtonClickShowForm}
                    className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
                >Show Form to add or edit Kid</button>
                {showForm && (
                    <form>
                        <label>
                            Name:
                            <input type="text" name="name" defaultValue={kidName} ref={nameRef} tabIndex={0}
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

    function EditKid({ id, name }: {id: number, name: string}) {
        async function handleClick() {
            handleButtonClickEditForm(id);
            // Set nameRef to name
            //nameRef.current.value = name;
            setKidName(name);
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



    // Questions
     function ButtonAddAutoQuestion() {
        async function handleClick() {
            axios
                .post("/api/questions", {
                    name: "test",
                    daycareId: daycareId
                })
                .then((response) => {
                    console.log("Question created:", response.data);
                    populateQuestionsData();
                })
                .catch((error) => {
                    console.error("Error creating question:", error);
                });
        }

        return (
            <button onClick={handleClick}
                className="shadow-lg outline outline-black/9 dark:bg-slate-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10"
            >
                Add auto Question
            </button>
        );
    }

    function RemoveQuestion({ id }: {id: number}) {
        async function handleClick() {
            axios
                .delete("/api/questions/" + id, {
                    //id: 2
                })
                .then((response) => {
                    console.log("Question deleted:", response.data);
                    populateQuestionsData();
                })
                .catch((error) => {
                    console.error("Error deleting question:", error);
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

    function EditQuestion({ id, name }: { id: number, name: string }) {
        async function handleClick() {
            handleButtonClickEditQuestionForm(id);
            // Set nameRef to name
            //nameRef.current.value = name;
            setQuestionName(name);
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

    function ButtonShowFormQuestion() {
        async function handleClick(event : React.MouseEvent) {
            event.preventDefault();

            if (questionIdRef.current != 0) {
                // Edit Question
                axios
                    .put("/api/questions/" + questionIdRef.current, {
                        name: nameQuestionRef.current?.value,
                        daycareId: daycareId
                    })
                    .then((response) => {
                        console.log("Question updated:", response.data);
                        populateQuestionsData();
                        setShowForm(false);
                    })
                    .catch((error) => {
                        console.error("Error updating Question:", error);
                    });
                return;
            }

            axios
                .post("/api/questions", {
                    name: nameQuestionRef.current?.value,
                    daycareId: daycareId
                })
                .then((response) => {
                    console.log("Question created:", response.data);
                    populateQuestionsData();
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
                >Show Form to add or edit Question</button>
                {showForm && (
                    <form>
                        <label>
                            Name:
                            <input type="text" name="name" defaultValue={questionName} ref={nameQuestionRef}
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