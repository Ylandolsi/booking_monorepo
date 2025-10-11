using System.Threading.Channels;
using Booking.Common.Endpoints;
using Booking.Modules.Catalog.BackgroundJobs.Escrow;
using Booking.Modules.Catalog.BackgroundJobs.Payout;
using Booking.Modules.Catalog.BackgroundJobs.Statistics;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Features.Orders.PaymentWebhook;
using Booking.Modules.Catalog.Features.Payout.Admin.Approve.Webhook;
using Booking.Modules.Catalog.Features.Stores;
using Booking.Modules.Catalog.Features.Stores.StoreVisit;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
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
            .AddChannels()
            .AddBackgroundJobs()
            .AddEndpoints(AssemblyReference.Assembly);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

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

    private static IServiceCollection AddChannels(this IServiceCollection services)
    {
        services.AddSingleton(_ => Channel.CreateBounded<StoreVisit>(new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        }));

        services.AddSingleton<StoreVisitChannel>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<GoogleCalendarService>();
        services.AddScoped<StoreService>();

        // Add other services here

        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<PayoutJob>();
        services.AddScoped<EscrowJob>();
        services.AddScoped<PayoutWebhookJob>();
        services.AddScoped<PaymentWebhookJob>();
        services.AddScoped<StoreVisitBatchJob>();
        services.AddScoped<StoreStatsAggregatorJob>();
        return services;
    }

    public static void ConfigureBackgroundJobs()
    {
        RecurringJob.AddOrUpdate<StoreVisitBatchJob>(
            "store-batch-visits-job",
            job => job.ExecuteAsync(null), // todo : review this null context 
            "*/3 * * * *"); // run every 3 minutes

        RecurringJob.AddOrUpdate<StoreStatsAggregatorJob>(
            "store-stats-aggregator-job",
            job => job.ExecuteAsync(null),
            "*/15 * * * *"); // Run every 15 minutes
    }
}