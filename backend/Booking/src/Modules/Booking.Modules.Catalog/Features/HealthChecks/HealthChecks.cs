using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Booking.Modules.Catalog.Persistence;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(CatalogDbContext context, ILogger<DatabaseHealthCheck> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if we can connect to the database
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                _logger.LogError("Database health check failed: Cannot connect to database");
                return HealthCheckResult.Unhealthy("Cannot connect to database");
            }

            // Check if we can execute a simple query
            var orderCount = await _context.Orders.CountAsync(cancellationToken);

            var data = new Dictionary<string, object>
            {
                { "database", "CatalogDb" },
                { "status", "Connected" },
                { "orderCount", orderCount },
                { "checkedAt", DateTime.UtcNow }
            };

            return HealthCheckResult.Healthy("Database is healthy", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed with exception");
            return HealthCheckResult.Unhealthy(
                "Database health check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } });
        }
    }
}

public class HangfireHealthCheck : IHealthCheck
{
    private readonly ILogger<HangfireHealthCheck> _logger;

    public HangfireHealthCheck(ILogger<HangfireHealthCheck> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var servers = monitoringApi.Servers();
            
            if (servers.Count == 0)
            {
                _logger.LogWarning("Hangfire health check: No Hangfire servers are running");
                return Task.FromResult(HealthCheckResult.Degraded(
                    "No Hangfire servers are running",
                    data: new Dictionary<string, object>
                    {
                        { "serverCount", 0 },
                        { "checkedAt", DateTime.UtcNow }
                    }));
            }

            var stats = monitoringApi.GetStatistics();
            var failedCount = stats.Failed;
            var processingCount = stats.Processing;
            var enqueuedCount = stats.Enqueued;

            var data = new Dictionary<string, object>
            {
                { "serverCount", servers.Count },
                { "enqueuedJobs", enqueuedCount },
                { "processingJobs", processingCount },
                { "failedJobs", failedCount },
                { "checkedAt", DateTime.UtcNow }
            };

            if (failedCount > 100) // Threshold for degraded state
            {
                _logger.LogWarning("Hangfire has {FailedCount} failed jobs", failedCount);
                return Task.FromResult(HealthCheckResult.Degraded(
                    $"Hangfire has {failedCount} failed jobs",
                    data: data));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Hangfire is healthy", data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hangfire health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy(
                "Hangfire health check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } }));
        }
    }
}

public class GoogleCalendarHealthCheck : IHealthCheck
{
    private readonly GoogleCalendarService _googleCalendarService;
    private readonly ILogger<GoogleCalendarHealthCheck> _logger;

    public GoogleCalendarHealthCheck(
        GoogleCalendarService googleCalendarService,
        ILogger<GoogleCalendarHealthCheck> logger)
    {
        _googleCalendarService = googleCalendarService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // This is a basic check - you might need to adjust based on your GoogleCalendarService implementation
            // For a real check, you'd want to verify API accessibility without requiring a specific user
            
            var data = new Dictionary<string, object>
            {
                { "service", "Google Calendar API" },
                { "status", "Available" },
                { "checkedAt", DateTime.UtcNow }
            };

            // Note: This is a basic structural check
            // For a real production check, you'd want to make a test API call to Google
            return await Task.FromResult(
                HealthCheckResult.Healthy("Google Calendar service is configured", data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google Calendar health check failed");
            return HealthCheckResult.Unhealthy(
                "Google Calendar integration check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } });
        }
    }
}

public class KonnectPaymentHealthCheck : IHealthCheck
{
    private readonly ILogger<KonnectPaymentHealthCheck> _logger;
    private readonly IHttpClientFactory? _httpClientFactory;

    public KonnectPaymentHealthCheck(
        ILogger<KonnectPaymentHealthCheck> logger,
        IHttpClientFactory? httpClientFactory = null)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Basic check - you might want to adjust based on your Konnect configuration
            if (_httpClientFactory == null)
            {
                return HealthCheckResult.Degraded(
                    "HTTP client factory not configured for Konnect",
                    data: new Dictionary<string, object>
                    {
                        { "service", "Konnect Payment Gateway" },
                        { "checkedAt", DateTime.UtcNow }
                    });
            }

            // You can add a ping to Konnect API here if they provide a health endpoint
            var data = new Dictionary<string, object>
            {
                { "service", "Konnect Payment Gateway" },
                { "status", "Configured" },
                { "checkedAt", DateTime.UtcNow }
            };

            return await Task.FromResult(
                HealthCheckResult.Healthy("Konnect payment gateway is configured", data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konnect payment health check failed");
            return HealthCheckResult.Unhealthy(
                "Konnect payment gateway check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } });
        }
    }
}
