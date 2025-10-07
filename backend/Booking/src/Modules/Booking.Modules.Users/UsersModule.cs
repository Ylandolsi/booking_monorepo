using Booking.Common.Contracts.Users;
using Booking.Common.Email;
using Booking.Common.Endpoints;
using Booking.Common.SlugGenerator;
using Booking.Common.Uploads;
using Booking.Modules.Users.BackgroundJobs.Cleanup;
using Booking.Modules.Users.Contracts;
using Booking.Modules.Users.Features.Authentication;
using Booking.Modules.Users.Features.Authentication.Google;
using Booking.Modules.Users.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddServices()
            .ExposeApiForModules()
            .AddDatabase(configuration)
            .AddBackgroundJobs()
            .AddResielenecPipelines(configuration)
            .AddEndpoints(AssemblyReference.Assembly);
    }

    private static IServiceCollection ExposeApiForModules(this IServiceCollection service)
    {
        service.AddScoped<IUsersModuleApi, UsersModuleApi>();
        return service;
    }


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<RoleService>();
        services.AddScoped<AwsSesEmailService>();
        services.AddSingleton<EmailTemplateProvider>();
        // AWS UPLOAD
        services.AddScoped<S3ImageProcessingService>();

        services.AddScoped<SlugGenerator>();

        services.AddScoped<TokenHelper>();

        services.AddScoped<GoogleTokenService>();


        return services;
    }


    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<UsersDbContext>((sp, options) => options
            .UseNpgsql(connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users);
                })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }


    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<TokenCleanupJob>();

        return services;
    }

    public static void ConfigureBackgroundJobs()
    {
        RecurringJob.AddOrUpdate<TokenCleanupJob>(
            "token-cleanup-job",
            job => job.CleanUpAsync(null), //  // todo : review this null context 
            Cron.Daily(1, 0), // Runs daily at 2:00 AM GMT+1 
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc // timezone = GMT (UTC)
            });
    }

    public static IServiceCollection AddResielenecPipelines(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}