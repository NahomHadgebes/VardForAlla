using Microsoft.Extensions.Logging;
using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IEmailService emailService,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<(bool Success, string Token, User? User)> LoginAsync(string email, string password)
    {
        _logger.LogInformation("Inloggningsförsök för {Email}", email);

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("Användare {Email} hittades inte", email);
            return (false, string.Empty, null);
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Användare {Email} är inaktiv", email);
            return (false, string.Empty, null);
        }

        if (!user.IsEmailVerified)
        {
            _logger.LogWarning("Email för {Email} är inte verifierad", email);
            return (false, string.Empty, null);
        }

        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            _logger.LogWarning("Felaktigt lösenord för {Email}", email);
            return (false, string.Empty, null);
        }

        var roles = await GetUserRolesAsync(user.Id);
        var token = _tokenService.GenerateJwtToken(user, roles);

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Användare {Email} loggade in framgångsrikt", email);
        return (true, token, user);
    }

    public async Task<(bool Success, string Message)> RegisterUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        int adminId)
    {
        _logger.LogInformation("Registrerar ny användare {Email}", email);

        var admin = await _userRepository.GetByIdAsync(adminId);
        if (admin == null)
        {
            return (false, "Admin hittades inte");
        }

        var adminRoles = await GetUserRolesAsync(adminId);
        if (!adminRoles.Contains("Admin"))
        {
            _logger.LogWarning("Användare {AdminId} försökte registrera användare utan admin-behörighet", adminId);
            return (false, "Endast admin kan registrera användare");
        }

        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            return (false, "Email används redan");
        }

        var user = new User
        {
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(password),
            FirstName = firstName,
            LastName = lastName,
            IsEmailVerified = false,
            EmailVerificationToken = _tokenService.GenerateEmailVerificationToken(),
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddDays(7),
            IsActive = true
        };

        await _userRepository.AddAsync(user);

        var userRole = await _roleRepository.GetByNameAsync("User");
        if (userRole != null)
        {
            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = userRole.Id
            });
            await _userRepository.UpdateAsync(user);
        }

        await _emailService.SendEmailVerificationAsync(
            user.Email,
            user.EmailVerificationToken!,
            $"{user.FirstName} {user.LastName}");

        _logger.LogInformation("Användare {Email} registrerad", email);
        return (true, "Användare registrerad. Verifieringsmail skickat.");
    }

    public async Task<(bool Success, string Message)> VerifyEmailAsync(string token)
    {
        _logger.LogInformation("Verifierar email med token");

        var user = await _userRepository.GetByEmailVerificationTokenAsync(token);
        if (user == null)
        {
            return (false, "Ogiltig verifieringstoken");
        }

        if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
        {
            return (false, "Verifieringstoken har gått ut");
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpiry = null;

        await _userRepository.UpdateAsync(user);
        await _emailService.SendWelcomeEmailAsync(user.Email, $"{user.FirstName} {user.LastName}");

        _logger.LogInformation("Email verifierad för {Email}", user.Email);
        return (true, "Email verifierad framgångsrikt");
    }

    public async Task<(bool Success, string Message)> RequestPasswordResetAsync(string email)
    {
        _logger.LogInformation("Lösenordsåterställning begärd för {Email}", email);

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return (true, "Om email finns i systemet har ett återställningsmail skickats");
        }

        user.PasswordResetToken = _tokenService.GeneratePasswordResetToken();
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

        await _userRepository.UpdateAsync(user);
        await _emailService.SendPasswordResetAsync(
            user.Email,
            user.PasswordResetToken,
            $"{user.FirstName} {user.LastName}");

        return (true, "Om email finns i systemet har ett återställningsmail skickats");
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword)
    {
        _logger.LogInformation("Återställer lösenord med token");

        var user = await _userRepository.GetByPasswordResetTokenAsync(token);
        if (user == null)
        {
            return (false, "Ogiltig återställningstoken");
        }

        if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            return (false, "Återställningstoken har gått ut");
        }

        user.PasswordHash = _passwordHasher.HashPassword(newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;

        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Lösenord återställt för {Email}", user.Email);
        return (true, "Lösenord återställt framgångsrikt");
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new List<string>();
        }

        return user.UserRoles.Select(ur => ur.Role.Name).ToList();
    }
}
