using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces;
public interface IStepTranslationRepository
{
    Task<List<StepTranslation>> GetTranslationsForStepAsync(int stepId);
    Task<StepTranslation?> GetByIdAsync(int id);
    Task AddAsync(StepTranslation translation);
    Task UpdateAsync(StepTranslation translation);
    Task DeleteAsync(int id);
}

