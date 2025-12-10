using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VardForAlla.Application.Interfaces;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Entities;
using Xunit;

namespace VardForAlla.Tests.Services;

public class StepTranslationServiceTests
{
    private readonly Mock<IStepTranslationRepository> _translationRepoMock;
    private readonly Mock<ILanguageRepository> _languageRepoMock;
    private readonly Mock<IRoutineStepRepository> _stepRepoMock;
    private readonly StepTranslationService _sut;

    public StepTranslationServiceTests()
    {
        _translationRepoMock = new Mock<IStepTranslationRepository>();
        _languageRepoMock = new Mock<ILanguageRepository>();
        _stepRepoMock = new Mock<IRoutineStepRepository>();
        var logger = NullLogger<StepTranslationService>.Instance;

        _sut = new StepTranslationService(
            _translationRepoMock.Object,
            _languageRepoMock.Object,
            _stepRepoMock.Object,
            logger);
    }

    [Fact]
    public async Task GetForStepAsync_Ska_Returnera_Tom_Lista_Nar_Inget_Finns()
    {
        // ARRANGE
        _translationRepoMock
            .Setup(r => r.GetTranslationsForStepAsync(1))
            .ReturnsAsync(new List<StepTranslation>());

        // ACT
        var result = await _sut.GetForStepAsync(1);

        // ASSERT
        Assert.NotNull(result);
        Assert.Empty(result);

        _translationRepoMock.Verify(r => r.GetTranslationsForStepAsync(1), Times.Once);
        _translationRepoMock.VerifyNoOtherCalls();
        _languageRepoMock.VerifyNoOtherCalls();
        _stepRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetForStepAsync_Nar_Translations_Finns_Ska_Returnera_Dem()
    {
        // ARRANGE
        var translations = new List<StepTranslation>
        {
            new() { Id = 1, RoutineStepId = 5, Text = "Hej" },
            new() { Id = 2, RoutineStepId = 5, Text = "Hello" }
        };

        _translationRepoMock
            .Setup(r => r.GetTranslationsForStepAsync(5))
            .ReturnsAsync(translations);

        // ACT
        var result = await _sut.GetForStepAsync(5);

        // ASSERT
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Id == 1 && t.Text == "Hej");
        Assert.Contains(result, t => t.Id == 2 && t.Text == "Hello");

        _translationRepoMock.Verify(r => r.GetTranslationsForStepAsync(5), Times.Once);
    }

    [Fact]
    public async Task AddTranslationAsync_Ska_Skapa_Oversattning_Nar_Steg_Och_Sprak_Finns()
    {
        // ARRANGE
        var step = new RoutineStep { Id = 10, Order = 1, SimpleText = "Simple" };
        var language = new Language { Id = 3, Code = "sv", Name = "Svenska" };

        _stepRepoMock
            .Setup(r => r.GetByIdAsync(step.Id))
            .ReturnsAsync(step);

        _languageRepoMock
            .Setup(r => r.GetByCodeAsync("sv"))
            .ReturnsAsync(language);

        _translationRepoMock
            .Setup(r => r.AddAsync(It.IsAny<StepTranslation>()))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.AddTranslationAsync(step.Id, "sv", "Hej");

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(step.Id, result.RoutineStepId);
        Assert.Equal(language.Id, result.LanguageId);
        Assert.Equal("Hej", result.Text);

        _stepRepoMock.Verify(r => r.GetByIdAsync(step.Id), Times.Once);
        _languageRepoMock.Verify(r => r.GetByCodeAsync("sv"), Times.Once);
        _translationRepoMock.Verify(r => r.AddAsync(It.Is<StepTranslation>(t =>
            t.RoutineStepId == step.Id &&
            t.LanguageId == language.Id &&
            t.Text == "Hej")), Times.Once);
    }

    [Fact]
    public async Task AddTranslationAsync_Ska_Kasta_InvalidOperation_Nar_Steg_Ej_Finns()
    {
        // ARRANGE
        _stepRepoMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((RoutineStep?)null);

        // ACT & ASSERT
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.AddTranslationAsync(999, "sv", "Hej"));

        _stepRepoMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        _languageRepoMock.Verify(r => r.GetByCodeAsync(It.IsAny<string>()), Times.Never);
        _translationRepoMock.Verify(r => r.AddAsync(It.IsAny<StepTranslation>()), Times.Never);
    }

    [Fact]
    public async Task AddTranslationAsync_Ska_Kasta_InvalidOperation_Nar_Sprak_Ej_Finns()
    {
        // ARRANGE
        var step = new RoutineStep { Id = 10 };

        _stepRepoMock
            .Setup(r => r.GetByIdAsync(step.Id))
            .ReturnsAsync(step);

        _languageRepoMock
            .Setup(r => r.GetByCodeAsync("xx"))
            .ReturnsAsync((Language?)null);

        // ACT & ASSERT
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.AddTranslationAsync(step.Id, "xx", "Hej"));

        _stepRepoMock.Verify(r => r.GetByIdAsync(step.Id), Times.Once);
        _languageRepoMock.Verify(r => r.GetByCodeAsync("xx"), Times.Once);
        _translationRepoMock.Verify(r => r.AddAsync(It.IsAny<StepTranslation>()), Times.Never);
    }

    [Fact]
    public async Task UpdateTranslationAsync_Ska_Uppdatera_Text_Nar_Translation_Finns()
    {
        // ARRANGE
        var translation = new StepTranslation
        {
            Id = 5,
            RoutineStepId = 10,
            LanguageId = 3,
            Text = "Gamla"
        };

        _translationRepoMock
            .Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync(translation);

        _translationRepoMock
            .Setup(r => r.UpdateAsync(translation))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.UpdateTranslationAsync(5, "Nya");

        // ASSERT
        Assert.True(result);
        Assert.Equal("Nya", translation.Text);

        _translationRepoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        _translationRepoMock.Verify(r => r.UpdateAsync(translation), Times.Once);
    }

    [Fact]
    public async Task UpdateTranslationAsync_Ska_Returnera_False_Nar_Translation_Ej_Finns()
    {
        // ARRANGE
        _translationRepoMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((StepTranslation?)null);

        // ACT
        var result = await _sut.UpdateTranslationAsync(999, "Nya");

        // ASSERT
        Assert.False(result);

        _translationRepoMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        _translationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<StepTranslation>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTranslationAsync_Ska_Ta_Bort_Oversattning_Nar_Finns()
    {
        // ARRANGE
        var translation = new StepTranslation { Id = 7 };

        _translationRepoMock
            .Setup(r => r.GetByIdAsync(7))
            .ReturnsAsync(translation);

        _translationRepoMock
            .Setup(r => r.DeleteAsync(7))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.DeleteTranslationAsync(7);

        // ASSERT
        Assert.True(result);

        _translationRepoMock.Verify(r => r.GetByIdAsync(7), Times.Once);
        _translationRepoMock.Verify(r => r.DeleteAsync(7), Times.Once);
    }

    [Fact]
    public async Task DeleteTranslationAsync_Ska_Returnera_False_Nar_Translation_Ej_Finns()
    {
        // ARRANGE
        _translationRepoMock
            .Setup(r => r.GetByIdAsync(123))
            .ReturnsAsync((StepTranslation?)null);

        // ACT
        var result = await _sut.DeleteTranslationAsync(123);

        // ASSERT
        Assert.False(result);

        _translationRepoMock.Verify(r => r.GetByIdAsync(123), Times.Once);
        _translationRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}



