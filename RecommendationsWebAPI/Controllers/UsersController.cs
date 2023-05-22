using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using RecommendationsWebAPI.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecommendationsWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserDataAccess _userDataAccess;

    public UserController(IUserDataAccess userDataAcess)
    {
        _userDataAccess = userDataAcess;
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = DTOConverter<User, UserDTO>.From(await _userDataAccess.GetByIdAsync(id));

        if (user == null)
        {
            return BadRequest($"User with id {id} does not exist!");
        }

        return Ok(user);
    }

    // POST api/<UserController>
    [HttpPost]
    public async Task<IActionResult> Post(UserDTO user)
    {
        return Ok(await _userDataAccess.CreateUserAsync(DTOConverter<UserDTO, User>.From(user))); ;
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

}
