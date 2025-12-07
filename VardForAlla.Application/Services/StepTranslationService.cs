using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Services;

public class StepTranslationService : IStepTranslationService
{
    private readonly IStepTranslationRepository _translationRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IRoutineStepRepository _stepRepository;
    private readonly ILogger<StepTranslationService> _logger;

    public StepTranslationService(
        IStepTranslationRepository translationRepository,
        ILanguageRepository languageRepository,
        IRoutineStepRepository stepRepository,
        ILogger<StepTranslationService> logger)
    {
        _translationRepository = translationRepository;
        _languageRepository = languageRepository;
        _stepRepository = stepRepository;
        _logger = logger;
    }

    public async Task<List<StepTranslation>> GetForStepAsync(int stepId)
    {
        _logger.LogInformation("Hämtar översättningar för steg {StepId}.", stepId);
        return await _translationRepository.GetTranslationsForStepAsync(stepId);
    }

    public async Task<StepTranslation> AddTranslationAsync(int stepId, string languageCode, string text)
    {
        _logger.LogInformation("Lägger till översättning för steg {StepId} på språk {Code}.", stepId, languageCode);

        var step = await _stepRepository.GetByIdAsync(stepId);
        if (step == null)
        {
            _logger.LogWarning("Kan inte lägga till översättning. Steg {StepId} hittades inte.", stepId);
            throw new InvalidOperationException($"Steg med id {stepId} finns inte.");
        }

        var language = await _languageRepository.GetByCodeAsync(languageCode);
        if (language == null)
        {
            _logger.LogWarning("Kan inte lägga till översättning. Språk {Code} finns inte.", languageCode);
            throw new InvalidOperationException($"Språk med kod '{languageCode}' finns inte.");
        }

        var translation = new StepTranslation
        {
            RoutineStepId = stepId,
            LanguageId = language.Id,
            Text = text
        };

        await _translationRepository.AddAsync(translation);

        _logger.LogInformation("Översättning skapad med id {Id}.", translation.Id);

        return translation;
    }

    public async Task<bool> UpdateTranslationAsync(int id, string text)
    {
        _logger.LogInformation("Försöker uppdatera översättning med id {Id}.", id);

        var translation = await _translationRepository.GetByIdAsync(id);
        if (translation == null)
        {
            _logger.LogWarning("Ingen översättning hittades med id {Id}.", id);
            return false;
        }

        translation.Text = text;

        await _translationRepository.UpdateAsync(translation);

        _logger.LogInformation("Översättning med id {Id} uppdaterad.", id);

        return true;
    }

    public async Task<bool> DeleteTranslationAsync(int id)
    {
        _logger.LogInformation("Försöker ta bort översättning med id {Id}.", id);

        await _translationRepository.DeleteAsync(id);

        return true;
    }
}

