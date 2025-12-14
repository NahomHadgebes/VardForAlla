using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;

namespace VardForAlla.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailVerificationAsync(string email, string token, string userName)
    {
        var verificationUrl = $"{_configuration["App:BaseUrl"]}/verify-email?token={token}";

        _logger.LogInformation("Skickar verifieringsmail till {Email}", email);
        _logger.LogInformation("Verifieringslänk: {Url}", verificationUrl);

        await Task.CompletedTask;
    }

    public async Task SendPasswordResetAsync(string email, string token, string userName)
    {
        var resetUrl = $"{_configuration["App:BaseUrl"]}/reset-password?token={token}";

        _logger.LogInformation("Skickar återställningsmail till {Email}", email);
        _logger.LogInformation("Återställningslänk: {Url}", resetUrl);

        await Task.CompletedTask;
    }

    public async Task SendWelcomeEmailAsync(string email, string userName)
    {
        _logger.LogInformation("Skickar välkomstmail till {Email}", email);

        await Task.CompletedTask;
    }
}
