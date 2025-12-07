using VardForAlla.Application.Interfaces;
using VardForAlla.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using VardForAlla.Infrastructure.Data;
using VardForAlla.Application.Services;
using VardForAlla.Application.Factories;
using VardForAlla.Domain.Factories;
using VardForAlla.Api.Middleware;
using VardForAlla.Infrastructure.Seeding;


namespace VardForAlla.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IRoutineRepository, RoutineRepository>();
            builder.Services.AddScoped<IRoutineStepRepository, RoutineStepRepository>();
            builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
            builder.Services.AddScoped<IStepTranslationRepository, StepTranslationRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<RoutineFactory, SimpleRoutineFactory>();
            builder.Services.AddScoped<IRoutineService, RoutineService>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<IStepTranslationService, StepTranslationService>();
            builder.Services.AddScoped<IRoutineStepService, RoutineStepService>();
            builder.Services.AddScoped<ITagService, TagService>();

            builder.Services.AddDbContext<VardForAllaDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<VardForAllaDbContext>();
                DataSeeder.Seed(dbContext);
            }

            app.UseGlobalExceptionHandling();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
