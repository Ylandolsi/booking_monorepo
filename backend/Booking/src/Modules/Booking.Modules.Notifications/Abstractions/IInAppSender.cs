using Booking.Modules.Notifications.Contracts;

namespace Booking.Modules.Notifications.Abstractions;

/// <summary>
/// Abstraction for managing persistent in-app notifications
/// Used for admin dashboard notifications and user notification history
/// </summary>
public interface IInAppSender
{
    /// <summary>
    /// Saves a notification to persistent storage for later viewing
    /// </summary>
    /// <param name="request">The in-app notification request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing the notification ID</returns>
    Task<InAppSendResult> SaveNotificationAsync(
        InAppNotificationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a specific notification as read
    /// </summary>
    /// <param name="notificationId">The notification ID to mark as read</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successfully marked as read</returns>
    Task<bool> MarkAsReadAsync(
        int notificationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks all notifications for a recipient as read
    /// </summary>
    /// <param name="recipientId">The recipient ID (user ID or "admins")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of notifications marked as read</returns>
    Task<int> MarkAllAsReadAsync(
        string recipientId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets unread notification count for a recipient
    /// </summary>
    /// <param name="recipientId">The recipient ID (user ID or "admins")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Count of unread notifications</returns>
    Task<int> GetUnreadCountAsync(
        string recipientId,
        CancellationToken cancellationToken = default);
}