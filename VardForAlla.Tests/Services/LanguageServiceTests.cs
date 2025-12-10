using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VardForAlla.Application.Interfaces;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Entities;
using Xunit;

namespace VardForAlla.Tests.Services;

public class LanguageServiceTests
{
    private readonly Mock<ILanguageRepository> _languageRepositoryMock;
    private readonly LanguageService _sut;

    public LanguageServiceTests()
    {
        _languageRepositoryMock = new Mock<ILanguageRepository>();
        var logger = NullLogger<LanguageService>.Instance;

        _sut = new LanguageService(_languageRepositoryMock.Object, logger);
    }

    [Fact]
    public async Task GetAllAsync_Nar_Inga_Sprak_Finns_Ska_Returnera_Tom_Lista()
    {
        // ARRANGE
        _languageRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Language>());

        // ACT
        var result = await _sut.GetAllAsync();

        // ASSERT
        Assert.NotNull(result);
        Assert.Empty(result);
        _languageRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        _languageRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllAsync_Nar_Flera_Sprak_Finns_Ska_Returnera_Alla_I_Samma_Ordning()
    {
        // ARRANGE
        var languages = new List<Language>
        {
            new() { Id = 1, Code = "sv", Name = "Svenska" },
            new() { Id = 2, Code = "en", Name = "Engelska" }
        };

        _languageRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(languages);

        // ACT
        var result = await _sut.GetAllAsync();

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Collection(result,
            first =>
            {
                Assert.Equal(1, first.Id);
                Assert.Equal("sv", first.Code);
                Assert.Equal("Svenska", first.Name);
            },
            second =>
            {
                Assert.Equal(2, second.Id);
                Assert.Equal("en", second.Code);
                Assert.Equal("Engelska", second.Name);
            });

        _languageRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        _languageRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetByCodeAsync_Nar_Sprak_Finns_Ska_Returnera_Spraket()
    {
        // ARRANGE
        var language = new Language { Id = 1, Code = "sv", Name = "Svenska" };

        _languageRepositoryMock
            .Setup(r => r.GetByCodeAsync("sv"))
            .ReturnsAsync(language);

        // ACT
        var result = await _sut.GetByCodeAsync("sv");

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
        Assert.Equal("sv", result.Code);
        Assert.Equal("Svenska", result.Name);

        _languageRepositoryMock.Verify(r => r.GetByCodeAsync("sv"), Times.Once);
        _languageRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetByCodeAsync_Nar_Spraket_Inte_Finns_Ska_Returnera_Null()
    {
        // ARRANGE
        _languageRepositoryMock
            .Setup(r => r.GetByCodeAsync("sv"))
            .ReturnsAsync((Language?)null);

        // ACT
        var result = await _sut.GetByCodeAsync("sv");

        // ASSERT
        Assert.Null(result);
        _languageRepositoryMock.Verify(r => r.GetByCodeAsync("sv"), Times.Once);
        _languageRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_Nar_Kod_Är_Unik_Ska_Skapa_Sprak_Och_Anropa_AddAsync()
    {
        // ARRANGE
        _languageRepositoryMock
            .Setup(r => r.GetByCodeAsync("sv"))
            .ReturnsAsync((Language?)null);

        _languageRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Language>()))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.CreateAsync("sv", "Svenska");

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("sv", result.Code);
        Assert.Equal("Svenska", result.Name);

        _languageRepositoryMock.Verify(r => r.GetByCodeAsync("sv"), Times.Once);
        _languageRepositoryMock.Verify(
            r => r.AddAsync(It.Is<Language>(l => l.Code == "sv" && l.Name == "Svenska")),
            Times.Once);
        _languageRepositoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_Nar_Kod_Redas_Finns_Ska_Kasta_InvalidOperationException_Och_Inte_Anropa_AddAsync()
    {
        // ARRANGE
        var existing = new Language { Id = 1, Code = "sv", Name = "Svenska" };

        _languageRepositoryMock
            .Setup(r => r.GetByCodeAsync("sv"))
            .ReturnsAsync(existing);

        // ACT & ASSERT
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.CreateAsync("sv", "Dublett"));

        _languageRepositoryMock.Verify(r => r.GetByCodeAsync("sv"), Times.Once);
        _languageRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Language>()),
            Times.Never);
        _languageRepositoryMock.VerifyNoOtherCalls();
    }
}







