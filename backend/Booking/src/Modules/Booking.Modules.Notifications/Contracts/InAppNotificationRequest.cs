namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Request to save an in-app notification to persistent storage
/// </summary>
public sealed record InAppNotificationRequest
{
    /// <summary>
    /// Recipient identifier (user ID or "admins" for admin notifications)
    /// </summary>
    public required string RecipientId { get; init; }

    /// <summary>
    /// Notification title
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
    /// Severity level for admin alerts
    /// </summary>
    public NotificationSeverity Severity { get; init; } = NotificationSeverity.Info;

    /// <summary>
    /// Additional metadata as JSON string
    /// </summary>
    public string? Metadata { get; init; }

    /// <summary>
    /// Reference to related entity (optional)
    /// </summary>
    public string? RelatedEntityId { get; init; }

    /// <summary>
    /// Type of related entity (optional)
    /// </summary>
    public string? RelatedEntityType { get; init; }

    /// <summary>
    /// Correlation ID for tracking
    /// </summary>
    public string? CorrelationId { get; init; }
}