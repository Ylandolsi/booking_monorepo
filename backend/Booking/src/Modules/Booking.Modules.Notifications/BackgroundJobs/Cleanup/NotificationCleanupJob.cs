using Booking.Modules.Notifications.Contracts;
using Booking.Modules.Notifications.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Notifications.BackgroundJobs.Cleanup;

/// <summary>
/// Background job that cleans up old notifications from the outbox
/// </summary>
public class NotificationCleanupJob
{
    private readonly NotificationsDbContext _dbContext;
    private readonly ILogger<NotificationCleanupJob> _logger;

    public NotificationCleanupJob(
        NotificationsDbContext dbContext,
        ILogger<NotificationCleanupJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Removes old sent and failed notifications from the database
    /// </summary>
    /// <param name="retentionDays">Number of days to retain notifications (default: 30)</param>
    public async Task CleanupAsync(int retentionDays = 30, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting notification cleanup job with retention period of {RetentionDays} days", retentionDays);

        var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);

        // Delete old sent notifications
        var sentDeleted = await _dbContext.NotificationOutbox
            .Where(n => n.Status == NotificationStatus.Sent && n.SentAt < cutoffDate)
            .ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("Deleted {Count} old sent notifications", sentDeleted);

        // Delete old failed notifications (that have exceeded max attempts)
        var failedDeleted = await _dbContext.NotificationOutbox
            .Where(n => n.Status == NotificationStatus.Failed &&
                       n.Attempts >= n.MaxAttempts &&
                       n.LastAttemptAt < cutoffDate)
            .ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("Deleted {Count} old failed notifications", failedDeleted);

        // Delete old cancelled notifications
        var cancelledDeleted = await _dbContext.NotificationOutbox
            .Where(n => n.Status == NotificationStatus.Cancelled &&
                       n.UpdatedAt < cutoffDate)
            .ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("Deleted {Count} old cancelled notifications", cancelledDeleted);

        var totalDeleted = sentDeleted + failedDeleted + cancelledDeleted;
        _logger.LogInformation(
            "Notification cleanup job completed. Total deleted: {Total} (Sent: {Sent}, Failed: {Failed}, Cancelled: {Cancelled})",
            totalDeleted,
            sentDeleted,
            failedDeleted,
            cancelledDeleted);
    }
}
