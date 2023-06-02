import axios from "axios";

const dNetUrl = "http://localhost:5034/api/";

export async function logIn(name) {
  const requestUri = `${dNetUrl}Users/logIn/${name}`;

  try {
    const res = await axios.get(requestUri);
    let response = checkStatus(res);
    return response.data;
  } catch {
    return null;
  }
}

export async function createUser(name) {
  const user = {
    name: name,
  };

  const requestUri = `${dNetUrl}Users`;
  try {
    const res = await axios.post(requestUri, user);
    let response = checkStatus(res);
    return response.data;
  } catch {
    return null;
  }
}

export async function getAllMovies(userId) {
  const requestUri = `${dNetUrl}Movies/user/${userId}`;
  try {
    const res = await axios.get(requestUri);
    let response = checkStatus(res);
    return response.data;
  } catch {
    return null;
  }
}

export async function getRecommendedMovies(userId) {
  const requestUri = `${dNetUrl}Users/${userId}/recommendations`;
  try {
    const res = await axios.get(requestUri);
    let response = checkStatus(res);
    return response.data;
  } catch {
    return null;
  }
}

export async function putSeen(userId, movieId) {
  const requestUri = `${dNetUrl}Users/${userId}/movies/${movieId}/see`;
  try {
    const res = await axios.put(requestUri);
    let response = checkStatus(res);
    return response.data;
  } catch {
    return null;
  }
}

export async function putLike(userId, movieId) {
  const requestUri = `${dNetUrl}Users/${userId}/movies/${movieId}/like`;
  try {
    const res = await axios.put(requestUri);
    let response = checkStatus(res);
    return response.data;
  } catch {
    return null;
  }
}

function checkStatus(response) {
  if (response.status >= 200 && response.status < 300) {
    return response;
  } else {
    return false;
  }
}
