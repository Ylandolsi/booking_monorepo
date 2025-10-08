namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Represents a persistent in-app notification that can be displayed to users.
/// Used for notification history and unread notification tracking.
/// </summary>
public sealed record InAppNotification
{
    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public int Id { get; init; }

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
    public NotificationType Type { get; init; }

    /// <summary>
    /// Severity level of the notification
    /// </summary>
    public NotificationSeverity Severity { get; init; }

    /// <summary>
    /// Whether the notification has been read
    /// </summary>
    public bool IsRead { get; init; }

    /// <summary>
    /// When the notification was created
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// When the notification was read (if applicable)
    /// </summary>
    public DateTime? ReadAt { get; init; }

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