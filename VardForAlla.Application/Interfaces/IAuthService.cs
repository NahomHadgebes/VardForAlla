using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, User? User)> LoginAsync(string email, string password);
        Task<(bool Success, string Message)> RegisterUserAsync(string email, string password, string firstName, string lastName, int adminId);
        Task<(bool Success, string Message)> VerifyEmailAsync(string token);
        Task<(bool Success, string Message)> RequestPasswordResetAsync(string email);
        Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<string>> GetUserRolesAsync(int userId);
    }
}
