namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Result of a notification send operation
/// </summary>
public sealed record SendNotificationResult
{
    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public required bool IsSuccess { get; init; }

    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public Guid? NotificationId { get; init; }

    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Indicates if the notification was queued (true) or sent immediately (false)
    /// </summary>
    public bool IsQueued { get; init; }

    /// <summary>
    /// When the notification was sent (for immediate sends)
    /// </summary>
    public DateTime? SentAt { get; init; }

    /// <summary>
    /// Creates a successful result for a queued notification
    /// </summary>
    public static SendNotificationResult Queued(Guid notificationId) => new()
    {
        IsSuccess = true,
        NotificationId = notificationId,
        IsQueued = true
    };

    /// <summary>
    /// Creates a successful result for an immediately sent notification
    /// </summary>
    public static SendNotificationResult Sent(Guid notificationId, DateTime sentAt) => new()
    {
        IsSuccess = true,
        NotificationId = notificationId,
        IsQueued = false,
        SentAt = sentAt
    };

    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static SendNotificationResult Failed(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage,
        IsQueued = false
    };
}
