using TodoApi.DTOs;

namespace TodoApi.Repository;
public interface INoteRepository
{
    Task<NoteDto> AddAsync(CreateNoteDto createNoteDto, int userId);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<NoteDto>> GetAllAsync();
    Task<NoteDto> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdateNoteDto updateNoteDto);
    Task<IEnumerable<NoteDto>> GetAllByUserIdAsync(int userId);
}