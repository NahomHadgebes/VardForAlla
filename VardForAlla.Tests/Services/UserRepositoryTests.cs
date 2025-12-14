using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;
using VardForAlla.Infrastructure.Repositories;
using Xunit;

namespace VardForAlla.Tests.Infrastructure;

public class UserRepositoryTests
{
    private VardForAllaDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<VardForAllaDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        return new VardForAllaDbContext(options);
    }

    [Fact]
    public async Task GetByEmailAsync_Nar_Anvandare_Finns_Ska_Returnera_Anvandare()
    {
        // ARRANGE
        using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Email = "test@test.se",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "hash"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // ACT
        var result = await repository.GetByEmailAsync("test@test.se");

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("test@test.se", result.Email);
        Assert.Equal("Test", result.FirstName);
    }

    [Fact]
    public async Task GetByEmailAsync_Nar_Anvandare_Inte_Finns_Ska_Returnera_Null()
    {
        // ARRANGE
        using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);

        // ACT
        var result = await repository.GetByEmailAsync("nonexistent@test.se");

        // ASSERT
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_Ska_Lagga_Till_Anvandare()
    {
        // ARRANGE
        using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Email = "new@test.se",
            FirstName = "New",
            LastName = "User",
            PasswordHash = "hash"
        };

        // ACT
        await repository.AddAsync(user);

        // ASSERT
        var saved = await context.Users.FirstOrDefaultAsync(u => u.Email == "new@test.se");
        Assert.NotNull(saved);
        Assert.Equal("New", saved.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_Ska_Markera_Anvandare_Som_Inaktiv()
    {
        // ARRANGE
        using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);

        var user = new User
        {
            Email = "delete@test.se",
            FirstName = "Delete",
            LastName = "Me",
            PasswordHash = "hash",
            IsActive = true
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // ACT
        await repository.DeleteAsync(user.Id);

        // ASSERT
        var updated = await context.Users.FindAsync(user.Id);
        Assert.NotNull(updated);
        Assert.False(updated.IsActive);
    }
}
