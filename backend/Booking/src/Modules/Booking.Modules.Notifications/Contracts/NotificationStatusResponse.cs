namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Response containing the status of a notification
/// </summary>
public sealed class NotificationStatusResponse
{
    public Guid Id { get; init; }

    public NotificationStatus Status { get; init; }

    public string Recipient { get; init; } = string.Empty;

    public string? Subject { get; init; }

    public NotificationChannel Channel { get; init; }

    public NotificationPriority Priority { get; init; }

    public int Attempts { get; init; }

    public int MaxAttempts { get; init; }

    public string? LastError { get; init; }

    public DateTime? LastAttemptAt { get; init; }

    public DateTime? SentAt { get; init; }

    public DateTime? ScheduledAt { get; init; }

    public DateTime CreatedAt { get; init; }

    public string? ProviderMessageId { get; init; }
}
