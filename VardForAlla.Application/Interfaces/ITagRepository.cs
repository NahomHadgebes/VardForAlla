using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;
public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int id);
    Task<List<Tag>> GetAllAsync();
    Task AddAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(int id);
}

