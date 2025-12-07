using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;

public interface ILanguageService
{
    Task<List<Language>> GetAllAsync();
    Task<Language?> GetByCodeAsync(string code);
    Task<Language> CreateAsync(string code, string name);
}

