using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Booking.Modules.Mentorships.Persistence;

public class MentorshipsDbContextFactory : IDesignTimeDbContextFactory<MentorshipsDbContext>
{
    public MentorshipsDbContext CreateDbContext(string[] args)
    {
        // Build configuration - look for appsettings.json in the API project
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Api", "Booking.Api");
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("Database");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string not found in configuration.");
        }

        // Configure DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<MentorshipsDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schemas.Mentorships);
            })
            .UseSnakeCaseNamingConvention();

        return new MentorshipsDbContext(optionsBuilder.Options);
    }
}