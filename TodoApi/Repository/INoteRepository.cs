using TodoApi.DTOs;

namespace TodoApi.Repository;
public interface INoteRepository
{
    Task<NoteDto> AddAsync(CreateNoteDto createNoteDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<NoteDto>> GetAllAsync();
    Task<NoteDto> GetByIdAsync(int id);
    Task SaveChangesAsync();
    Task<bool> UpdateAsync(int id, UpdateNoteDto updateNoteDto);
}