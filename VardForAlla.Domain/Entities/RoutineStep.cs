namespace VardForAlla.Domain.Entities;

public class RoutineStep
{
    public int Id { get; set; }
    public int RoutineId { get; set; }
    public Routine Routine { get; set; } = null!;
    public int Order { get; set; }
    public string SimpleText { get; set; } = string.Empty;
    public string? OriginalText { get; set; }
    public string? IconKey { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<StepTranslation> Translations { get; set; } = new List<StepTranslation>();
}