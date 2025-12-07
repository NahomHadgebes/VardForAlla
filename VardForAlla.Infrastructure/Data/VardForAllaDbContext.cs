using Microsoft.EntityFrameworkCore;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Infrastructure.Data;

public class VardForAllaDbContext : DbContext
{
    public VardForAllaDbContext(DbContextOptions<VardForAllaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Routine> Routines { get; set; } = null!;
    public DbSet<RoutineStep> RoutineSteps { get; set; } = null!;
    public DbSet<Language> Languages { get; set; } = null!;
    public DbSet<StepTranslation> StepTranslations { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Routine>()
            .HasMany(r => r.Steps)
            .WithOne(s => s.Routine)
            .HasForeignKey(s => s.RoutineId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoutineStep>()
            .HasMany(s => s.Translations)
            .WithOne(t => t.RoutineStep)
            .HasForeignKey(t => t.RoutineStepId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Language>()
            .HasMany(l => l.Translations)
            .WithOne(t => t.Language)
            .HasForeignKey(t => t.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Routine>()
            .HasMany(r => r.Tags)
            .WithMany(t => t.Routines)
            .UsingEntity(j => j.ToTable("RoutineTags"));
    }
}

