using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;

public interface IRoutineService
{
    Task<List<Routine>> GetAllAsync(int? userId = null, bool includeTemplates = true);
    Task<Routine?> GetByIdAsync(int id, int? userId = null);

    Task<Routine> CreateRoutineAsync(
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        IEnumerable<(int order, string simpleText, string? originalText, string? iconKey, string? imageUrl)> steps,
        int? userId = null,
        bool isTemplate = false);

    Task<bool> UpdateRoutineAsync(
        int id,
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        int? userId = null);

    Task<bool> DeleteRoutineAsync(int id, int? userId = null);
    Task<bool> CanUserAccessRoutineAsync(int routineId, int userId, bool isAdmin);
}