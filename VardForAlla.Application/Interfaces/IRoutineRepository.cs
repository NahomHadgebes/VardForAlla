using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;
public interface IRoutineRepository
{
    Task<Routine?> GetByIdAsync(int id);
    Task<List<Routine>> GetAllAsync();
    Task AddAsync(Routine routine);
    Task UpdateAsync(Routine routine);
    Task DeleteAsync(int id);
}