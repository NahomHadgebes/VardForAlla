using System.Collections.Generic;
using System.Linq;
using VardForAlla.Api.Dtos;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Api.DtoBuilder
{
    public class TagDtoBuilder
    {
        public List<TagDto> BuildList(IEnumerable<Tag> tags)
        {
            return tags
                .OrderBy(t => t.Name)
                .Select(BuildItem)
                .ToList();
        }

        public TagDto BuildItem(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }
    }
}

