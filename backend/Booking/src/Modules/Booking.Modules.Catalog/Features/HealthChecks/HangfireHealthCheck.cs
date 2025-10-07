using Hangfire;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.HealthChecks;

public class HangfireHealthCheck(ILogger<HangfireHealthCheck> logger) : IHealthCheck
{
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
                logger.LogWarning("Hangfire health check: No Hangfire servers are running");
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
                logger.LogWarning("Hangfire has {FailedCount} failed jobs", failedCount);
                return Task.FromResult(HealthCheckResult.Degraded(
                    $"Hangfire has {failedCount} failed jobs",
                    data: data));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Hangfire is healthy", data));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Hangfire health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy(
                "Hangfire health check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } }));
        }
    }
}
