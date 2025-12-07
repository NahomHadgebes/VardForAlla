using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class StepTranslationCreateDto
{
    [Required]
    public string LanguageCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;
}

