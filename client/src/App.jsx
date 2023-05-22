import { useState } from 'react'
import './App.css'

function App() {
  const [name, setName] = useState('');
  const logIn = (name) => {
    //Call api
    console.log(name)
  }

  const register = (name) => {
        //Call api
        console.log(name)
  }

  const handleChange = (event) => {
    setName(event.target.value);
  };

  return (
    <>
      <h2>Movie recomendation system</h2>
      <div className='logo'>
          {/* <img src="public/logo.png" className="logo" alt="Movie logo" /> */}
      </div>
      <div className="card">
      <input placeholder="Your name" type="text" value={name} onChange={handleChange} />
      <div className='buttons'>
      <button className="button" onClick={logIn(name)}>
          Log In
        </button>
        <button className="button" onClick={register(name)}>
          Register
        </button>
      </div>
        
      </div>
    </>
  )
}

export default App
