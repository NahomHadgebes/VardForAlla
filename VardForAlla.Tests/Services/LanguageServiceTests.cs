using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VardForAlla.Application.Services;
using VardForAlla.Infrastructure.Data;
using VardForAlla.Infrastructure.Repositories;
using Xunit;

namespace VardForAlla.Tests.Services;

public class LanguageServiceTests
{
    private VardForAllaDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<VardForAllaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new VardForAllaDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_Ska_Skapa_Nytt_Sprak()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new LanguageRepository(context);
        var logger = NullLogger<LanguageService>.Instance;
        var service = new LanguageService(repo, logger);

        // ACT
        var language = await service.CreateAsync("sv", "Svenska");
        var all = await service.GetAllAsync();

        // ASSERT
        Assert.NotNull(language);
        Assert.Equal("sv", language.Code);
        Assert.Single(all);
    }

    [Fact]
    public async Task CreateAsync_Ska_Kasta_Nar_Sprak_Kod_Redas_Finns()
    {
        // ARRANGE
        var context = CreateInMemoryContext();
        var repo = new LanguageRepository(context);
        var logger = NullLogger<LanguageService>.Instance;
        var service = new LanguageService(repo, logger);

        await service.CreateAsync("sv", "Svenska");

        // ACT & ASSERT
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.CreateAsync("sv", "Dublett");
        });
    }
}

