using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Services;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILogger<LanguageService> _logger;

    public LanguageService(ILanguageRepository languageRepository, ILogger<LanguageService> logger)
    {
        _languageRepository = languageRepository;
        _logger = logger;
    }

    public async Task<List<Language>> GetAllAsync()
    {
        _logger.LogInformation("Hämtar alla språk.");
        return await _languageRepository.GetAllAsync();
    }

    public async Task<Language?> GetByCodeAsync(string code)
    {
        _logger.LogInformation("Hämtar språk med kod {Code}.", code);
        return await _languageRepository.GetByCodeAsync(code);
    }

    public async Task<Language> CreateAsync(string code, string name)
    {
        _logger.LogInformation("Försöker skapa nytt språk med kod {Code}.", code);

        var existing = await _languageRepository.GetByCodeAsync(code);
        if (existing != null)
        {
            _logger.LogWarning("Språk med kod {Code} finns redan.", code);
            throw new InvalidOperationException($"Språk med kod '{code}' finns redan.");
        }

        var language = new Language
        {
            Code = code,
            Name = name
        };

        await _languageRepository.AddAsync(language);

        _logger.LogInformation("Språk {Name} ({Code}) skapat med id {Id}.", name, code, language.Id);

        return language;
    }
}

