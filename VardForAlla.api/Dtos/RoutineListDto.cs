namespace VardForAlla.Api.Dtos;

public class RoutineListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsTemplate { get; set; }
    public int StepCount { get; set; } 
    public List<string> Tags { get; set; } = new();
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}