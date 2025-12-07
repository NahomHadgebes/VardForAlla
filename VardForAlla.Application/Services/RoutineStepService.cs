using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Services;

public class RoutineStepService : IRoutineStepService
{
    private readonly IRoutineStepRepository _stepRepository;
    private readonly IRoutineRepository _routineRepository;
    private readonly ILogger<RoutineStepService> _logger;

    public RoutineStepService(
        IRoutineStepRepository stepRepository,
        IRoutineRepository routineRepository,
        ILogger<RoutineStepService> logger)
    {
        _stepRepository = stepRepository;
        _routineRepository = routineRepository;
        _logger = logger;
    }

    public async Task<List<RoutineStep>> GetByRoutineIdAsync(int routineId)
    {
        _logger.LogInformation("Hämtar steg för rutin {RoutineId}.", routineId);
        return await _stepRepository.GetByRoutineIdAsync(routineId);
    }

    public async Task<RoutineStep?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Hämtar steg med id {Id}.", id);
        return await _stepRepository.GetByIdAsync(id);
    }

    public async Task<RoutineStep> AddStepAsync(
        int routineId,
        int order,
        string simpleText,
        string? originalText,
        string? iconKey)
    {
        _logger.LogInformation("Försöker lägga till steg i rutin {RoutineId}.", routineId);

        var routine = await _routineRepository.GetByIdAsync(routineId);
        if (routine == null)
        {
            _logger.LogWarning("Kan inte lägga till steg. Rutin {RoutineId} hittades inte.", routineId);
            throw new InvalidOperationException($"Rutin med id {routineId} finns inte.");
        }

        var step = new RoutineStep
        {
            RoutineId = routineId,
            Order = order,
            SimpleText = simpleText,
            OriginalText = originalText,
            IconKey = iconKey
        };

        await _stepRepository.AddAsync(step);

        _logger.LogInformation("Steg med id {Id} skapades för rutin {RoutineId}.", step.Id, routineId);

        return step;
    }

    public async Task<bool> UpdateStepAsync(
        int id,
        int order,
        string simpleText,
        string? originalText,
        string? iconKey)
    {
        _logger.LogInformation("Försöker uppdatera steg med id {Id}.", id);

        var step = await _stepRepository.GetByIdAsync(id);
        if (step == null)
        {
            _logger.LogWarning("Ingen steg hittades med id {Id}.", id);
            return false;
        }

        step.Order = order;
        step.SimpleText = simpleText;
        step.OriginalText = originalText;
        step.IconKey = iconKey;

        await _stepRepository.UpdateAsync(step);

        _logger.LogInformation("Steg med id {Id} uppdaterat.", id);

        return true;
    }

    public async Task<bool> DeleteStepAsync(int id)
    {
        _logger.LogInformation("Försöker ta bort steg med id {Id}.", id);

        var step = await _stepRepository.GetByIdAsync(id);
        if (step == null)
        {
            _logger.LogWarning("Kan inte ta bort steg. Inget steg hittades med id {Id}.", id);
            return false;
        }

        await _stepRepository.DeleteAsync(id);

        _logger.LogInformation("Steg med id {Id} borttaget.", id);

        return true;
    }
}

