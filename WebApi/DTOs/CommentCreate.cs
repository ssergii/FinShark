using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs;

public class CommentCreate
{
    [Required]
    [MinLength(5, ErrorMessage = "{0} must be at least {1} characters long.")]
    [MaxLength(25, ErrorMessage = "{0} must not exceed {1} characters.")]
    public string? Title { get; set; }

    [Required]
    [Length(25, 250, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    public string? Content { get; set; }
}
