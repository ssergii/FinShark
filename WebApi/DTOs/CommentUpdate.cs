using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs;

public class CommentUpdate
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters long.")]
    [MaxLength(25, ErrorMessage = "Title must not exceed 50 characters.")]
    public string? Title { get; set; }

    [Required]
    [MinLength(25, ErrorMessage = "Title must be at least 25 characters long.")]
    [MaxLength(250, ErrorMessage = "Title must not exceed 250 characters.")]
    public string? Content { get; set; }
}
