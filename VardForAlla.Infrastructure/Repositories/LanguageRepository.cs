using Microsoft.EntityFrameworkCore;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Repositories;

public class LanguageRepository : ILanguageRepository
{
    private readonly VardForAllaDbContext _context;

    public LanguageRepository(VardForAllaDbContext context)
    {
        _context = context;
    }

    public async Task<Language?> GetByCodeAsync(string code)
    {
        return await _context.Languages.FirstOrDefaultAsync(l => l.Code == code);
    }

    public async Task<List<Language>> GetAllAsync()
    {
        return await _context.Languages.ToListAsync();
    }

    public async Task AddAsync(Language language)
    {
        _context.Languages.Add(language);
        await _context.SaveChangesAsync();
    }
}

