namespace VardForAlla.Api.Dtos;

public class RoutineDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? SimpleDescription { get; set; }
    public string? OriginalDescription { get; set; }

    public List<RoutineStepDto> Steps { get; set; } = new();
}

