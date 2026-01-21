using Moq;
using VardForAlla.Application.Interfaces;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Behövs för .First()

namespace VardForAlla.Tests.Services;

public class RoutineServiceTests
{
    private readonly Mock<IRoutineRepository> _routineRepoMock;
    private readonly Mock<IRoutineFactory> _routineFactoryMock;
    private readonly RoutineService _sut;

    public RoutineServiceTests()
    {
        _routineRepoMock = new Mock<IRoutineRepository>();
        _routineFactoryMock = new Mock<IRoutineFactory>();
        var logger = NullLogger<RoutineService>.Instance;

        _sut = new RoutineService(_routineRepoMock.Object, _routineFactoryMock.Object, logger);
    }

    [Fact]
    public async Task GetAllAsync_Nar_Inga_Rutiner_Finns_Ska_Returnera_Tom_Lista()
    {
        // ARRANGE
        _routineRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Routine>());

        // ACT
        var result = await _sut.GetAllAsync();

        // ASSERT
        Assert.NotNull(result);
        Assert.Empty(result);

        _routineRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _routineRepoMock.VerifyNoOtherCalls();
        _routineFactoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllAsync_Nar_Rutiner_Finns_Ska_Returnera_Lista()
    {
        // ARRANGE
        var routines = new List<Routine>
        {
            new() { Id = 1, Title = "R1" },
            new() { Id = 2, Title = "R2" }
        };

        _routineRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(routines);

        // ACT
        var result = await _sut.GetAllAsync();

        // ASSERT
        Assert.Equal(2, result.Count);
        Assert.Collection(result,
            r => Assert.Equal("R1", r.Title),
            r => Assert.Equal("R2", r.Title));

        _routineRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        _routineFactoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateRoutineAsync_Ska_Anropa_Factory_Och_Repo()
    {
        // ARRANGE
        var routine = new Routine
        {
            Id = 10,
            Title = "Test titel",
            IsActive = true,
            Steps = new List<RoutineStep> { new() { Order = 1, SimpleText = "Steg 1" } }
        };

        // UPPDATERAD: tupeln har nu 5 värden (lagt till string? på slutet)
        _routineFactoryMock.Setup(f => f.CreateRoutine(
            "Test titel", "Kategori", null, null,
            It.Is<IEnumerable<(int, string, string?, string?, string?)>>(steps =>
                steps.First().Item1 == 1 &&
                steps.First().Item2 == "Steg 1")))
            .Returns(routine);

        _routineRepoMock.Setup(r => r.AddAsync(routine))
            .Returns(Task.CompletedTask);

        // ACT
        // UPPDATERAD: lagt till null som femte värde (imageUrl)
        var result = await _sut.CreateRoutineAsync(
            "Test titel", "Kategori", null, null,
            new List<(int, string, string?, string?, string?)>
            {
                (1, "Steg 1", null, null, null)
            });

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("Test titel", result.Title);
        Assert.True(result.IsActive);
        Assert.Single(result.Steps);

        // UPPDATERAD: verifiering matchar den nya 5-värdes-tupeln
        _routineFactoryMock.Verify(f => f.CreateRoutine(
            "Test titel", "Kategori", null, null, It.IsAny<IEnumerable<(int, string, string?, string?, string?)>>()),
            Times.Once);

        _routineRepoMock.Verify(r => r.AddAsync(routine), Times.Once);
        _routineRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateRoutineAsync_Nar_Rutin_Finns_Ska_Uppdatera_Och_Returnera_True()
    {
        // ARRANGE
        var routine = new Routine { Id = 99, Title = "Gammal", Category = "Kat" };

        _routineRepoMock.Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync(routine);

        _routineRepoMock.Setup(r => r.UpdateAsync(routine))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.UpdateRoutineAsync(99, "Ny", "NyKat", "desc", "orig");

        // ASSERT
        Assert.True(result);
        Assert.Equal("Ny", routine.Title);
        Assert.Equal("NyKat", routine.Category);
        Assert.Equal("desc", routine.SimpleDescription);

        _routineRepoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        _routineRepoMock.Verify(r => r.UpdateAsync(routine), Times.Once);
        _routineRepoMock.VerifyNoOtherCalls();
        _routineFactoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateRoutineAsync_Nar_Rutin_Inte_Finns_Ska_Returnera_False()
    {
        // ARRANGE
        _routineRepoMock.Setup(r => r.GetByIdAsync(42))
            .ReturnsAsync((Routine?)null);

        // ACT
        var result = await _sut.UpdateRoutineAsync(42, "T", "K", null, null);

        // ASSERT
        Assert.False(result);

        _routineRepoMock.Verify(r => r.GetByIdAsync(42), Times.Once);
        _routineRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteRoutineAsync_Ska_Markera_Som_Inaktiv_Och_Returnera_True()
    {
        // ARRANGE
        var routine = new Routine { Id = 5, IsActive = true };

        _routineRepoMock.Setup(r => r.GetByIdAsync(5))
            .ReturnsAsync(routine);

        _routineRepoMock.Setup(r => r.UpdateAsync(routine))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.DeleteRoutineAsync(5);

        // ASSERT
        Assert.True(result);
        Assert.False(routine.IsActive);

        _routineRepoMock.Verify(r => r.GetByIdAsync(5), Times.Once);
        _routineRepoMock.Verify(r => r.UpdateAsync(routine), Times.Once);
        _routineRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteRoutineAsync_Nar_Rutin_Ej_Finns_Ska_Returnera_False()
    {
        // ARRANGE
        _routineRepoMock.Setup(r => r.GetByIdAsync(123))
            .ReturnsAsync((Routine?)null);

        // ACT
        var result = await _sut.DeleteRoutineAsync(123);

        // ASSERT
        Assert.False(result);

        _routineRepoMock.Verify(r => r.GetByIdAsync(123), Times.Once);
        _routineRepoMock.VerifyNoOtherCalls();
        _routineFactoryMock.VerifyNoOtherCalls();
    }
}