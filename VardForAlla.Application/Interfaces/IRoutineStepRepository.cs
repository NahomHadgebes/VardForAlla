using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;
public interface IRoutineStepRepository
{
    Task<RoutineStep?> GetByIdAsync(int id);
    Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId);
    Task AddAsync(RoutineStep step);
    Task UpdateAsync(RoutineStep step);
    Task DeleteAsync(int id);
}

