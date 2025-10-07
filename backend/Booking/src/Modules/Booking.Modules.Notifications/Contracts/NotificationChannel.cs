namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Represents the delivery channel for a notification
/// </summary>
public enum NotificationChannel
{
    /// <summary>
    /// Email notification
    /// </summary>
    Email = 1,

    /// <summary>
    /// SMS notification (future)
    /// </summary>
    Sms = 2,

    /// <summary>
    /// Push notification (future)
    /// </summary>
    Push = 3,

    /// <summary>
    /// In-app notification
    /// </summary>
    InApp = 4
}
