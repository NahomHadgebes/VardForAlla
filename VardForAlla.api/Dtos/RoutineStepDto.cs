namespace VardForAlla.Api.Dtos;

public class RoutineStepDto
{
    public int Order { get; set; }
    public string SimpleText { get; set; } = string.Empty;
    public string? OriginalText { get; set; }
    public string? IconKey { get; set; }
}