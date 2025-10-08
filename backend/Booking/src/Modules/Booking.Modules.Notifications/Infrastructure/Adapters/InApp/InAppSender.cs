using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Domain.Entities;
using Booking.Modules.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Notifications.Infrastructure.Adapters.InApp;

/// <summary>
/// Implementation of IInAppSender that manages persistent in-app notifications
/// Handles saving, reading, and querying notifications stored in the database
/// </summary>
public sealed class InAppSender : IInAppSender
{
    private readonly NotificationsDbContext _dbContext;
    private readonly ILogger<InAppSender> _logger;

    public InAppSender(
        NotificationsDbContext dbContext,
        ILogger<InAppSender> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Saves a new in-app notification to the database
    /// </summary>
    public async Task<InAppSendResult> SaveNotificationAsync(
        InAppNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Saving in-app notification for recipient {RecipientId}", request.RecipientId);

            var notification = new InAppNotificationEntity(
                request.RecipientId,
                request.Title,
                request.Message,
                request.Type,
                request.Severity,
                request.Metadata,
                request.RelatedEntityId,
                request.RelatedEntityType,
                request.CorrelationId);

            _dbContext.InAppNotifications.Add(notification);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully saved in-app notification {NotificationId} for recipient {RecipientId}",
                notification.Id, request.RecipientId);

            return InAppSendResult.Success(notification.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save in-app notification for recipient {RecipientId}", request.RecipientId);
            return InAppSendResult.Failed($"Failed to save notification: {ex.Message}");
        }
    }

    /// <summary>
    /// Marks a specific notification as read
    /// </summary>
    public async Task<bool> MarkAsReadAsync(
        int notificationId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Marking notification {NotificationId} as read", notificationId);

            var notification = await _dbContext.InAppNotifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && !n.IsRead, cancellationToken);

            if (notification == null)
            {
                _logger.LogWarning("Notification {NotificationId} not found or already read", notificationId);
                return false;
            }

            notification.MarkAsRead();

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully marked notification {NotificationId} as read", notificationId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark notification {NotificationId} as read", notificationId);
            return false;
        }
    }

    /// <summary>
    /// Marks all notifications for a recipient as read
    /// </summary>
    public async Task<int> MarkAllAsReadAsync(
        string recipientId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Marking all notifications as read for recipient {RecipientId}", recipientId);

            var notifications = await _dbContext.InAppNotifications
                .Where(n => n.RecipientId == recipientId && !n.IsRead)
                .ToListAsync(cancellationToken);

            if (!notifications.Any())
            {
                _logger.LogDebug("No unread notifications found for recipient {RecipientId}", recipientId);
                return 0;
            }

            var readAt = DateTime.UtcNow;
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = readAt;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var notificationCount = notifications.Count;
            _logger.LogInformation("Successfully marked {Count} notifications as read for recipient {RecipientId}",
                notificationCount, recipientId);

            return notificationCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark all notifications as read for recipient {RecipientId}", recipientId);
            return 0;
        }
    }

    /// <summary>
    /// Gets unread notification count for a recipient
    /// </summary>
    public async Task<int> GetUnreadCountAsync(
        string recipientId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting unread notification count for recipient {RecipientId}", recipientId);

            var count = await _dbContext.InAppNotifications
                .CountAsync(n => n.RecipientId == recipientId && !n.IsRead, cancellationToken);

            _logger.LogDebug("Found {Count} unread notifications for recipient {RecipientId}", count, recipientId);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get unread count for recipient {RecipientId}", recipientId);
            return 0;
        }
    }
}