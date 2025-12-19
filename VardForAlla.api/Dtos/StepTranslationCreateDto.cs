using System.ComponentModel.DataAnnotations;

public class StepTranslationCreateDto
{
    [Required]
    public string LanguageCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string TranslatedText { get; set; } = string.Empty;
}
