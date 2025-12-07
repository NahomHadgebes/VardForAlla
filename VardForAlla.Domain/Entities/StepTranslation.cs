namespace VardForAlla.Domain.Entities;

public class StepTranslation
{
    public int Id { get; set; }
    public int RoutineStepId { get; set; }
    public RoutineStep RoutineStep { get; set; } = null!;
    public int LanguageId { get; set; }
    public Language Language { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
}

