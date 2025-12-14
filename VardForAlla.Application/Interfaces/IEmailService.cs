namespace VardForAlla.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(string email, string token, string userName);
        Task SendPasswordResetAsync(string email, string token, string userName);
        Task SendWelcomeEmailAsync(string email, string userName);
    }
}
