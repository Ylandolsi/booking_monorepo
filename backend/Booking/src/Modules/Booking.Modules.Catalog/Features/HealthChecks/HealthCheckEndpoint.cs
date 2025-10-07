using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Booking.Modules.Catalog.Features.HealthChecks;

public static class HealthCheckEndpoint
{
    public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {

        // ### `/health` (Basic Health Check)
        //         -**Purpose * *: Performs all registered health checks(e.g., database connectivity, external services).
        //         - **Predicate * *: None specified(defaults to all checks).
        //         -**Response * *: Detailed JSON including overall status, individual check results(name, status, description, duration, data, exception), total duration, and timestamp.
        //         - **Use Case * *: Comprehensive health assessment to verify the application and its dependencies are functioning.


        // Basic health check endpoint
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        duration = entry.Value.Duration.TotalMilliseconds,
                        data = entry.Value.Data,
                        exception = entry.Value.Exception?.Message
                    }),
                    totalDuration = report.TotalDuration.TotalMilliseconds,
                    timestamp = DateTime.UtcNow
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
            }
        });

        // ### `/health/live` (Liveness Check)
        //         -**Purpose * *: Simple liveness probe to confirm the service is running(no actual health checks executed).
        //         -**Predicate * *: `_ => false` (explicitly skips all checks).
        //         -**Response * *: Minimal JSON with hardcoded "Healthy" status and timestamp.
        //         - **Use Case * *: Lightweight ping for container orchestrators (e.g., Kubernetes) to detect if the process is alive, without evaluating dependencies.

        // Liveness check - simple check to see if the service is running
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // No health checks, just returns if the service is up
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = "Healthy",
                    timestamp = DateTime.UtcNow
                }));
            }
        });

        // Readiness check - checks if the service is ready to accept requests
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description
                    }),
                    timestamp = DateTime.UtcNow
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
            }
        });
    }
}
