using System.Security.Claims;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Services;

public class NoteService : INoteService
{
    private readonly ILogger _logger;
    private readonly INoteRepository _repository;

    public NoteService(ILogger logger, INoteRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<NoteDto> AddAsync(CreateNoteDto createNoteDto, int userId)
    {
        if (string.IsNullOrWhiteSpace(createNoteDto.Title) || string.IsNullOrWhiteSpace(createNoteDto.Description))
        {
            _logger.LogWarning("Both Title and description needs to be provided.");
            return null!;
        }

        var noteDto = await _repository.AddAsync(createNoteDto, userId);

        if (noteDto is null)
        {
            _logger.LogWarning("Something went wrong when adding note.");
            return null!;
        }

        return noteDto;
    }

    public async Task<NoteDto> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<IEnumerable<NoteDto>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<NoteDto>> GetAllByUserIdAsync(int userId)
        => await _repository.GetAllByUserIdAsync(userId);

    public async Task<bool> UpdateAsync(int id, UpdateNoteDto updateNoteDto)
    {
        if (string.IsNullOrWhiteSpace(updateNoteDto.Title) || string.IsNullOrWhiteSpace(updateNoteDto.Description))
        {
            _logger.LogWarning("Both Title and description needs to be provided.");
            return false;
        }

        var updateSuccess = await _repository.UpdateAsync(id, updateNoteDto);

        if (!updateSuccess)
        {
            _logger.LogWarning("Something went wrong adding the note.");
            return false;
        }

        return updateSuccess;
    }

    public async Task<bool> DeleteAsync(int id)
        => await _repository.DeleteAsync(id);
}
