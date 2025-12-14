using Microsoft.EntityFrameworkCore;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly VardForAllaDbContext _context;

    public RoleRepository(VardForAllaDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task AddAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
    }
}
