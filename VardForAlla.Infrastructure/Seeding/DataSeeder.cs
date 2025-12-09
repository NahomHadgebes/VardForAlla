using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Seeding;

public static class DataSeeder
{
    public static void Seed(VardForAllaDbContext context)
    {
        SeedLanguages(context);
    }

    private static void SeedLanguages(VardForAllaDbContext context)
    {
        if (!context.Languages.Any())
        {
            context.Languages.AddRange(
                new Language { Code = "SWE", Name = "Svenska" },
                new Language { Code = "ENG", Name = "Engelska" },
                new Language { Code = "ARB", Name = "Arabiska" },
                new Language { Code = "SOM", Name = "Somaliska" },
                new Language { Code = "POL", Name = "Polska" },
                new Language { Code = "FIN", Name = "Finska" }
    );
            context.SaveChanges();
        }
    }
}

