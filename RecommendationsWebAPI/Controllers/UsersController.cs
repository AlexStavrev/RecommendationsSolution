using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using RecommendationsWebAPI.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserDataAccess _userDataAccess;

    public UsersController(IUserDataAccess userDataAcess)
    {
        _userDataAccess = userDataAcess;
    }

    // GET api/<UserController>/5
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var user = DTOConverter<User, UserDTO>.From(await _userDataAccess.GetByIdAsync(userId));

        if (user == null)
        {
            return BadRequest($"User with id {userId} does not exist!");
        }

        return Ok(user);
    }

    [HttpGet("logIn/{userName}")]
    public async Task<IActionResult> LogIn(string userName)
    {
        return Ok(await _userDataAccess.LoginUserAsync(userName));
    }

    // POST api/<UserController>
    [HttpPost]
    public async Task<IActionResult> Post(UserDTO user)
    {
        int? createdId = await _userDataAccess.CreateUserAsync(DTOConverter<UserDTO, User>.From(user));
        if(!createdId.HasValue)
        {
            return BadRequest($"Failed to create user {user}");
        }
        return CreatedAtAction(nameof(GetUserById), new { userId = createdId }, createdId);
    }

    // PUT api/<UserController>/userId/5/movieId/5
    [HttpPut("users/{userId}/movies/{movieId}/see")]
    public async Task<IActionResult> PutSeeMovie(int userId, int movieId)
    {
        await _userDataAccess.SeeMovieAsync(userId, movieId);
        return Ok();
    }

    // PUT api/<UserController>/userId/5/movieId/5
    [HttpPut("users/{userId}/movies/{movieId}/like")]
    public async Task<IActionResult> PutLikeMovie(int userId, int movieId)
    {
        await _userDataAccess.LikeMovieAsync(userId, movieId);
        return Ok();
    }
}
