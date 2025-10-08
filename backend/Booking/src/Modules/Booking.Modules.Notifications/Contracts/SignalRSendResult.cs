namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Result of a SignalR notification send operation
/// </summary>
public sealed record SignalRSendResult
{
    /// <summary>
    /// Indicates if the send operation was successful
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Number of clients that received the notification
    /// </summary>
    public int? ClientCount { get; init; }

    /// <summary>
    /// Timestamp when the notification was sent
    /// </summary>
    public DateTime SentAt { get; init; }

    /// <summary>
    /// Creates a successful send result
    /// </summary>
    /// <param name="clientCount">Number of clients that received the notification</param>
    /// <returns>Successful SignalRSendResult</returns>
    public static SignalRSendResult Success(int? clientCount = null) => new()
    {
        IsSuccess = true,
        ClientCount = clientCount,
        SentAt = DateTime.UtcNow
    };

    /// <summary>
    /// Creates a failed send result
    /// </summary>
    /// <param name="errorMessage">The error message</param>
    /// <returns>Failed SignalRSendResult</returns>
    public static SignalRSendResult Failed(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage,
        SentAt = DateTime.UtcNow
    };
}