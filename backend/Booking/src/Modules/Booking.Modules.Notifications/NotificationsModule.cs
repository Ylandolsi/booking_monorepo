using Booking.Common.Messaging;
using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.BackgroundJobs.Cleanup;
using Booking.Modules.Notifications.BackgroundJobs.OutboxProcessor;
using Booking.Modules.Notifications.Features.Outbox.Enqueue;
using Booking.Modules.Notifications.Features.Outbox.Process;
using Booking.Modules.Notifications.Infrastructure.Adapters.AwsSes;
using Booking.Modules.Notifications.Infrastructure.TemplateEngine;
using Booking.Modules.Notifications.Persistence;
using Booking.Modules.Notifications.Services;
using FluentValidation;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Modules.Notifications;

public static class NotificationsModule
{
    public static IServiceCollection AddNotificationsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDatabase(configuration)
            .AddOptions(configuration)
            .AddServices()
            .AddAdapters()
            .AddBackgroundJobs();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure AWS SES options
        services.Configure<AwsSesOptions>(configuration.GetSection(AwsSesOptions.SectionKey));

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<NotificationsDbContext>((sp, options) => options
            .UseNpgsql(connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        Schemas.Notifications);
                })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Register template engine
        services.AddSingleton<ITemplateEngine, EmbeddedTemplateEngine>();

        // Register high-level notification service
        services.AddScoped<INotificationService, NotificationService>();

        /*// Register command handlers
        services.AddScoped<ICommandHandler<EnqueueNotificationCommand, Guid>,
            EnqueueNotificationCommandHandler>();
        services.AddScoped<ICommandHandler<ProcessOutboxCommand, ProcessOutboxResult>,
            ProcessOutboxCommandHandler>();*/

        /*// Register validators
        services.AddScoped<IValidator<EnqueueNotificationCommand>, EnqueueNotificationCommandValidator>();*/

        return services;
    }

    private static IServiceCollection AddAdapters(this IServiceCollection services)
    {
        // Register AWS SES email sender
        services.AddScoped<AwsSesEmailSender>();

        // Register IEmailSender with AWS SES as the default implementation
        services.AddScoped<IEmailSender, AwsSesEmailSender>();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        // Register background job classes
        services.AddScoped<OutboxProcessorJob>();
        services.AddScoped<NotificationCleanupJob>();

        return services;
    }

    /// <summary>
    /// Configures Hangfire recurring jobs for notification processing
    /// Call this method after the application has started (e.g., in Program.cs after app.Run())
    /// </summary>
    public static void ConfigureBackgroundJobs()
    {
        // Process outbox every 2 minutes
        RecurringJob.AddOrUpdate<OutboxProcessorJob>(
            "process-notification-outbox",
            job => job.ProcessAsync(default),
            "*/2 * * * *"); // Every 2 minutes

        // Cleanup old notifications daily at 2 AM
        RecurringJob.AddOrUpdate<NotificationCleanupJob>(
            "cleanup-old-notifications",
            job => job.CleanupAsync(30, default), // 30 days retention
            "0 2 * * *"); // Daily at 2:00 AM
    }
}