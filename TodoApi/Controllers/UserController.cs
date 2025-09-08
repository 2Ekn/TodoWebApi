using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }




        var userDto = await _userService.AddAsync(user);

        if (userDto is null)
        {
            return BadRequest("Something went wrong when adding user.");
        }

        return Ok( new UserDto { Username = user.UserName, Id = userDto.Id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var userDto = await _userService.GetByIdAsync(id);

        if (userDto == null)
        {
            return NotFound($"Could not find user with ID: {id}");
        }

        return Ok(userDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(int id, [FromBody]UpdateUserDto userToUpdate)
    {
        var userUpdated = await _userService.UpdateAsync(id, userToUpdate);

        if (!userUpdated)
        {
            return BadRequest("Something went wrong updating user");
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var userDeleted = await _userService.DeleteAsync(id);

        if (!userDeleted)
        {
            return BadRequest("Something went wrong deleting user.");
        }

        return NoContent();
    }
}
