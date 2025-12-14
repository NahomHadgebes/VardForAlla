using VardForAlla.Infrastructure.Services;
using Xunit;

namespace VardForAlla.Tests.Services;

public class PasswordHasherTests
{
    private readonly PasswordHasher _sut;

    public PasswordHasherTests()
    {
        _sut = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_Ska_Generera_Hash_Med_Salt()
    {
        // ARRANGE
        var password = "TestPassword123!";

        // ACT
        var hash = _sut.HashPassword(password);

        // ASSERT
        Assert.NotNull(hash);
        Assert.Contains(".", hash);
        var parts = hash.Split('.');
        Assert.Equal(2, parts.Length);
    }

    [Fact]
    public void HashPassword_Ska_Generera_Olika_Hash_For_Samma_Losenord()
    {
        // ARRANGE
        var password = "TestPassword123!";

        // ACT
        var hash1 = _sut.HashPassword(password);
        var hash2 = _sut.HashPassword(password);

        // ASSERT
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void VerifyPassword_Nar_Losenord_Ar_Korrekt_Ska_Returnera_True()
    {
        // ARRANGE
        var password = "TestPassword123!";
        var hash = _sut.HashPassword(password);

        // ACT
        var result = _sut.VerifyPassword(password, hash);

        // ASSERT
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_Nar_Losenord_Ar_Felaktigt_Ska_Returnera_False()
    {
        // ARRANGE
        var correctPassword = "TestPassword123!";
        var wrongPassword = "WrongPassword456!";
        var hash = _sut.HashPassword(correctPassword);

        // ACT
        var result = _sut.VerifyPassword(wrongPassword, hash);

        // ASSERT
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_Nar_Hash_Ar_Ogiltigt_Format_Ska_Returnera_False()
    {
        // ARRANGE
        var password = "TestPassword123!";
        var invalidHash = "invalid-hash-format";

        // ACT
        var result = _sut.VerifyPassword(password, invalidHash);

        // ASSERT
        Assert.False(result);
    }
}
