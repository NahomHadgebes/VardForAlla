using VardForAlla.Api.Dtos;
using VardForAlla.Domain.Entities;
using System.Linq;

namespace VardForAlla.Api.DtoBuilder;

public class RoutineDtoBuilder
{
    public List<RoutineListDto> BuildList(IEnumerable<Routine> routines)
    {
        return routines
            .OrderBy(r => r.Category)
            .ThenBy(r => r.Title)
            .Select(BuildListItem)
            .ToList();
    }

    public RoutineListDto BuildListItem(Routine routine)
    {
        if (routine == null)
            throw new ArgumentNullException(nameof(routine));

        return new RoutineListDto
        {
            Id = routine.Id,
            Title = routine.Title,
            Category = routine.Category,
            Description = routine.SimpleDescription,
            IsActive = routine.IsActive,
            IsTemplate = routine.IsTemplate,
            StepCount = routine.Steps?.Count ?? 0,
            Tags = routine.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
            UpdatedAt = DateTime.UtcNow
        };
    }

    public RoutineDetailDto BuildDetail(Routine routine)
    {
        if (routine == null)
            throw new ArgumentNullException(nameof(routine));

        return new RoutineDetailDto
        {
            Id = routine.Id,
            Title = routine.Title,
            Category = routine.Category,
            Description = routine.SimpleDescription,
            IsTemplate = routine.IsTemplate,
            Tags = routine.Tags?.Select(t => t.Name).ToList() ?? new List<string>(),
            Steps = (routine.Steps ?? new List<RoutineStep>())
                .OrderBy(s => s.Order)
                .Select(s => new RoutineStepDetailDto
                {
                    Id = s.Id,
                    Order = s.Order,
                    SimpleText = s.SimpleText,
                    OriginalText = s.OriginalText ?? "",
                    IconKey = s.IconKey ?? "default",
                    ImageUrl = s.ImageUrl,
                    Translations = (s.Translations ?? new List<StepTranslation>())
                        .Select(t => new StepTranslationDto
                        {
                            Id = t.Id,
                            LanguageCode = t.Language?.Code ?? "",
                            TranslatedText = t.Text
                        })
                        .ToList()
                })
                .ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}