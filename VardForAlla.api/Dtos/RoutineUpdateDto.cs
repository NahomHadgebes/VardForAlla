using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class RoutineUpdateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public string? SimpleDescription { get; set; }

    public string? OriginalDescription { get; set; }
}

