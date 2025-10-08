namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Request to send a multi-channel notification (email, SignalR, in-app).
/// This is the unified request contract that supports all notification channels.
/// </summary>
public sealed record SendNotificationRequest
{
    /// <summary>
    /// Primary recipient identifier (email address, user ID, or group identifier)
    /// </summary>
    public required string Recipient { get; init; }

    /// <summary>
    /// Subject/title of the notification
    /// </summary>
    public required string Subject { get; init; }

    /// <summary>
    /// Main message content of the notification
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Channels through which to send this notification
    /// </summary>
    public required NotificationChannel[] Channels { get; init; }

    /// <summary>
    /// Type of notification for categorization and filtering
    /// </summary>
    public NotificationType Type { get; init; } = NotificationType.System;

    /// <summary>
    /// Severity level of the notification
    /// </summary>
    public NotificationSeverity Severity { get; init; } = NotificationSeverity.Info;

    // Email-specific properties
    /// <summary>
    /// HTML body content for email (if using raw body instead of template)
    /// </summary>
    public string? HtmlBody { get; init; }

    /// <summary>
    /// Plain text body content for email (fallback)
    /// </summary>
    public string? TextBody { get; init; }

    /// <summary>
    /// Email template name to use (alternative to HtmlBody)
    /// </summary>
    public string? TemplateName { get; init; }

    /// <summary>
    /// Data to populate the email template
    /// </summary>
    public object? TemplateData { get; init; }

    // SignalR-specific properties
    /// <summary>
    /// SignalR group name for group notifications (optional)
    /// </summary>
    public string? GroupName { get; init; }

    /// <summary>
    /// SignalR method name to invoke on clients (defaults to "ReceiveNotification")
    /// </summary>
    public string SignalRMethod { get; init; } = "ReceiveNotification";

    /// <summary>
    /// Additional data payload for SignalR clients
    /// </summary>
    public object? SignalRData { get; init; }

    // In-App specific properties
    /// <summary>
    /// Reference to related entity for in-app notifications (optional)
    /// </summary>
    public string? RelatedEntityId { get; init; }

    /// <summary>
    /// Type of related entity for in-app notifications (optional)
    /// </summary>
    public string? RelatedEntityType { get; init; }

    /// <summary>
    /// Additional metadata as JSON string for in-app notifications
    /// </summary>
    public string? Metadata { get; init; }

    // Common properties
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

    /// <summary>
    /// Converts this unified request to an email-specific request
    /// </summary>
    /// <returns>SendEmailRequest with email-specific properties</returns>
    public SendEmailRequest ToEmailRequest() => new()
    {
        Recipient = Recipient,
        Subject = Subject,
        HtmlBody = HtmlBody,
        TextBody = TextBody ?? Message,
        TemplateName = TemplateName,
        TemplateData = TemplateData,
        CorrelationId = CorrelationId,
        NotificationReference = NotificationReference,
        Priority = Priority,
        ScheduledAt = ScheduledAt,
        CreatedBy = CreatedBy,
        MaxAttempts = MaxAttempts
    };

    /// <summary>
    /// Converts this unified request to an in-app notification request
    /// </summary>
    /// <returns>InAppNotificationRequest with in-app specific properties</returns>
    public InAppNotificationRequest ToInAppRequest() => new()
    {
        RecipientId = Recipient,
        Title = Subject,
        Message = Message,
        Type = Type,
        Severity = Severity,
        Metadata = Metadata,
        RelatedEntityId = RelatedEntityId,
        RelatedEntityType = RelatedEntityType,
        CorrelationId = CorrelationId
    };

    /// <summary>
    /// Converts this unified request to a SignalR notification request
    /// </summary>
    /// <returns>SignalRSendRequest with SignalR specific properties</returns>
    public SignalRSendRequest ToSignalRRequest() => new()
    {
        UserSlug = string.IsNullOrEmpty(GroupName) ? Recipient : null,
        GroupName = GroupName,
        Method = SignalRMethod,
        Title = Subject,
        Message = Message,
        Type = Type,
        Severity = Severity,
        Data = SignalRData,
        CorrelationId = CorrelationId
    };
}