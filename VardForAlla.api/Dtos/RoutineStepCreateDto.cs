using System.ComponentModel.DataAnnotations;

public class RoutineStepCreateDto
{
    [Required]
    public int Order { get; set; }

    [Required]
    [MaxLength(500)]
    public string SimpleText { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string OriginalText { get; set; } = string.Empty;

    public string? IconKey { get; set; }

    public List<StepTranslationCreateDto> Translations { get; set; } = new();
}
