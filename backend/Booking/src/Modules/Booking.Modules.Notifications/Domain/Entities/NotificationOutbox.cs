using Booking.Common.Domain.Entity;
using Booking.Modules.Notifications.Contracts;

namespace Booking.Modules.Notifications.Domain.Entities;

/// <summary>
/// Represents a notification in the outbox pattern for transactional delivery
/// </summary>
public sealed class NotificationOutbox : Entity
{
    private NotificationOutbox()
    {
        // EF Core constructor
    }

    public NotificationOutbox(
        string recipient,
        NotificationChannel channel,
        string? subject,
        string payload,
        string? templateName,
        NotificationPriority priority,
        string? notificationReference,
        string? correlationId,
        DateTime? scheduledAt,
        string? createdBy,
        int maxAttempts = 3)
    {
        Id = Guid.NewGuid();
        Recipient = recipient;
        Channel = channel;
        Subject = subject;
        Payload = payload;
        TemplateName = templateName;
        Priority = priority;
        NotificationReference = notificationReference;
        CorrelationId = correlationId;
        ScheduledAt = scheduledAt;
        CreatedBy = createdBy;
        MaxAttempts = maxAttempts;
        Status = NotificationStatus.Pending;
        Attempts = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    /// <summary>
    /// Unique reference for deduplication (optional)
    /// </summary>
    public string? NotificationReference { get; private set; }

    /// <summary>
    /// Delivery channel (Email, SMS, Push, etc.)
    /// </summary>
    public NotificationChannel Channel { get; private set; }

    /// <summary>
    /// Priority level
    /// </summary>
    public NotificationPriority Priority { get; private set; }

    /// <summary>
    /// Recipient identifier (email address, phone number, etc.)
    /// </summary>
    public string Recipient { get; private set; }

    /// <summary>
    /// Optional recipient user ID for tracking
    /// </summary>
    public int? RecipientUserId { get; private set; }

    /// <summary>
    /// Subject line (for emails)
    /// </summary>
    public string? Subject { get; private set; }

    /// <summary>
    /// JSON payload containing template data or rendered content
    /// </summary>
    public string Payload { get; private set; }

    /// <summary>
    /// Template name to use for rendering (optional)
    /// </summary>
    public string? TemplateName { get; private set; }

    /// <summary>
    /// Current delivery status
    /// </summary>
    public NotificationStatus Status { get; private set; }

    /// <summary>
    /// Number of delivery attempts made
    /// </summary>
    public int Attempts { get; private set; }

    /// <summary>
    /// Maximum number of retry attempts allowed
    /// </summary>
    public int MaxAttempts { get; private set; }

    /// <summary>
    /// Timestamp of the last delivery attempt
    /// </summary>
    public DateTime? LastAttemptAt { get; private set; }

    /// <summary>
    /// Error message from the last failed attempt
    /// </summary>
    public string? LastError { get; private set; }

    /// <summary>
    /// Scheduled send time (null = send ASAP)
    /// </summary>
    public DateTime? ScheduledAt { get; private set; }

    /// <summary>
    /// Timestamp when successfully sent
    /// </summary>
    public DateTime? SentAt { get; private set; }

    /// <summary>
    /// Timestamp when the notification was created
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// User or system that created this notification
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Correlation ID for tracking related operations
    /// </summary>
    public string? CorrelationId { get; private set; }

    /// <summary>
    /// Provider-specific message ID (e.g., AWS SES Message ID)
    /// </summary>
    public string? ProviderMessageId { get; private set; }

    /// <summary>
    /// Marks the notification as successfully sent
    /// </summary>
    public void MarkAsSent(string? providerMessageId = null)
    {
        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        LastAttemptAt = DateTime.UtcNow;
        ProviderMessageId = providerMessageId;
        LastError = null;
    }

    /// <summary>
    /// Records a failed delivery attempt
    /// </summary>
    public void RecordFailedAttempt(string errorMessage)
    {
        Attempts++;
        LastAttemptAt = DateTime.UtcNow;
        LastError = errorMessage;

        if (Attempts >= MaxAttempts)
        {
            Status = NotificationStatus.Failed;
        }
    }

    /// <summary>
    /// Marks the notification as being processed
    /// </summary>
    public void MarkAsProcessing()
    {
        Status = NotificationStatus.Processing;
        Attempts++;
        LastAttemptAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the notification
    /// </summary>
    public void Cancel()
    {
        if (Status == NotificationStatus.Pending)
        {
            Status = NotificationStatus.Cancelled;
        }
    }

    /// <summary>
    /// Checks if the notification is ready to be sent
    /// </summary>
    public bool IsReadyToSend()
    {
        if (Status != NotificationStatus.Pending)
            return false;

        if (Attempts >= MaxAttempts)
            return false;

        if (ScheduledAt.HasValue && ScheduledAt.Value > DateTime.UtcNow)
            return false;

        return true;
    }

    /// <summary>
    /// Calculates the next retry delay using exponential backoff
    /// </summary>
    public TimeSpan GetNextRetryDelay()
    {
        var baseDelay = TimeSpan.FromMinutes(1);
        var multiplier = Math.Pow(2, Attempts);
        var maxDelay = TimeSpan.FromHours(1);

        var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * multiplier);
        return delay > maxDelay ? maxDelay : delay;
    }
}
