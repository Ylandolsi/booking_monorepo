using Booking.Common.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Modules.Catalog.Domain.Entities;

public class AdminNotification : Entity
{
    private AdminNotification()
    {
    }

    public AdminNotification(
        string title,
        string message,
        AdminNotificationSeverity severity,
        AdminNotificationType type,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        string? metadata = null)
    {
        Title = title;
        Message = message;
        Severity = severity;
        Type = type;
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
        Metadata = metadata;
        IsRead = false;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public AdminNotificationSeverity Severity { get; private set; }
    public AdminNotificationType Type { get; private set; }
    public string? RelatedEntityId { get; private set; }
    public string? RelatedEntityType { get; private set; }
    public string? Metadata { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum AdminNotificationSeverity
{
    Info = 0,
    Warning = 1,
    Error = 2,
    Critical = 3
}

public enum AdminNotificationType
{
    IntegrationFailure,
    PaymentAnomaly,
    SessionBookingIssue,
    SystemError,
    HealthCheckFailure,
    Other
}
