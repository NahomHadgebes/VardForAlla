using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class RoutineCreateStepDto
{
    [Required]
    public int Order { get; set; }

    [Required]
    [MaxLength(500)]
    public string SimpleText { get; set; } = string.Empty;

    public string? OriginalText { get; set; }
    public string? IconKey { get; set; }
}

public class RoutineCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public string? SimpleDescription { get; set; }
    public string? OriginalDescription { get; set; }

    [MinLength(1, ErrorMessage = "Minst ett steg krävs.")]
    public List<RoutineCreateStepDto> Steps { get; set; } = new();
}