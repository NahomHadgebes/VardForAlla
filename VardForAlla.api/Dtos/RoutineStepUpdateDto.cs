using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class RoutineStepUpdateDto
{
    [Required]
    public int Order { get; set; }

    [Required]
    [MaxLength(500)]
    public string SimpleText { get; set; } = string.Empty;

    public string? OriginalText { get; set; }
    public string? IconKey { get; set; }

    public string? ImageUrl { get; set; }
}

