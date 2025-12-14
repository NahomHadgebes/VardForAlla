using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task<List<Role>> GetAllAsync();
        Task AddAsync(Role role);
    }
}
