
namespace VardForAlla.Api.Dtos;

public class RoutineDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsTemplate { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<RoutineStepDetailDto> Steps { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

