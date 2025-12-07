using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class StepTranslationUpdateDto
{
    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;
}

