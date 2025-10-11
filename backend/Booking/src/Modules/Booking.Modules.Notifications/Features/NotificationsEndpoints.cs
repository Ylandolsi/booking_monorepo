namespace Booking.Modules.Notifications.Features;

public static class NotificationsEndpoints
{
    public static class Admin
    {
        public const string get = "api/notifications/admin";
        public const string markAllRead = "api/notifications/admin";
        public const string markSignleRead = $"api/notifications/admin/{{notificationId}}";
    }
}