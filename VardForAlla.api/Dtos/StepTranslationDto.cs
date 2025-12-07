namespace VardForAlla.Api.Dtos;

public class StepTranslationDto
{
    public int Id { get; set; }
    public int StepId { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

