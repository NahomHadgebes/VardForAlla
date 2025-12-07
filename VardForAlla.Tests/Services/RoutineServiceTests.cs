using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VardForAlla.Application.Factories;
using VardForAlla.Application.Services;
using VardForAlla.Domain.Factories;
using VardForAlla.Infrastructure.Data;
using VardForAlla.Infrastructure.Repositories;
using Xunit;

namespace VardForAlla.Tests.Services;

public class RoutineServiceTests
{
    private VardForAllaDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<VardForAllaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new VardForAllaDbContext(options);
    }

    [Fact]
    public async Task CreateRoutineAsync_Ska_Skapa_Rutin_Med_Steg()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new RoutineRepository(context);
        var factory = new SimpleRoutineFactory();
        var logger = NullLogger<RoutineService>.Instance;

        var service = new RoutineService(repo, factory, logger);

        var steps = new List<(int order, string simpleText, string? originalText, string? iconKey)>
        {
            (1, "Steg 1", null, null),
            (2, "Steg 2", null, null)
        };

        // ACT
        var routine = await service.CreateRoutineAsync(
            "Test rutin",
            "Test kategori",
            "Enkel beskrivning",
            "Original beskrivning",
            steps);

        var all = await service.GetAllAsync();

        // ASSERT
        Assert.NotNull(routine);
        Assert.Equal("Test rutin", routine.Title);
        Assert.Equal(2, routine.Steps.Count);
        Assert.Single(all);
    }

    [Fact]
    public async Task UpdateRoutineAsync_Ska_Returnera_False_Nar_Rutin_Ej_Finns()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new RoutineRepository(context);
        var factory = new SimpleRoutineFactory();
        var logger = NullLogger<RoutineService>.Instance;

        var service = new RoutineService(repo, factory, logger);

        // ACT
        var result = await service.UpdateRoutineAsync(
            id: 999,
            title: "Ny titel",
            category: "Ny kategori",
            simpleDescription: null,
            originalDescription: null);

        // ASSERT
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteRoutineAsync_Ska_Satta_IsActive_False()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new RoutineRepository(context);
        var factory = new SimpleRoutineFactory();
        var logger = NullLogger<RoutineService>.Instance;
        var service = new RoutineService(repo, factory, logger);

        var steps = new List<(int order, string simpleText, string? originalText, string? iconKey)>
        {
            (1, "Steg 1", null, null)
        };

        var routine = await service.CreateRoutineAsync(
            "Rutin att ta bort",
            "Kategori",
            null,
            null,
            steps);

        var id = routine.Id;

        // ACT
        var deleted = await service.DeleteRoutineAsync(id);

        // ASSERT
        Assert.True(deleted);
        Assert.False(routine.IsActive);
    }
}

