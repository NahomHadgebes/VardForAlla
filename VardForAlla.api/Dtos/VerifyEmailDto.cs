using System.ComponentModel.DataAnnotations;

namespace VardForAlla.api.Dtos
{
    public class VerifyEmailDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
