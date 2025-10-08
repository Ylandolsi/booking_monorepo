namespace Booking.Modules.Notifications.Contracts;

/// <summary>
/// Result of an in-app notification save operation.
/// Contains success status and the saved notification ID.
/// </summary>
public sealed record InAppSendResult
{
    /// <summary>
    /// Indicates whether the save operation was successful
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Error message if the operation failed
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// The ID of the saved notification (if successful)
    /// </summary>
    public int? NotificationId { get; init; }

    /// <summary>
    /// Timestamp when the operation completed
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful result with the notification ID
    /// </summary>
    /// <param name="notificationId">The ID of the saved notification</param>
    /// <returns>Success result</returns>
    public static InAppSendResult Success(int notificationId) => new()
    {
        IsSuccess = true,
        NotificationId = notificationId
    };

    /// <summary>
    /// Creates a failed result with an error message
    /// </summary>
    /// <param name="errorMessage">The error message</param>
    /// <returns>Failed result</returns>
    public static InAppSendResult Failed(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}