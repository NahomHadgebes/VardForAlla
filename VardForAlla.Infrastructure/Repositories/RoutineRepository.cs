
using Microsoft.EntityFrameworkCore;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Repositories;

public class RoutineRepository : IRoutineRepository
{
    private readonly VardForAllaDbContext _context;

    public RoutineRepository(VardForAllaDbContext context)
    {
        _context = context;
    }

    public async Task<Routine?> GetByIdAsync(int id)
    {
        return await _context.Routines
            .Include(r => r.Steps)
                .ThenInclude(s => s.Translations)
                    .ThenInclude(t => t.Language)
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Routine>> GetAllAsync()
    {
        return await _context.Routines
            .Where(r => r.IsActive)
            .Include(r => r.Steps) 
            .Include(r => r.Tags)
            .ToListAsync();
    }

    public async Task AddAsync(Routine routine)
    {
        _context.Routines.Add(routine);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Routine routine)
    {
        _context.Routines.Update(routine);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Routines.FindAsync(id);
        if (entity != null)
        {
            _context.Routines.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

