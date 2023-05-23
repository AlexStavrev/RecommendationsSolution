import React, { useState, useEffect } from "react";
import { getAllMovies } from "./ApiClient/ApiClient.js";
import Card from "@mui/material/Card";
import CardHeader from "@mui/material/CardHeader";
import CardMedia from "@mui/material/CardMedia";
import CardContent from "@mui/material/CardContent";
import "./Movies.css";

function Movies() {
  const [movieList, setMovieList] = useState([]);
  const [recomendedList, setRecomendedList] = useState([]);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const movies = await getAllMovies();
        setMovieList(movies);
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
        </div>
        <div className="all">
          <h1>All movies</h1>
          <div className="all-videos">
            {movieList.map((movie, index) => {
              return (
                <div key={index} className="movie-holder">
                  <Card className="movie-card" sx={{ width: 300 }}>
                    <CardMedia
                      component="img"
                      height="194"
                      image={movie.url}
                    />
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
        </div>
      </div>
    </>
  );
}

export default Movies;
