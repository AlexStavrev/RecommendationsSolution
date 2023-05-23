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
  const [clickedCards, setClickedCards] = useState([]);

  const setIsSeenOverlay = (index) => {
    if (clickedCards.includes(index)) {
      setClickedCards(clickedCards.filter((cardIndex) => cardIndex !== index));
    } else {
      setClickedCards([...clickedCards, index]);
    }
  };

  const setSeenVideo = (userId, movieId) => {
    console.log(userId);
    console.log(movieId);
    putSeen(userId, movieId);
  };

  const setLikedVideo = (userId, movieId) => {
    putLike(userId, movieId);
  };

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const recomended = await getRecommendedMovies(userId);
        const movies = await getAllMovies();
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
        <div className="recomended">
          <h1>Recomened</h1>
          {recomendedList.map((movie, index) => {
            return (
              <div key={index} className="movie-holder">
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
                    {clickedCards.includes(index) && (
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
                          setIsSeenOverlay(index);
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
                              className="likebutton"
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
