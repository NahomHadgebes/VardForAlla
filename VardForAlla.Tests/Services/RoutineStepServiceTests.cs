using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VardForAlla.Application.Interfaces;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Entities;
using Xunit;

namespace VardForAlla.Tests.Services;

public class RoutineStepServiceTests
{
    private readonly Mock<IRoutineStepRepository> _stepRepoMock;
    private readonly Mock<IRoutineRepository> _routineRepoMock;
    private readonly RoutineStepService _sut;

    public RoutineStepServiceTests()
    {
        _stepRepoMock = new Mock<IRoutineStepRepository>();
        _routineRepoMock = new Mock<IRoutineRepository>();
        var logger = NullLogger<RoutineStepService>.Instance;

        _sut = new RoutineStepService(_stepRepoMock.Object, _routineRepoMock.Object, logger);
    }

    [Fact]
    public async Task AddStepAsync_Ska_Lägga_Till_Steg_Till_Routine_Och_Anropa_AddAsync()
    {
        // ARRANGE
        var routine = new Routine { Id = 10, Title = "Rutintest", Category = "Cat" };

        _routineRepoMock
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(routine);

        _stepRepoMock
            .Setup(s => s.AddAsync(It.IsAny<RoutineStep>()))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.AddStepAsync(10, 1, "Steg1", "Orig1", null);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(10, result.RoutineId);
        Assert.Equal(1, result.Order);
        Assert.Equal("Steg1", result.SimpleText);
        Assert.Equal("Orig1", result.OriginalText);

        _routineRepoMock.Verify(r => r.GetByIdAsync(10), Times.Once);
        _stepRepoMock.Verify(s => s.AddAsync(It.Is<RoutineStep>(st =>
            st.RoutineId == 10 &&
            st.Order == 1 &&
            st.SimpleText == "Steg1" &&
            st.OriginalText == "Orig1")), Times.Once);

        _stepRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AddStepAsync_Nar_Rutin_Inte_Finns_Ska_Kasta_InvalidOperationException()
    {
        // ARRANGE
        _routineRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Routine?)null);

        // ACT & ASSERT
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _sut.AddStepAsync(99, 1, "Steg1", null, null));

        _routineRepoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        _stepRepoMock.Verify(s => s.AddAsync(It.IsAny<RoutineStep>()), Times.Never);
    }

    [Fact]
    public async Task GetByRoutineIdAsync_Ska_Returnera_Steg_Sorterade_Efter_Order()
    {
    // ARRANGE     
        var steps = new List<RoutineStep>
    {
        new() { Id = 1, RoutineId = 10, Order = 1, SimpleText = "A" },
        new() { Id = 2, RoutineId = 10, Order = 2, SimpleText = "B" }
    };

        _stepRepoMock
            .Setup(s => s.GetByRoutineIdAsync(10))
            .ReturnsAsync(steps);

        // ACT
        var result = await _sut.GetByRoutineIdAsync(10);

        // ASSERT
        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].SimpleText);
        Assert.Equal("B", result[1].SimpleText);

        _stepRepoMock.Verify(s => s.GetByRoutineIdAsync(10), Times.Once);
        _stepRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetStepsForRoutineAsync_Nar_Inga_Steg_Finns_Ska_Returnera_Tom_Lista()
    {
        // ARRANGE
        _stepRepoMock
            .Setup(s => s.GetByRoutineIdAsync(10))
            .ReturnsAsync(new List<RoutineStep>());

        // ACT
        var result = await _sut.GetByRoutineIdAsync(10);

        // ASSERT
        Assert.NotNull(result);
        Assert.Empty(result);

        _stepRepoMock.Verify(s => s.GetByRoutineIdAsync(10), Times.Once);
    }

    [Fact]
    public async Task UpdateStepAsync_Ska_Uppdatera_Falt_Och_Returnera_True()
    {
        // ARRANGE
        var step = new RoutineStep
        {
            Id = 5,
            RoutineId = 10,
            Order = 1,
            SimpleText = "Gamla",
            OriginalText = "Orig",
            IconKey = "old-icon"
        };

        _stepRepoMock
            .Setup(s => s.GetByIdAsync(5))
            .ReturnsAsync(step);

        _stepRepoMock
            .Setup(s => s.UpdateAsync(step))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.UpdateStepAsync(5, 2, "Ny", "NyOrig", "new-icon");

        // ASSERT
        Assert.True(result);
        Assert.Equal(2, step.Order);
        Assert.Equal("Ny", step.SimpleText);
        Assert.Equal("NyOrig", step.OriginalText);
        Assert.Equal("new-icon", step.IconKey);

        _stepRepoMock.Verify(s => s.GetByIdAsync(5), Times.Once);
        _stepRepoMock.Verify(s => s.UpdateAsync(step), Times.Once);
        _stepRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateStepAsync_Nar_Steg_Inte_Finns_Ska_Returnera_False_Och_Inte_Uppdatera()
    {
        // ARRANGE
        _stepRepoMock
            .Setup(s => s.GetByIdAsync(999))
            .ReturnsAsync((RoutineStep?)null);

        // ACT
        var result = await _sut.UpdateStepAsync(999, 1, "Ny", null, null);

        // ASSERT
        Assert.False(result);

        _stepRepoMock.Verify(s => s.GetByIdAsync(999), Times.Once);
        _stepRepoMock.Verify(s => s.UpdateAsync(It.IsAny<RoutineStep>()), Times.Never);
    }

    [Fact]
    public async Task DeleteStepAsync_Ska_Ta_Bort_Steg_Och_Returnera_True()
    {
        // ARRANGE
        var step = new RoutineStep { Id = 7, RoutineId = 10 };

        _stepRepoMock
            .Setup(s => s.GetByIdAsync(7))
            .ReturnsAsync(step);

        _stepRepoMock
            .Setup(s => s.DeleteAsync(7))
            .Returns(Task.CompletedTask);

        // ACT
        var result = await _sut.DeleteStepAsync(7);

        // ASSERT
        Assert.True(result);

        _stepRepoMock.Verify(s => s.GetByIdAsync(7), Times.Once);
        _stepRepoMock.Verify(s => s.DeleteAsync(7), Times.Once);
        _stepRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteStepAsync_Nar_Steg_Inte_Finns_Ska_Returnera_False()
    {
        // ARRANGE
        _stepRepoMock
            .Setup(s => s.GetByIdAsync(123))
            .ReturnsAsync((RoutineStep?)null);

        // ACT
        var result = await _sut.DeleteStepAsync(123);

        // ASSERT
        Assert.False(result);

        _stepRepoMock.Verify(s => s.GetByIdAsync(123), Times.Once);
        _stepRepoMock.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}



