using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;

public interface ITagService
{
    Task<List<Tag>> GetAllAsync();
    Task<Tag?> GetByIdAsync(int id);
    Task<Tag> CreateAsync(string name);
    Task<bool> UpdateAsync(int id, string name);
    Task<bool> DeleteAsync(int id);
}

