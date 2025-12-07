using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;

public interface IStepTranslationService
{
    Task<List<StepTranslation>> GetForStepAsync(int stepId);
    Task<StepTranslation> AddTranslationAsync(
        int stepId,
        string languageCode,
        string text);
    Task<bool> UpdateTranslationAsync(int id, string text);
    Task<bool> DeleteTranslationAsync(int id);
}

