using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;
using VardForAlla.Infrastructure.Data;

namespace VardForAlla.Infrastructure.Seeding;

public static class DataSeeder
{
    public static void Seed(VardForAllaDbContext context, IPasswordHasher passwordHasher)
    {
        SeedRoles(context);
        SeedAdminUser(context, passwordHasher);
        SeedLanguages(context);
    }

    private static void SeedRoles(VardForAllaDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role
                {
                    Name = "Admin",
                    Description = "Administratör med full behörighet"
                },
                new Role
                {
                    Name = "User",
                    Description = "Vanlig användare med begränsad behörighet"
                }
            );
            context.SaveChanges();
        }
    }

    private static void SeedAdminUser(VardForAllaDbContext context, IPasswordHasher passwordHasher)
    {
        if (!context.Users.Any())
        {
            var adminRole = context.Roles.First(r => r.Name == "Admin");

            var admin = new User
            {
                Email = "admin@vardforalla.se",
                PasswordHash = passwordHasher.HashPassword("Admin123!"),
                FirstName = "System",
                LastName = "Administrator",
                IsEmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            context.SaveChanges();

            context.UserRoles.Add(new UserRole
            {
                UserId = admin.Id,
                RoleId = adminRole.Id,
                AssignedAt = DateTime.UtcNow
            });

            context.SaveChanges();
        }
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

