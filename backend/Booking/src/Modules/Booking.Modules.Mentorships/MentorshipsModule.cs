using Booking.Common.Contracts.Mentorships;
using Booking.Common.Domain.Events;
using Booking.Common.Email;
using Booking.Common.Endpoints;
using Booking.Modules.Mentorships.BackgroundJobs.Escrow;
using Booking.Modules.Mentorships.BackgroundJobs.Payout;
using Booking.Modules.Mentorships.Contracts;
using Booking.Modules.Mentorships.Features.GoogleCalendar;
using Booking.Modules.Mentorships.Persistence;
using Booking.Modules.Users.BackgroundJobs;
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
        services.AddScoped<ProcessOutboxMessagesJobMentorShipModule>();
        services.AddScoped<ProcessOutboxMessagesJobMentorShipModule>();
        services.AddScoped<PayoutJob>();
        services.AddScoped<EscrowJob>();
        return services;
    }

    public static IServiceCollection AddResielenecPipelines(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddResiliencePipeline(ProcessOutboxMessagesJobMentorShipModule.OutboxProcessorPipelineKey,
            builder =>
            {
                // polly : add resilience for executing outbox messages
                builder.AddRetry(new RetryStrategyOptions
                {
                    Delay = TimeSpan.FromSeconds(1),
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true, // Helps prevent "thundering herd" issues.
                    // This will add a random delay to each retry attempt, which can help distribute load more evenly.
                    // to avoid overwhelming the system with retries at the same time.

                    OnRetry = args =>
                    {
                        Console.WriteLine(
                            $"Retrying operation due to: {args.Outcome.Exception?.Message}. Attempt #{args.AttemptNumber}");
                        return ValueTask.CompletedTask;
                    }
                });

                builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                    FailureRatio = 0.5, // Break the circuit if 50% of requests fail...
                    MinimumThroughput =
                        5, // at least 5 request must be made before ( 1 request = complete process with retry )
                    SamplingDuration = TimeSpan.FromSeconds(60), // within a 60-second window.
                    BreakDuration = TimeSpan.FromSeconds(30),
                    OnOpened = args =>
                    {
                        Console.WriteLine(
                            $"Circuit breaker opened for {args.BreakDuration.TotalSeconds}s due to: {args.Outcome.Exception?.Message}");
                        return ValueTask.CompletedTask;
                    },
                    OnClosed = _ =>
                    {
                        Console.WriteLine("Circuit breaker closed. Operations have resumed.");
                        return ValueTask.CompletedTask;
                    }
                });
            });

        return services;
    }
}