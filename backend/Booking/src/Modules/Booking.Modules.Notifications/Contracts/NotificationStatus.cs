namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Represents the current status of a notification
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    /// Notification is queued and waiting to be sent
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Notification is currently being processed
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Notification was sent successfully
    /// </summary>
    Sent = 3,

    /// <summary>
    /// Notification failed to send after all retry attempts
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Notification was cancelled before sending
    /// </summary>
    Cancelled = 5
}
