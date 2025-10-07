using Booking.Modules.Catalog.Persistence;
using Booking.Modules.Notifications.Persistence;
using Booking.Modules.Users.Persistence;
 
using Microsoft.EntityFrameworkCore;

namespace Booking.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        ApplyMigration<UsersDbContext>(scope);
        ApplyMigration<CatalogDbContext>(scope);
        ApplyMigration<NotificationsDbContext>(scope);

        //ApplyMigration<SessionsDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}