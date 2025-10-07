using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.HealthChecks;

public class KonnectPaymentHealthCheck(
    ILogger<KonnectPaymentHealthCheck> logger,
    IHttpClientFactory? httpClientFactory = null) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Basic check - you might want to adjust based on your Konnect configuration
            if (httpClientFactory == null)
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
            logger.LogError(ex, "Konnect payment health check failed");
            return HealthCheckResult.Unhealthy(
                "Konnect payment gateway check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } });
        }
    }
}
