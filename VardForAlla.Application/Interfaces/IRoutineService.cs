using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;
public interface IRoutineService
{
    Task<List<Routine>> GetAllAsync();
    Task<Routine?> GetByIdAsync(int id);

    Task<Routine> CreateRoutineAsync(
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        IEnumerable<(int order, string simpleText, string? originalText, string? iconKey)> steps);

    Task<bool> UpdateRoutineAsync(
        int id,
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription);

    Task<bool> DeleteRoutineAsync(int id);
}

