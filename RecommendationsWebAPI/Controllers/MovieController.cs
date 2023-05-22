using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using RecommendationsWebAPI.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieDataAccess _movieDataAccess;

    public MovieController(IMovieDataAccess movieDataAcess)
    {
        _movieDataAccess = movieDataAcess;
    }

    // GET: api/<MovieController>
    [HttpGet]
    public async Task<IEnumerable<MovieDTO>> GetAllMovies()
    {
        return DTOConverter<Movie, MovieDTO>.FromList(await _movieDataAccess.GetAllAsync());
    }

    // GET api/<MovieController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<MovieController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<MovieController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<MovieController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
