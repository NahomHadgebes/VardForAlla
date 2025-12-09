using System.Collections.Generic;
using System.Linq;
using VardForAlla.Api.Dtos;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Api.DtoBuilder
{
    public class RoutineStepDtoBuilder
    {
        public List<RoutineStepDto> BuildList(IEnumerable<RoutineStep> steps)
        {
            return steps
                .OrderBy(s => s.Order)
                .Select(BuildItem)
                .ToList();
        }

        public RoutineStepDto BuildItem(RoutineStep step)
        {
            return new RoutineStepDto
            {
                Order = step.Order,
                SimpleText = step.SimpleText,
                OriginalText = step.OriginalText,
                IconKey = step.IconKey
            };
        }
    }
}

