using System.ComponentModel.DataAnnotations;

namespace FinShark.WebApi.DTOs;

public class UserLogin
{
    [Required]
    public string? Login { get; set; }
    [Required]
    public string? Password { get; set; }
}
