namespace VardForAlla.Domain.Entities;

public class Routine
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? SimpleDescription { get; set; }
    public string? OriginalDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<RoutineStep> Steps { get; set; } = new List<RoutineStep>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}

