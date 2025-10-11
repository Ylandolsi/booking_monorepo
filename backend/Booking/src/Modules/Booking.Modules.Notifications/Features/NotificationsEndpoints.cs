namespace Booking.Modules.Notifications.Features;

public static class NotificationsEndpoints
{
    public static class Admin
    {
        public const string get = "api/notifications/admin";
        public const string getUnreadCount = "api/notifications/admin/count";
        public const string markAllRead = "api/notifications/admin";
        public const string markSignleRead = $"api/notifications/admin/{{notificationId}}";
    }
}