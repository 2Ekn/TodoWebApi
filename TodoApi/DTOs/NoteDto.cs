namespace TodoApi.DTOs;

public class NoteDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public int UserId { get; set; }
}
