using System.ComponentModel.DataAnnotations;
public class RoutineStepCreateDto
{
    [Required]
    public int Order { get; set; }

    [Required]
    [MaxLength(500)]
    public string SimpleText { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? OriginalText { get; set; }

    public string? IconKey { get; set; }

    public string? ImageUrl { get; set; }

    public List<StepTranslationCreateDto> Translations { get; set; } = new();
}