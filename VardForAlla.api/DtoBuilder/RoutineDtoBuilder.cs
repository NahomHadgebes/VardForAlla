using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using VardForAlla.Api.Dtos;
using VardForAlla.Domain.Entities;

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
            throw new ArgumentNullException();

        return new RoutineListDto
        {
            Id = routine.Id,
            Title = routine.Title,
            Category = routine.Category,
            IsActive = routine.IsActive
        };
    }

    public RoutineDetailDto BuildDetail(Routine routine)
    {
        if (routine == null)
            throw new ArgumentNullException();

        return new RoutineDetailDto
        {
            Id = routine.Id,
            Title = routine.Title,
            Category = routine.Category,
            SimpleDescription = routine.SimpleDescription,
            OriginalDescription = routine.OriginalDescription,
            Steps = routine.Steps
                .OrderBy(s => s.Order)
                .Select(s => new RoutineStepDto
                {
                    Order = s.Order,
                    SimpleText = s.SimpleText,
                    OriginalText = s.OriginalText,
                    IconKey = s.IconKey
                })
                .ToList()
        };
    }
}

