using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;

public interface IRoutineStepService
{
    Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId);
    Task<RoutineStep?> GetByIdAsync(int id);

    Task<RoutineStep> AddStepAsync(
        int routineId,
        int order,
        string simpleText,
        string? originalText,
        string? iconKey);

    Task<bool> UpdateStepAsync(
        int id,
        int order,
        string simpleText,
        string? originalText,
        string? iconKey);

    Task<bool> DeleteStepAsync(int id);
}

