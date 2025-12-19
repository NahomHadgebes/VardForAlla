using System.ComponentModel.DataAnnotations;

public class RoutineCreateDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public string? Description { get; set; }

    public List<string> Tags { get; set; } = new();

    [MinLength(1, ErrorMessage = "Minst ett steg krävs.")]
    public List<RoutineStepCreateDto> Steps { get; set; } = new();

    public bool? IsTemplate { get; set; }
}