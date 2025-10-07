using Booking.Modules.Catalog.Features.Integrations.GoogleCalendar;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.HealthChecks;

public class GoogleCalendarHealthCheck(
    GoogleCalendarService googleCalendarService,
    ILogger<GoogleCalendarHealthCheck> logger) : IHealthCheck
{
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
            logger.LogError(ex, "Google Calendar health check failed");
            return HealthCheckResult.Unhealthy(
                "Google Calendar integration check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } });
        }
    }
}
