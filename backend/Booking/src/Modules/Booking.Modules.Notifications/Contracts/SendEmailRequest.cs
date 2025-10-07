namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Request to send an email notification
/// </summary>
public sealed record SendEmailRequest
{
    /// <summary>
    /// Email recipient address
    /// </summary>
    public required string Recipient { get; init; }

    /// <summary>
    /// Email subject line
    /// </summary>
    public required string Subject { get; init; }

    /// <summary>
    /// HTML body content (if using raw body instead of template)
    /// </summary>
    public string? HtmlBody { get; init; }

    /// <summary>
    /// Plain text body content (fallback)
    /// </summary>
    public string? TextBody { get; init; }

    /// <summary>
    /// Template name to use (alternative to HtmlBody)
    /// </summary>
    public string? TemplateName { get; init; }

    /// <summary>
    /// Data to populate the template
    /// </summary>
    public object? TemplateData { get; init; }

    /// <summary>
    /// Optional correlation ID for tracking related operations
    /// </summary>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// Optional notification reference for idempotency
    /// </summary>
    public string? NotificationReference { get; init; }

    /// <summary>
    /// Priority level for this notification
    /// </summary>
    public NotificationPriority Priority { get; init; } = NotificationPriority.Normal;

    /// <summary>
    /// Optional scheduled send time (null = send immediately)
    /// </summary>
    public DateTime? ScheduledAt { get; init; }

    /// <summary>
    /// Identifier of the user who triggered this notification
    /// </summary>
    public string? CreatedBy { get; init; }

    /// <summary>
    /// Maximum retry attempts (null = use default)
    /// </summary>
    public int? MaxAttempts { get; init; }
}
