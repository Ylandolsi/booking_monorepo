using Booking.Api.Middlewares;
using Booking.Modules.Users.Domain.Entities;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;

namespace Booking.Api.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection UseHangFire(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireConnectionString = configuration.GetConnectionString("Database");

        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer() // Store only namespace and assembly name of the job 
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(hangfireConnectionString), new PostgreSqlStorageOptions
            {
                SchemaName = "hangfire",
                DistributedLockTimeout = TimeSpan.FromMinutes(2)
            })
            .UseConsole()); // logs to the Hangfire dashboard

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = Environment.ProcessorCount; // = cpu cores
            options.Queues = ["default", "critical"];
        });


        return services;
    }

    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app)
    {

        var options = new DashboardOptions
        {
            AsyncAuthorization = [new HangfireDashboardAuthorizationFilter(app)],
        };
        app.UseHangfireDashboard("/hangfire", options);

        return app;
    }
}