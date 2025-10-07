namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Represents the priority level of a notification
/// </summary>
public enum NotificationPriority
{
    /// <summary>
    /// Low priority - can be delayed
    /// </summary>
    Low = 1,

    /// <summary>
    /// Normal priority - default
    /// </summary>
    Normal = 2,

    /// <summary>
    /// High priority - should be processed quickly
    /// </summary>
    High = 3,

    /// <summary>
    /// Critical priority - process immediately
    /// </summary>
    Critical = 4
}
