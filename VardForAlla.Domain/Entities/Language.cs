namespace VardForAlla.Domain.Entities;

public class Language
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<StepTranslation> Translations { get; set; } = new List<StepTranslation>();
}
