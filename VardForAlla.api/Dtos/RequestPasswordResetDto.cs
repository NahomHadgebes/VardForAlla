using System.ComponentModel.DataAnnotations;

namespace VardForAlla.api.Dtos
{
    public class RequestPasswordResetDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
