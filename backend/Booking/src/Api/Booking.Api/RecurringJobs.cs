using Booking.Modules.Catalog;
using Booking.Modules.Notifications;
using Booking.Modules.Users;

namespace Booking.Api;

public static class RecurringJobs
{
    public static void AddRecurringJobs()
    {
        CatalogModule.ConfigureBackgroundJobs();
        UsersModule.ConfigureBackgroundJobs();
        NotificationsModule.ConfigureBackgroundJobs();
    }
}