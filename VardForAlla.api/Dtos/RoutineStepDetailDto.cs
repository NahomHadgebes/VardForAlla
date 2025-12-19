namespace VardForAlla.Api.Dtos;

public class RoutineStepDetailDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string SimpleText { get; set; } = string.Empty;
    public string OriginalText { get; set; } = string.Empty;
    public string IconKey { get; set; } = "default";
    public List<StepTranslationDto> Translations { get; set; } = new();
}
