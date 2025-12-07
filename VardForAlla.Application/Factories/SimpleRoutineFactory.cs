using VardForAlla.Domain.Builders;
using VardForAlla.Domain.Entities;
using VardForAlla.Domain.Factories;

namespace VardForAlla.Application.Factories;
public class SimpleRoutineFactory : RoutineFactory
{
    public override Routine CreateRoutine(
        string title,
        string category,
        string? simpleDescription,
        string? originalDescription,
        IEnumerable<(int order, string simpleText, string? originalText, string? iconKey)> steps)
    {
        var builder = new RoutineBuilder()
            .WithTitle(title)
            .WithCategory(category)
            .WithSimpleDescription(simpleDescription)
            .WithOriginalDescription(originalDescription);

        foreach (var step in steps)
        {
            builder.AddStep(step.order, step.simpleText, step.originalText, step.iconKey);
        }

        return builder.Build();
    }
}

