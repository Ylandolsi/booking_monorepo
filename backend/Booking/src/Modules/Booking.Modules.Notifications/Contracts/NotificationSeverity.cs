namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Represents the severity level of a notification.
/// Used to prioritize notifications and determine display styling.
/// </summary>
public enum NotificationSeverity
{
    /// <summary>
    /// Informational messages that provide general updates or confirmations
    /// </summary>
    Info = 1,

    /// <summary>
    /// Success notifications indicating completed actions or positive outcomes
    /// </summary>
    Success = 2,

    /// <summary>
    /// Warning notifications that require attention but are not critical
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Error notifications indicating failures or problems that need immediate attention
    /// </summary>
    Error = 4,

    /// <summary>
    /// Critical notifications requiring urgent action (system failures, security alerts)
    /// </summary>
    Critical = 5
}