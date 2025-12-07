using Microsoft.EntityFrameworkCore;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Repositories;

public class RoutineStepRepository : IRoutineStepRepository
{
    private readonly VardForAllaDbContext _context;

    public RoutineStepRepository(VardForAllaDbContext context)
    {
        _context = context;
    }

    public async Task<RoutineStep?> GetByIdAsync(int id)
    {
        return await _context.RoutineSteps
            .Include(s => s.Translations)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId)
    {
        return await _context.RoutineSteps
            .Where(s => s.RoutineId == routineId)
            .OrderBy(s => s.Order)
            .Include(s => s.Translations)
            .ToListAsync();
    }

    public async Task AddAsync(RoutineStep step)
    {
        _context.RoutineSteps.Add(step);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RoutineStep step)
    {
        _context.RoutineSteps.Update(step);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.RoutineSteps.FindAsync(id);
        if (entity != null)
        {
            _context.RoutineSteps.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

