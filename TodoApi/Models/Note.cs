using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class Note
{
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;



    public int UserId { get; set; }
    public User? User { get; set; }
}
