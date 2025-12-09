using System.Collections.Generic;
using System.Linq;
using VardForAlla.Api.Dtos;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Api.DtoBuilders
{
    public class LanguageDtoBuilder
    {
        public List<LanguageDto> BuildList(IEnumerable<Language> languages)
        {
            return languages
                .OrderBy(l => l.Name)
                .Select(BuildItem)
                .ToList();
        }

        public LanguageDto BuildItem(Language language)
        {
            return new LanguageDto
            {
                Id = language.Id,
                Code = language.Code,
                Name = language.Name
            };
        }
    }
}

