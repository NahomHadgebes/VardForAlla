namespace VardForAlla.Api.Models;

public class ApiErrorResponse
{
    public string Message { get; set; } = "Ett oväntat fel inträffade.";
    public string? Detail { get; set; }
    public string TraceId { get; set; } = string.Empty;
}

