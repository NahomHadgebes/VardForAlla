using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Services;

public class RoutineService : IRoutineService
{
    private readonly IRoutineRepository _routineRepository;
    private readonly IRoutineFactory _routineFactory;
    private readonly ILogger<RoutineService> _logger;

    public RoutineService(
        IRoutineRepository routineRepository,
        IRoutineFactory factory,
        ILogger<RoutineService> logger)
    {
        _routineRepository = routineRepository;
        _routineFactory = factory;
        _logger = logger;
    }

    public async Task<List<Routine>> GetAllAsync(int? userId = null, bool includeTemplates = true)
    {
        _logger.LogInformation("Hämtar rutiner för användare {UserId}", userId);
        var routines = await _routineRepository.GetAllAsync();

        if (userId.HasValue)
        {
            routines = routines.Where(r =>
                r.UserId == userId.Value ||
                (r.IsTemplate && includeTemplates))
                .ToList();
        }

        _logger.LogInformation("Hämtade {Count} rutiner.", routines.Count);
        return routines;
    }

    public async Task<Routine?> GetByIdAsync(int id, int? userId = null)
    {
        _logger.LogInformation("Hämtar rutin med id {Id}.", id);
        var routine = await _routineRepository.GetByIdAsync(id);

        if (routine == null)
        {
            _logger.LogWarning("Ingen rutin hittades med id {Id}.", id);
            return null;
        }

        if (userId.HasValue && routine.UserId != userId.Value && !routine.IsTemplate)
        {
            _logger.LogWarning("Användare {UserId} har inte åtkomst till rutin {Id}", userId, id);
            return null;
        }

        return routine;
    }

    public async Task<Routine> CreateRoutineAsync(
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        IEnumerable<(int order, string simpleText, string? originalText, string? iconKey)> steps,
        int? userId = null,
        bool isTemplate = false)
    {
        _logger.LogInformation("Skapar ny rutin med titel {Title}.", title);

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Titel får inte vara tom.", nameof(title));

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Kategori får inte vara tom.", nameof(category));

        if (!steps.Any())
            throw new ArgumentException("Minst ett steg krävs.", nameof(steps));

        var routine = _routineFactory.CreateRoutine(
            title,
            category,
            simpleDescription,
            originalDescription,
            steps);

        routine.UserId = userId;
        routine.IsTemplate = isTemplate;

        await _routineRepository.AddAsync(routine);

        _logger.LogInformation("Rutin skapad med id {Id}.", routine.Id);

        return routine;
    }

    public async Task<bool> UpdateRoutineAsync(
        int id,
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        int? userId = null)
    {
        _logger.LogInformation("Försöker uppdatera rutin med id {Id}.", id);

        var routine = await _routineRepository.GetByIdAsync(id);
        if (routine == null)
        {
            _logger.LogWarning("Kan inte uppdatera rutin. Ingen rutin hittades med id {Id}.", id);
            return false;
        }

        if (userId.HasValue && routine.UserId != userId.Value)
        {
            _logger.LogWarning("Användare {UserId} försökte uppdatera rutin {Id} utan behörighet", userId, id);
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

    public async Task<bool> DeleteRoutineAsync(int id, int? userId = null)
    {
        _logger.LogInformation("Försöker ta bort (soft delete) rutin med id {Id}.", id);

        var routine = await _routineRepository.GetByIdAsync(id);
        if (routine == null)
        {
            _logger.LogWarning("Kan inte ta bort rutin. Ingen rutin hittades med id {Id}.", id);
            return false;
        }

        if (userId.HasValue && routine.UserId != userId.Value)
        {
            _logger.LogWarning("Användare {UserId} försökte ta bort rutin {Id} utan behörighet", userId, id);
            return false;
        }

        routine.IsActive = false;
        await _routineRepository.UpdateAsync(routine);

        _logger.LogInformation("Rutin med id {Id} markerad som inaktiv.", id);

        return true;
    }

    public async Task<bool> CanUserAccessRoutineAsync(int routineId, int userId, bool isAdmin)
    {
        if (isAdmin) return true;

        var routine = await _routineRepository.GetByIdAsync(routineId);
        if (routine == null) return false;

        return routine.UserId == userId || routine.IsTemplate;
    }
}

