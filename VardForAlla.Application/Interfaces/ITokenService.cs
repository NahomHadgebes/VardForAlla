using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user, List<string> roles);
        string GenerateEmailVerificationToken();
        string GeneratePasswordResetToken();
    }
}
