using Booking.Modules.Catalog.Persistence;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.HealthChecks;

public class DatabaseHealthCheck(CatalogDbContext context, ILogger<DatabaseHealthCheck> logger) : IHealthCheck
{
    private readonly CatalogDbContext _context = context;

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
                logger.LogError("Database health check failed: Cannot connect to database");
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
            logger.LogError(ex, "Database health check failed with exception");
            return HealthCheckResult.Unhealthy(
                "Database health check failed",
                ex,
                new Dictionary<string, object> { { "error", ex.Message } });
        }
    }
}
