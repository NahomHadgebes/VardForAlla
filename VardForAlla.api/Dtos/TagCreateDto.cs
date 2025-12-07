using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class TagCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
