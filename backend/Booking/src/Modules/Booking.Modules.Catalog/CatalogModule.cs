using Booking.Common.Contracts.Mentorships;
using Booking.Common.Email;
using Booking.Common.Endpoints;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Features.Stores;
using Booking.Modules.Catalog.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDatabase(configuration)
            .AddServices()
            .AddEndpoints(AssemblyReference.Assembly);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<CatalogDbContext>((sp, options) => options
            .UseNpgsql(connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Catalog);
                })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<AwsSesEmailService>();
        services.AddSingleton<EmailTemplateProvider>();
        services.AddScoped<GoogleCalendarService>();
        services.AddScoped<StoreService>();

        // Add other services here

        return services;
    }
}