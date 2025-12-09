using Microsoft.Extensions.Logging;
using VardForAlla.Application.Factories;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Domain.Factories;

namespace VardForAlla.Application.Services;

public class RoutineService : IRoutineService
{
    private readonly IRoutineRepository _routineRepository;
    private readonly IRoutineFactory _routineFactory;
    private readonly ILogger<RoutineService> _logger;

    public RoutineService(IRoutineRepository routineRepository, IRoutineFactory factory, ILogger<RoutineService> logger)
    {
        _routineRepository = routineRepository;
        _routineFactory = factory;
        _logger = logger;
    }

    public async Task<List<Routine>> GetAllAsync()
    {
        _logger.LogInformation("Hämtar alla aktiva rutiner.");
        var routines = await _routineRepository.GetAllAsync();
        _logger.LogInformation("Hämtade {Count} rutiner.", routines.Count);
        return routines;
    }

    public async Task<Routine?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Hämtar rutin med id {Id}.", id);
        var routine = await _routineRepository.GetByIdAsync(id);

        if (routine == null)
        {
            _logger.LogWarning("Ingen rutin hittades med id {Id}.", id);
        }

        return routine;
    }

    public async Task<Routine> CreateRoutineAsync(
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        IEnumerable<(int order, string simpleText, string? originalText, string? iconKey)> steps)
    {
        _logger.LogInformation("Skapar ny rutin med titel {Title}.", title);

        var routine = _routineFactory.CreateRoutine(
            title,
            category,
            simpleDescription,
            originalDescription,
            steps);

        await _routineRepository.AddAsync(routine);

        _logger.LogInformation("Rutin skapad med id {Id}.", routine.Id);

        return routine;
    }

    public async Task<bool> UpdateRoutineAsync(
        int id,
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription)
    {
        _logger.LogInformation("Försöker uppdatera rutin med id {Id}.", id);

        var routine = await _routineRepository.GetByIdAsync(id);
        if (routine == null)
        {
            _logger.LogWarning("Kan inte uppdatera rutin. Ingen rutin hittades med id {Id}.", id);
            return false;
        }

        routine.Title = title;
        routine.Category = category;
        routine.SimpleDescription = simpleDescription;
        routine.OriginalDescription = originalDescription;

        await _routineRepository.UpdateAsync(routine);

        _logger.LogInformation("Rutin med id {Id} uppdaterad.", id);

        return true;
    }

    public async Task<bool> DeleteRoutineAsync(int id)
    {
        _logger.LogInformation("Försöker ta bort (soft delete) rutin med id {Id}.", id);

        var routine = await _routineRepository.GetByIdAsync(id);
        if (routine == null)
        {
            _logger.LogWarning("Kan inte ta bort rutin. Ingen rutin hittades med id {Id}.", id);
            return false;
        }

        routine.IsActive = false;
        await _routineRepository.UpdateAsync(routine);

        _logger.LogInformation("Rutin med id {Id} markerad som inaktiv.", id);

        return true;
    }
}

