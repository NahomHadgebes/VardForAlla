using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VardForAlla.Application.Services;
using VardForAlla.Infrastructure.Data;
using VardForAlla.Infrastructure.Repositories;
using Xunit;

namespace VardForAlla.Tests.Services;

public class TagServiceTests
{
    private VardForAllaDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<VardForAllaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new VardForAllaDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_Ska_Skapa_Ny_Tagg()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new TagRepository(context);
        var logger = NullLogger<TagService>.Instance;
        var service = new TagService(repo, logger);

        // ACT
        var tag = await service.CreateAsync("Hygien");
        var all = await service.GetAllAsync();

        // ASSERT
        Assert.NotNull(tag);
        Assert.Equal("Hygien", tag.Name);
        Assert.Single(all);
    }

    [Fact]
    public async Task DeleteAsync_Ska_Returnera_False_Nar_Tagg_Ej_Finns()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new TagRepository(context);
        var logger = NullLogger<TagService>.Instance;
        var service = new TagService(repo, logger);

        // ACT
        var result = await service.DeleteAsync(999);

        // ASSERT
        Assert.False(result);
    }
}

