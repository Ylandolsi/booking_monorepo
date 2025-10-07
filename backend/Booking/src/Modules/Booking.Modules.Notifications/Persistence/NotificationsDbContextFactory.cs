using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Booking.Modules.Notifications.Persistence;

public class NotificationsDbContextFactory : IDesignTimeDbContextFactory<NotificationsDbContext>
{
    public NotificationsDbContext CreateDbContext(string[] args)
    {
        // Build configuration - look for appsettings.json in the API project
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Api", "Booking.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Database connection string not found in configuration.");

        // Configure DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<NotificationsDbContext>();
        optionsBuilder.UseNpgsql(connectionString,
                npgsqlOptions => { npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schemas.Notifications); })
            .UseSnakeCaseNamingConvention();

        return new NotificationsDbContext(optionsBuilder.Options);
    }
}
