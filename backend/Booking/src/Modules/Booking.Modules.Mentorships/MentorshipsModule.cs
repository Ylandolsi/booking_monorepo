using Booking.Common.Contracts.Mentorships;
using Booking.Common.Domain.Events;
using Booking.Common.Email;
using Booking.Common.Endpoints;
using Booking.Common.SlugGenerator;
using Booking.Common.Uploads;
using Booking.Modules.Mentorships.Contracts;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Booking.Modules.Mentorships.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Booking.Modules.Mentorships;

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
        services.AddTransient<DomainEventsDispatcher>();
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
            // TODO : add this interceptor 
            //.AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }


    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddResielenecPipelines(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}