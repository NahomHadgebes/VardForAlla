using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Seeding;

public static class DataSeeder
{
    public static void Seed(VardForAllaDbContext context)
    {
        SeedLanguages(context);
        SeedRoutines(context);
    }

    private static void SeedLanguages(VardForAllaDbContext context)
    {
        var languages = new List<Language>
        {
            new Language { Code = "SV", Name = "Svenska" },
            new Language { Code = "ENG", Name = "English" },
            new Language { Code = "ARB", Name = "Arabiska" },
            new Language { Code = "FI", Name = "Finska" },
            new Language { Code = "SOM", Name = "Somaliska" },
            new Language { Code = "SPA", Name = "Spanska" }
        };

        context.Languages.AddRange(languages);
        context.SaveChanges();
    }

    private static void SeedRoutines(VardForAllaDbContext context)
    {
        var routine = new Routine
        {
            Title = "Basal hygien",
            Category = "Hygien",
            SimpleDescription = "Grundläggande hygienrutin för vårdpersonal.",
            OriginalDescription = "Detta är en förenklad rutin för basal hygien.",
            IsActive = true,
            Steps = new List<RoutineStep>
            {
                new RoutineStep
                {
                    Order = 1,
                    SimpleText = "Tvätta händerna noggrant med tvål och vatten.",
                    OriginalText = "Tvätta händerna i minst 30 sekunder.",
                    IconKey = "handwash"
                },
                new RoutineStep
                {
                    Order = 2,
                    SimpleText = "Torka händerna med engångshandduk.",
                    OriginalText = "Använd alltid ren engångshandduk.",
                    IconKey = "towel"
                },
                new RoutineStep
                {
                    Order = 3,
                    SimpleText = "Desinficera händerna med handsprit.",
                    OriginalText = "Använd tillräcklig mängd handsprit.",
                    IconKey = "sanitizer"
                }
            }
        };

        context.Routines.Add(routine);
        context.SaveChanges();
    }
}

