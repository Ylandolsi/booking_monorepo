using Booking.Common.Contracts.Users;
using Booking.Common.Domain.Events;
using Booking.Common.Email;
using Booking.Common.Endpoints;
using Booking.Common.SlugGenerator;
using Booking.Common.Uploads;
using Booking.Modules.Users.BackgroundJobs;
using Booking.Modules.Users.BackgroundJobs.Cleanup;
using Booking.Modules.Users.BackgroundJobs.SendingPasswordResetToken;
using Booking.Modules.Users.BackgroundJobs.SendingVerificationEmail;
using Booking.Modules.Users.Contracts;
using Booking.Modules.Users.Features.Authentication;
using Booking.Modules.Users.Features.Authentication.Google;
using Booking.Modules.Users.Features.Authentication.Verification;
using Booking.Modules.Users.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Booking.Modules.Users;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .ExposeApiForModules()
            // .AddAWS(configuration)
            .AddDatabase(configuration)
            .AddBackgroundJobs()
            .AddResielenecPipelines(configuration)
            .AddEndpoints(AssemblyReference.Assembly);

    private static IServiceCollection ExposeApiForModules(this IServiceCollection service)
    {
        service.AddScoped<IUsersModuleApi, UsersModuleApi>();
        return service; 
    }


    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<DomainEventsDispatcher>();
        services.AddScoped<AwsSesEmailService>();
        services.AddSingleton<EmailTemplateProvider>();
        // AWS UPLOAD
        services.AddScoped<S3ImageProcessingService>();

        services.AddScoped<SlugGenerator>();

        services.AddScoped<EmailVerificationSender>();
        services.AddScoped<TokenHelper>();

        services.AddScoped<GoogleTokenService>();


        return services;
    }


    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<UsersDbContext>((sp, options)=> options
            .UseNpgsql(connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users);
                })
            // TODO : add this interceptor 
            //.AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
            .UseSnakeCaseNamingConvention());
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }





    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<VerificationEmailForRegistrationJob>();
        services.AddScoped<ProcessOutboxMessagesJob>();
        services.AddScoped<OutboxCleanupJob>();
        services.AddScoped<TokenCleanupJob>();
        services.AddScoped<SendingPasswordResetToken>();

        return services;
    }

    public static IServiceCollection AddResielenecPipelines(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddResiliencePipeline(ProcessOutboxMessagesJob.OutboxProcessorPipelineKey,
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