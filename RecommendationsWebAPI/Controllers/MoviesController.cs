using DataAccess.Interfaces;
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

    // GET api/<MovieController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = DTOConverter<Movie?, MovieDTO?>.From(await _movieDataAccess.GetByIdAsync(id));

        if(movie == null) 
        { 
            return BadRequest(movie); 
        } 
        
        return Ok(movie);
        
    }
}
