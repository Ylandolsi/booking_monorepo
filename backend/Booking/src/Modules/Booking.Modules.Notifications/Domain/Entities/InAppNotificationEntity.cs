using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;
using Booking.Modules.Notifications.Contracts;

namespace Booking.Modules.Notifications.Domain.Entities;

/// <summary>
/// Domain entity representing a persistent in-app notification
/// Used for storing notifications that users can view in their notification history
/// </summary>
public sealed class InAppNotificationEntity : Entity
{
    private InAppNotificationEntity()
    {
        // EF Core constructor
    }

    public InAppNotificationEntity(
        string recipientId,
        string title,
        string message,
        NotificationType type,
        NotificationSeverity severity,
        string? metadata = null,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        string? correlationId = null)
    {
        RecipientId = recipientId;
        Title = title;
        Message = message;
        Type = type;
        Severity = severity;
        IsRead = false;
        ReadAt = null;
        Metadata = metadata;
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
        CorrelationId = correlationId;
    }

    /// <summary>
    /// Primary key identifier
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    /// <summary>
    /// Recipient identifier (user ID or "admins" for admin notifications)
    /// </summary>
    public string RecipientId { get; private set; } = null!;

    /// <summary>
    /// Notification title
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Notification message content
    /// </summary>
    public string Message { get; private set; } = null!;

    /// <summary>
    /// Type of notification for categorization
    /// </summary>
    public NotificationType Type { get; private set; }

    /// <summary>
    /// Severity level of the notification
    /// </summary>
    public NotificationSeverity Severity { get; private set; }

    /// <summary>
    /// Whether the notification has been read
    /// </summary>
    public bool IsRead { get; internal set; }

    /// <summary>
    /// When the notification was read (if applicable)
    /// </summary>
    public DateTime? ReadAt { get; internal set; }

    /// <summary>
    /// Additional metadata as JSON string
    /// </summary>
    public string? Metadata { get; private set; }

    /// <summary>
    /// Reference to related entity (optional)
    /// </summary>
    public string? RelatedEntityId { get; private set; }

    /// <summary>
    /// Type of related entity (optional)
    /// </summary>
    public string? RelatedEntityType { get; private set; }

    /// <summary>
    /// Correlation ID for tracking
    /// </summary>
    public string? CorrelationId { get; private set; }

    /// <summary>
    /// Marks the notification as read
    /// </summary>
    public void MarkAsRead()
    {
        if (!IsRead)
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Converts the entity to a contract object
    /// </summary>
    /// <returns>InAppNotification contract</returns>
    public InAppNotification ToContract() => new()
    {
        Id = Id,
        RecipientId = RecipientId,
        Title = Title,
        Message = Message,
        Type = Type,
        Severity = Severity,
        IsRead = IsRead,
        CreatedAt = CreatedAt,
        ReadAt = ReadAt,
        Metadata = Metadata,
        RelatedEntityId = RelatedEntityId,
        RelatedEntityType = RelatedEntityType,
        CorrelationId = CorrelationId
    };
}