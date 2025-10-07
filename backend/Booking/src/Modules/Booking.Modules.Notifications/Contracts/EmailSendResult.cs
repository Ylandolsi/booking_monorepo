namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Result of an email send operation from the underlying provider
/// </summary>
public sealed record EmailSendResult
{
    /// <summary>
    /// Indicates if the email was sent successfully
    /// </summary>
    public required bool IsSuccess { get; init; }

    /// <summary>
    /// Provider-specific message ID (for tracking)
    /// </summary>
    public string? MessageId { get; init; }

    /// <summary>
    /// Error message if the send failed
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// When the email was sent
    /// </summary>
    public DateTime SentAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static EmailSendResult Success(string? messageId = null) => new()
    {
        IsSuccess = true,
        MessageId = messageId
    };

    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static EmailSendResult Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}
