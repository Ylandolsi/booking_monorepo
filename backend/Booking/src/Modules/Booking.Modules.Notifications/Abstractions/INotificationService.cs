using Booking.Modules.Notifications.Contracts;

namespace Booking.Modules.Notifications.Abstractions;

/// <summary>
/// High-level service for sending notifications across various channels
/// </summary>
public interface INotificationService
{
    
    /// <summary>
    /// Enqueues an email notification for background delivery with transactional safety.
    /// This is the recommended approach for sending emails.
    /// </summary>
    /// <param name="request">Email send request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing the notification ID</returns>
    Task<SendNotificationResult> EnqueueEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Sends an email notification immediately (not recommended for most use cases).
    /// Use EnqueueEmailAsync for better reliability and transactional safety.
    /// </summary>
    /// <param name="request">Email send request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the send operation</returns>
    Task<SendNotificationResult> SendEmailAsync(
        SendEmailRequest request,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves the status of a notification
    /// </summary>
    /// <param name="notificationId">Notification identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current notification status, or null if not found</returns>
    Task<NotificationStatusResponse?> GetNotificationStatusAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a pending notification
    /// </summary>
    /// <param name="notificationId">Notification identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if cancelled successfully, false otherwise</returns>
    Task<bool> CancelNotificationAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default);
}
