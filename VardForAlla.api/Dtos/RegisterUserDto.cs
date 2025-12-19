using System.ComponentModel.DataAnnotations;

namespace VardForAlla.api.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Email är obligatorisk")]
        [EmailAddress(ErrorMessage = "Ogiltig email-adress")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lösenord är obligatoriskt")]
        [MinLength(8, ErrorMessage = "Lösenord måste vara minst 8 tecken")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Förnamn är obligatoriskt")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Efternamn är obligatoriskt")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
    }
}
