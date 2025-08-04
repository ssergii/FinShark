using System;
using System.ComponentModel.DataAnnotations;

namespace FinShark.WebApi.DTOs;

public class UserCreate
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
