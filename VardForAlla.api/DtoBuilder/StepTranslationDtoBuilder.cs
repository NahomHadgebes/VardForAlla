using System.Collections.Generic;
using System.Linq;
using VardForAlla.Api.Dtos;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Api.DtoBuilder
{
    public class StepTranslationDtoBuilder
    {
        public List<StepTranslationDto> BuildList(IEnumerable<StepTranslation> translations)
        {
            return translations
                .Select(BuildItem)
                .ToList();
        }

        public StepTranslationDto BuildItem(StepTranslation translation)
        {
            return new StepTranslationDto
            {
                Id = translation.Id,
                LanguageCode = translation.Language?.Code ?? string.Empty,
            };
        }
    }
}
