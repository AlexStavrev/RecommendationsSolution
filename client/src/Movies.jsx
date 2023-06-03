import React, { useState, useEffect } from "react";
import {
  getAllMovies,
  getRecommendedMovies,
  putLike,
  putSeen,
} from "./ApiClient/ApiClient.js";
import CardActionArea from "@mui/material/CardActionArea";
import Card from "@mui/material/Card";
import IconButton from "@mui/material/IconButton";
import ThumbUpIcon from "@mui/icons-material/ThumbUp";
import CardMedia from "@mui/material/CardMedia";
import CardContent from "@mui/material/CardContent";
import "./Movies.css";

function Movies(props) {
  const { userId } = props;
  const [movieList, setMovieList] = useState([]);
  const [recomendedList, setRecomendedList] = useState([]);

  const setSeenVideo = async (userId, movieId) => {
    updateMovieSeen(movieId);
    var response = await putSeen(userId, movieId);
    if(response == null){
      console.log("Response is null!")
    }else{
      const recomended = await getRecommendedMovies(userId);
      setRecomendedList(recomended);
    }

    // setTimeout(async () => {
    //   const recomended = await getRecommendedMovies(userId);
    //   setRecomendedList(recomended);
    // }, 1000); 
  };

const updateMovieLike = (movieId) => {

  // Create a copy of the movie list
  const updatedMovieList = [...movieList];

  // Find the index of the movie in the array
  const movieIndex = updatedMovieList.findIndex(movie => movie.id === movieId);

  if (movieIndex !== -1) {
    // Update the 'liked' property of the movie
    updatedMovieList[movieIndex] = {
      ...updatedMovieList[movieIndex],
      liked: true
    };

    // Set the updated movie list
    setMovieList(updatedMovieList);
}
}

const updateMovieSeen = (movieId) => {

  // Create a copy of the movie list
  const updatedMovieList = [...movieList];

  // Find the index of the movie in the array
  const movieIndex = updatedMovieList.findIndex(movie => movie.id === movieId);

  if (movieIndex !== -1) {
    // Update the 'liked' property of the movie
    updatedMovieList[movieIndex] = {
      ...updatedMovieList[movieIndex],
      seen: true
    };

    // Set the updated movie list
    setMovieList(updatedMovieList);
}
}

  const setLikedVideo = async (userId, movieId) => {
    updateMovieLike(movieId)
    var response = await putLike(userId, movieId);
    if(response == null){
      console.log("Response is null!")
    }else{
      const recomended = await getRecommendedMovies(userId);
      console.log(recomended)
      setRecomendedList(recomended);
    }
  };

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const recomended = await getRecommendedMovies(userId);
        const movies = await getAllMovies(userId);
        setMovieList(movies);
        setRecomendedList(recomended);
        console.log(movies);
      } catch (error) {
        console.error(error);
      }
    };

    fetchMovies();
  }, []);

  return (
    <>
      <div className="content">
      <h1>Recomended</h1>
        <div className="recomended">
          {recomendedList.map((movie, index) => {
            return (
              <div key={index} className="movie-margin">
              <div className="movie-holder">
                <Card className="movie-card" sx={{ width: 300 }}>
                  <CardMedia component="img" height="194" image={movie.url} />
                  <CardContent>
                    <div className="title">
                      <h3 className="moviename">{movie.name}</h3>
                    </div>
                    <div className="like"></div>
                  </CardContent>
                </Card>
              </div>
              </div>
            );
          })}
        </div>
        <div className="all">
          <h1>All movies</h1>
          <div className="all-videos">
            {movieList.map((movie, index) => {
              return (
                <div key={index} className="movie-margin">
                  <div className="movie-holder">
                    {movie.seen && (
                      <div className="overlay">
                        <img
                          className="image-overlay"
                          src="public/eye.svg"
                          alt="Overlay"
                        />
                      </div>
                    )}
                    <Card className="movie-card" sx={{ width: 300 }}>
                      <CardActionArea
                        onClick={() => {
                          setSeenVideo(userId, movie.id);
                        }}
                      >
                        <CardMedia
                          component="img"
                          height="194"
                          image={movie.url}
                        />
                        <CardContent className="cardContent">
                          <div className="title">
                            <h3 className="moviename">{movie.name}</h3>
                          </div>
                          <div className="like">
                            {" "}
                            <IconButton
                              onClick={(event) => {
                                event.stopPropagation(); // Stop event propagation
                                setLikedVideo(userId, movie.id);
                              }}
                              className={`${movie.liked ? 'liked' : 'likebutton '}`}
                              aria-label="like"
                              size="large"
                            >
                              <ThumbUpIcon />
                            </IconButton>
                          </div>
                        </CardContent>
                      </CardActionArea>
                    </Card>
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </>
  );
}

export default Movies;
