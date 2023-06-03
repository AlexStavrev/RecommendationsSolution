import * as React from "react";
import { logIn, createUser } from "./ApiClient/ApiClient.js";
import Snackbar from "@mui/material/Snackbar";
import Alert from "@mui/material/Alert";
import Movies from"./Movies.jsx";
import "./App.css";

function App() {
  const [name, setName] = React.useState("");
  const [open, setOpen] = React.useState(false);
  const [userId, setUserId] = React.useState(0);
  const [authorized, setAuthorized] = React.useState();

  const handleClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }
    setOpen(false);
  };

  const authorize = async () => {
    var response = await logIn(name);
    console.log(response.id)
    if (response == false) {
      setOpen(true);
    } else {
      setUserId(response.id);
      setAuthorized(true);
    }
    console.log(response);
  };

  const register = async () => {
    var response = await createUser(name);
    console.log(response);
    if (response == null) {
      setOpen(true);
    }
    setUserId(response);
    setAuthorized(true);
  };

  const handleChange = (event) => {
    setName(event.target.value);
  };

  return (
    <>
      <div className="App">
        {!authorized ? (
          <>
            <h2>Movie recomendation system</h2>
            <div className="logo">
              {/* <img src="public/neo4j.svg" className="logo" alt="Movie logo" /> */}
            </div>
            <div className="card">
              <Snackbar
                open={open}
                autoHideDuration={6000}
                onClose={handleClose}
              >
                <Alert
                  onClose={handleClose}
                  severity="error"
                  sx={{ width: "100%" }}
                >
                  Authorization failed!
                </Alert>
              </Snackbar>
              <input
                placeholder="Your name"
                type="text"
                value={name}
                onChange={handleChange}
              />
              <div className="buttons">
                <button className="button" onClick={authorize}>
                  Continue
                </button>
                <button className="button" onClick={register}>
                  Register
                </button>
              </div>
            </div>
          </>
        ) : (
          <>
            <Movies userId={userId}></Movies>
          </>
        )}
      </div>
    </>
  );
}

export default App;
