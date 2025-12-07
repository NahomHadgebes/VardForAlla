using System.ComponentModel.DataAnnotations;

namespace VardForAlla.Api.Dtos;

public class LanguageCreateDto
{
    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}

