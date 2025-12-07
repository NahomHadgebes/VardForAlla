using Microsoft.EntityFrameworkCore;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Repositories;

public class StepTranslationRepository : IStepTranslationRepository
{
    private readonly VardForAllaDbContext _context;

    public StepTranslationRepository(VardForAllaDbContext context)
    {
        _context = context;
    }

    public async Task<StepTranslation?> GetByIdAsync(int id)
    {
        return await _context.StepTranslations
            .Include(t => t.Language)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<StepTranslation>> GetTranslationsForStepAsync(int stepId)
    {
        return await _context.StepTranslations
            .Where(t => t.RoutineStepId == stepId)
            .Include(t => t.Language)
            .ToListAsync();
    }

    public async Task AddAsync(StepTranslation translation)
    {
        _context.StepTranslations.Add(translation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StepTranslation translation)
    {
        _context.StepTranslations.Update(translation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.StepTranslations.FindAsync(id);
        if (entity != null)
        {
            _context.StepTranslations.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

