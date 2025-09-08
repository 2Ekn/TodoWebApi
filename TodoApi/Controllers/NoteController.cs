using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;
[Route("api/notes")]
[ApiController]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNote([FromBody] CreateNoteDto createNoteDto)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Unauthorized("No user was found.");
        }

        if (!int.TryParse(userId, out int userIdInt))
        {
            return BadRequest("Invalid user ID in token.");
        }

        var note = await _noteService.AddAsync(createNoteDto, userIdInt);

        if (note is null)
        {
            return BadRequest("Failed to create note.");
        }

        return Ok(note);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUserNotes()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Unauthorized("No user was found.");
        }

        if (!int.TryParse(userId, out int userIdInt))
        {
            return BadRequest("Invalid user ID in token.");
        }

        var notes = await _noteService.GetAllByUserIdAsync(userIdInt);

        return Ok(notes);
    }

}
