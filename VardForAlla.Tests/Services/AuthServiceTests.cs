using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VardForAlla.Application.Interfaces;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Entities;
using Xunit;

namespace VardForAlla.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IRoleRepository> _roleRepoMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _roleRepoMock = new Mock<IRoleRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();
        _emailServiceMock = new Mock<IEmailService>();
        var logger = NullLogger<AuthService>.Instance;

        _sut = new AuthService(
            _userRepoMock.Object,
            _roleRepoMock.Object,
            _passwordHasherMock.Object,
            _tokenServiceMock.Object,
            _emailServiceMock.Object,
            logger);
    }

    [Fact]
    public async Task LoginAsync_Nar_Anvandare_Finns_Och_Losenord_Ar_Korrekt_Ska_Returnera_Success_Och_Token()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            Email = "test@test.se",
            PasswordHash = "hashed",
            IsActive = true,
            IsEmailVerified = true,
            UserRoles = new List<UserRole>
            {
                new UserRole { RoleId = 1, Role = new Role { Name = "User" } }
            }
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.se"))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.VerifyPassword("password123", "hashed"))
            .Returns(true);

        _tokenServiceMock
            .Setup(t => t.GenerateJwtToken(user, It.IsAny<List<string>>()))
            .Returns("jwt-token");

        // ACT
        var (success, token, returnedUser) = await _sut.LoginAsync("test@test.se", "password123");

        // ASSERT
        Assert.True(success);
        Assert.Equal("jwt-token", token);
        Assert.NotNull(returnedUser);
        Assert.Equal(1, returnedUser.Id);

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.LastLoginAt.HasValue)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_Nar_Anvandare_Inte_Finns_Ska_Returnera_False()
    {
        // ARRANGE
        _userRepoMock
            .Setup(r => r.GetByEmailAsync("nonexistent@test.se"))
            .ReturnsAsync((User?)null);

        // ACT
        var (success, token, user) = await _sut.LoginAsync("nonexistent@test.se", "password123");

        // ASSERT
        Assert.False(success);
        Assert.Equal(string.Empty, token);
        Assert.Null(user);

        _passwordHasherMock.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_Nar_Losenord_Ar_Felaktigt_Ska_Returnera_False()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            Email = "test@test.se",
            PasswordHash = "hashed",
            IsActive = true,
            IsEmailVerified = true
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.se"))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.VerifyPassword("wrongpassword", "hashed"))
            .Returns(false);

        // ACT
        var (success, token, returnedUser) = await _sut.LoginAsync("test@test.se", "wrongpassword");

        // ASSERT
        Assert.False(success);
        Assert.Equal(string.Empty, token);
        Assert.Null(returnedUser);

        _tokenServiceMock.Verify(t => t.GenerateJwtToken(It.IsAny<User>(), It.IsAny<List<string>>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_Nar_Email_Inte_Ar_Verifierad_Ska_Returnera_False()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            Email = "test@test.se",
            PasswordHash = "hashed",
            IsActive = true,
            IsEmailVerified = false
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.se"))
            .ReturnsAsync(user);

        // ACT
        var (success, token, returnedUser) = await _sut.LoginAsync("test@test.se", "password123");

        // ASSERT
        Assert.False(success);
        Assert.Equal(string.Empty, token);
        Assert.Null(returnedUser);
    }

    [Fact]
    public async Task RegisterUserAsync_Nar_Admin_Och_Email_Unik_Ska_Skapa_Anvandare()
    {
        // ARRANGE
        var admin = new User
        {
            Id = 1,
            Email = "admin@test.se",
            UserRoles = new List<UserRole>
            {
                new UserRole { RoleId = 1, Role = new Role { Name = "Admin" } }
            }
        };

        var userRole = new Role { Id = 2, Name = "User" };

        _userRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(admin);

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("newuser@test.se"))
            .ReturnsAsync((User?)null);

        _roleRepoMock
            .Setup(r => r.GetByNameAsync("User"))
            .ReturnsAsync(userRole);

        _passwordHasherMock
            .Setup(h => h.HashPassword("password123"))
            .Returns("hashed-password");

        _tokenServiceMock
            .Setup(t => t.GenerateEmailVerificationToken())
            .Returns("verification-token");

        // ACT
        var (success, message) = await _sut.RegisterUserAsync(
            "newuser@test.se",
            "password123",
            "New",
            "User",
            1);

        // ASSERT
        Assert.True(success);
        Assert.Contains("registrerad", message.ToLower());

        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Email == "newuser@test.se" &&
            u.FirstName == "New" &&
            u.LastName == "User"
        )), Times.Once);

        _emailServiceMock.Verify(e => e.SendEmailVerificationAsync(
            "newuser@test.se",
            "verification-token",
            It.IsAny<string>()
        ), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_Nar_Email_Redan_Finns_Ska_Returnera_False()
    {
        // ARRANGE
        var admin = new User
        {
            Id = 1,
            UserRoles = new List<UserRole>
            {
                new UserRole { Role = new Role { Name = "Admin" } }
            }
        };

        var existingUser = new User { Id = 2, Email = "existing@test.se" };

        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(admin);
        _userRepoMock.Setup(r => r.GetByEmailAsync("existing@test.se")).ReturnsAsync(existingUser);

        // ACT
        var (success, message) = await _sut.RegisterUserAsync(
            "existing@test.se",
            "password123",
            "Test",
            "User",
            1);

        // ASSERT
        Assert.False(success);
        Assert.Contains("redan", message.ToLower());

        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUserAsync_Nar_Inte_Admin_Ska_Returnera_False()
    {
        // ARRANGE
        var nonAdmin = new User
        {
            Id = 1,
            UserRoles = new List<UserRole>
            {
                new UserRole { Role = new Role { Name = "User" } }
            }
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(nonAdmin);

        // ACT
        var (success, message) = await _sut.RegisterUserAsync(
            "test@test.se",
            "password123",
            "Test",
            "User",
            1);

        // ASSERT
        Assert.False(success);
        Assert.Contains("admin", message.ToLower());

        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task VerifyEmailAsync_Nar_Token_Ar_Giltig_Ska_Verifiera_Email()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            Email = "test@test.se",
            EmailVerificationToken = "valid-token",
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddDays(1),
            IsEmailVerified = false
        };

        _userRepoMock
            .Setup(r => r.GetByEmailVerificationTokenAsync("valid-token"))
            .ReturnsAsync(user);

        // ACT
        var (success, message) = await _sut.VerifyEmailAsync("valid-token");

        // ASSERT
        Assert.True(success);
        Assert.Contains("verifierad", message.ToLower());

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.IsEmailVerified &&
            u.EmailVerificationToken == null
        )), Times.Once);

        _emailServiceMock.Verify(e => e.SendWelcomeEmailAsync(
            user.Email,
            It.IsAny<string>()
        ), Times.Once);
    }

    [Fact]
    public async Task VerifyEmailAsync_Nar_Token_Har_Gatt_Ut_Ska_Returnera_False()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            Email = "test@test.se",
            EmailVerificationToken = "expired-token",
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddDays(-1),
            IsEmailVerified = false
        };

        _userRepoMock
            .Setup(r => r.GetByEmailVerificationTokenAsync("expired-token"))
            .ReturnsAsync(user);

        // ACT
        var (success, message) = await _sut.VerifyEmailAsync("expired-token");

        // ASSERT
        Assert.False(success);
        Assert.Contains("gått ut", message.ToLower());

        _userRepoMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task ResetPasswordAsync_Nar_Token_Ar_Giltig_Ska_Aterstalla_Losenord()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            Email = "test@test.se",
            PasswordResetToken = "reset-token",
            PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1),
            PasswordHash = "old-hash"
        };

        _userRepoMock
            .Setup(r => r.GetByPasswordResetTokenAsync("reset-token"))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(h => h.HashPassword("newpassword123"))
            .Returns("new-hash");

        // ACT
        var (success, message) = await _sut.ResetPasswordAsync("reset-token", "newpassword123");

        // ASSERT
        Assert.True(success);
        Assert.Contains("återställt", message.ToLower());

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.PasswordHash == "new-hash" &&
            u.PasswordResetToken == null
        )), Times.Once);
    }

    [Fact]
    public async Task GetUserRolesAsync_Ska_Returnera_Anvandares_Roller()
    {
        // ARRANGE
        var user = new User
        {
            Id = 1,
            UserRoles = new List<UserRole>
            {
                new UserRole { Role = new Role { Name = "Admin" } },
                new UserRole { Role = new Role { Name = "User" } }
            }
        };

        _userRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(user);

        // ACT
        var roles = await _sut.GetUserRolesAsync(1);

        // ASSERT
        Assert.Equal(2, roles.Count);
        Assert.Contains("Admin", roles);
        Assert.Contains("User", roles);
    }
}
