using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using VardForAlla.Application.Services;
using VardForAlla.Infrastructure.Data;
using VardForAlla.Infrastructure.Repositories;
using VardForAlla.Domain.Entities;
using Xunit;

namespace VardForAlla.Tests.Services;

public class StepTranslationServiceTests
{
    private VardForAllaDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<VardForAllaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new VardForAllaDbContext(options);
    }

    [Fact]
    public async Task AddTranslationAsync_Ska_Skapa_Oversattning_For_Steg_Och_Sprak()
    {
        // ARRANGE
        var context = CreateInMemoryContext();

        // Skapa språk
        var language = new Language
        {
            Code = "sv",
            Name = "Svenska"
        };
        context.Languages.Add(language);

        // Skapa rutin + steg
        var routine = new Routine
        {
            Title = "Test rutin",
            Category = "Test",
            IsActive = true
        };
        var step = new RoutineStep
        {
            Order = 1,
            SimpleText = "Tvätta händerna",
            Routine = routine
        };

        context.Routines.Add(routine);
        context.RoutineSteps.Add(step);

        await context.SaveChangesAsync();

        // Riktiga repositories
        var translationRepo = new StepTranslationRepository(context);
        var languageRepo = new LanguageRepository(context);
        var stepRepo = new RoutineStepRepository(context);

        var logger = NullLogger<StepTranslationService>.Instance;
        var service = new StepTranslationService(
            translationRepo,
            languageRepo,
            stepRepo,
            logger);

        // ACT
        var translation = await service.AddTranslationAsync(step.Id, "sv", "Tvätta händerna noggrant");
        var translationsForStep = await translationRepo.GetTranslationsForStepAsync(step.Id);

        // ASSERT
        Assert.NotNull(translation);
        Assert.Equal(step.Id, translation.RoutineStepId);
        Assert.Equal("Tvätta händerna noggrant", translation.Text);
        Assert.Single(translationsForStep);
    }
}

