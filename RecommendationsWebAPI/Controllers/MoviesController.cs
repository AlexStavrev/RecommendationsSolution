﻿using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using RecommendationsWebAPI.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieDataAccess _movieDataAccess;

    public MoviesController(IMovieDataAccess movieDataAcess)
    {
        _movieDataAccess = movieDataAcess;
    }

    // GET: api/<MovieController>
    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        return Ok(DTOConverter<Movie, MovieDTO>.FromList(await _movieDataAccess.GetAllAsync()));
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllMoviesByUserId(int userId)
    {
        return Ok(DTOConverter<Movie, MovieDTO>.FromList(await _movieDataAccess.GetAllByUserIdAsync(userId)));
    }

    // GET api/<MovieController>/5
    [HttpGet("{movieId}")]
    public async Task<IActionResult> GetMovieById(int movieId)
    {
        var movie = DTOConverter<Movie?, MovieDTO?>.From(await _movieDataAccess.GetByIdAsync(movieId));

        if(movie == null) 
        { 
            return BadRequest(movie); 
        } 
        
        return Ok(movie);
        
    }
}
