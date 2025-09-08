using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    public ICollection<Note>? Tasks { get; set; }
}
