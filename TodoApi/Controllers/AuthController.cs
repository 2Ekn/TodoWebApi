using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody]LoginUserDto userDto)
    {
        var token = await _service.LoginUserAsync(userDto);

        return Ok(new { token });
    }
}
