using Booking.Common.Contracts.Mentorships;
using Booking.Common.Email;
using Booking.Common.Endpoints;
using Booking.Modules.Mentorships.refactored.BackgroundJobs.Escrow;
using Booking.Modules.Mentorships.refactored.BackgroundJobs.Payment;
using Booking.Modules.Mentorships.refactored.BackgroundJobs.Payout;
using Booking.Modules.Mentorships.refactored.Contracts;
using Booking.Modules.Mentorships.refactored.Features.GoogleCalendar;
using Booking.Modules.Mentorships.refactored.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Mentorships.refactored;

public static class MentorshipsModule
{
    public static IServiceCollection AddMentorshipsModule(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            // .AddAWS(configuration)
            .AddDatabase(configuration)
            .AddBackgroundJobs()
            .AddResielenecPipelines(configuration)
            .AddEndpoints(AssemblyReference.Assembly);


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<AwsSesEmailService>();
        services.AddSingleton<EmailTemplateProvider>();

        services.AddScoped<GoogleCalendarService>();

        services.AddScoped<IMentorshipsModuleApi, MentorshipsModuleApi>();


        return services;
    }


    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<MentorshipsDbContext>((sp, options) => options
            .UseNpgsql(connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Mentorships);
                })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }


    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<PayoutJob>();
        services.AddScoped<EscrowJob>();
        services.AddScoped<CompleteWebhook>();
        return services;
    }

    public static IServiceCollection AddResielenecPipelines(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}