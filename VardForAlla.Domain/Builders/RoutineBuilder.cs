using VardForAlla.Domain.Entities;

namespace VardForAlla.Domain.Builders;

public class RoutineBuilder
{
    private readonly Routine _routine;

    public RoutineBuilder()
    {
        _routine = new Routine();
    }

    public RoutineBuilder WithTitle(string title)
    {
        _routine.Title = title;
        return this;
    }

    public RoutineBuilder WithCategory(string category)
    {
        _routine.Category = category;
        return this;
    }

    public RoutineBuilder WithSimpleDescription(string? description)
    {
        _routine.SimpleDescription = description;
        return this;
    }

    public RoutineBuilder WithOriginalDescription(string? description)
    {
        _routine.OriginalDescription = description;
        return this;
    }

    public RoutineBuilder AddStep(int order, string simpleText, string? originalText = null, string? iconKey = null, string? imageUrl = null)
    {
        _routine.Steps.Add(new RoutineStep
        {
            Order = order,
            SimpleText = simpleText,
            OriginalText = originalText,
            IconKey = iconKey,
            ImageUrl = imageUrl 
        });

        return this;
    }

    public Routine Build()
    {
        return _routine;
    }
}