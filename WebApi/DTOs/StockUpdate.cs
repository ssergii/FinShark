using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs;

public class StockUpdate
{
    [Required]
    [Length(2, 10, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [Length(2, 25, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000, ErrorMessage = "{0} must be greater than {1} and less than {2}.")]
    public decimal Purchase { get; set; }

    [Required]
    [Range(0.01, 100, ErrorMessage = "{0} must be greater than {1} and less than {2}.")]
    public decimal LastDiv { get; set; }

    [Required]
    [Length(2, 10, ErrorMessage = "{0} must be between {1} and {2} characters long.")]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000, ErrorMessage = "{0} must be greater than {1} and less than {2}.")]
    public long MarketCap { get; set; }
}
