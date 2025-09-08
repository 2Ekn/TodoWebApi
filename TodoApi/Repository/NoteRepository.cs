using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Repository;

public class NoteRepository : INoteRepository
{
    private readonly ILogger _logger;
    private readonly TodoDbContext _dbContext;

    public NoteRepository(TodoDbContext dbContext, ILogger logger)
    {

        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<NoteDto> AddAsync(CreateNoteDto createNoteDto, int userId)
    {
        try
        {
            var note = new Note
            {
                Title = createNoteDto.Title,
                Description = createNoteDto.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            await _dbContext.AddAsync(note);
            await _dbContext.SaveChangesAsync();

            return new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                CreatedAt = note.CreatedAt,
                UserId = note.UserId
            };
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error while adding note.");
            return null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while adding note.");
            return null!;
        }
    }

    public async Task<NoteDto> GetByIdAsync(int id)
    {
        try
        {
            var note = await _dbContext.Note
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                _logger.LogWarning("No note was found with that Id: {Id}", id);
                return null!;
            }

            return new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                CreatedAt = note.CreatedAt
            };

        }
        catch (DbException e)
        {
            _logger.LogError(e, "Database error while retrieving note.");
            return null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while retrieving note.");
            return null!;
        }
    }

    public async Task<IEnumerable<NoteDto>> GetAllByUserIdAsync(int userId)
    {
        try
        {
            var notes = await _dbContext.Note
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            return notes;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error while retrieving note.");
            return null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while retrieving note.");
            return null!;
        }
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync()
    {
        try
        {
            var notes = await _dbContext.Note
                .AsNoTracking()
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();

            return notes;
        }
        catch (DbException e)
        {
            _logger.LogError(e, "Database error while retrieving notes.");
            return null!;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while retrieving notes.");
            return null!;
        }
    }
    public async Task<bool> UpdateAsync(int id, UpdateNoteDto updateNoteDto)
    {
        try
        {
            var noteFromDb = await _dbContext.Note
                .FirstOrDefaultAsync(n => n.Id == id);

            if (noteFromDb == null)
            {
                _logger.LogWarning("No note with that ID exists.");
                return false;
            }

            noteFromDb.Title = updateNoteDto.Title;
            noteFromDb.Description = updateNoteDto.Description;

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error while updating note.");
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while adding note.");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var rowsAff = await _dbContext.Note
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync();

            if (rowsAff < 1)
            {
                _logger.LogWarning("Something went wrong deleting note. Check if ID is valid.");
                return false;
            }

            await _dbContext.SaveChangesAsync();

            return true;

        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Database error while deleting note.");
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error while deleting note.");
            return false;
        }
    }
}
