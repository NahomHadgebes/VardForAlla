using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;
public interface ILanguageRepository
{
    Task<Language?> GetByCodeAsync(string code);
    Task<List<Language>> GetAllAsync();
    Task AddAsync(Language language);
}

