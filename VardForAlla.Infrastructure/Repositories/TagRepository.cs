using Microsoft.EntityFrameworkCore;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly VardForAllaDbContext _context;

    public TagRepository(VardForAllaDbContext context)
    {
        _context = context;
    }

    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task AddAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Tag tag)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Tags.FindAsync(id);
        if (entity != null)
        {
            _context.Tags.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

