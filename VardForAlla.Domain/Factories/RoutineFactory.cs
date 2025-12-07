using VardForAlla.Domain.Entities;

namespace VardForAlla.Domain.Factories;
public abstract class RoutineFactory
{
    public abstract Routine CreateRoutine(
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        IEnumerable<(int order, string simpleText, string? originalText, string? iconKey)> steps);
}

