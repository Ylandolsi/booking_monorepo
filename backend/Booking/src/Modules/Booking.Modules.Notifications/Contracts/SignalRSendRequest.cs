namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Request to send a SignalR real-time notification
/// </summary>
public sealed record SignalRSendRequest
{
    /// <summary>
    /// Target user slug/identifier for user-specific notifications
    /// If provided, the notification will be sent to this specific user
    /// </summary>
    public string? UserSlug { get; init; }

    /// <summary>
    /// Target group name for group-based notifications (e.g., "admins", "moderators")
    /// If provided, the notification will be sent to all users in this group
    /// </summary>
    public string? GroupName { get; init; }

    /// <summary>
    /// SignalR method name to invoke on clients
    /// </summary>
    public string Method { get; init; } = "ReceiveNotification";

    /// <summary>
    /// Notification title/subject
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Notification message content
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Type of notification for categorization
    /// </summary>
    public NotificationType Type { get; init; } = NotificationType.System;

    /// <summary>
    /// Severity level of the notification
    /// </summary>
    public NotificationSeverity Severity { get; init; } = NotificationSeverity.Info;

    /// <summary>
    /// Additional data payload to send with the notification
    /// Will be serialized as JSON
    /// </summary>
    public object? Data { get; init; }

    /// <summary>
    /// Optional correlation ID for tracking
    /// </summary>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// Validates that either UserSlug or GroupName is provided, but not both
    /// </summary>
    /// <returns>True if the request is valid</returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(UserSlug) ^ !string.IsNullOrEmpty(GroupName);
    }

    /// <summary>
    /// Gets the notification payload that will be sent to SignalR clients
    /// </summary>
    /// <returns>Structured payload object</returns>
    public object GetNotificationPayload()
    {
        return new
        {
            Title,
            Message,
            Type = Type.ToString(),
            Severity = Severity.ToString(),
            Data,
            Timestamp = DateTime.UtcNow,
            CorrelationId
        };
    }
}
