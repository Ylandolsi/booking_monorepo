using Booking.Common.RealTime;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.AdminNotifications;

public class AdminNotificationPersistence : IAdminNotificationPersistence
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<AdminNotificationPersistence> _logger;

    public AdminNotificationPersistence(
        CatalogDbContext context,
        ILogger<AdminNotificationPersistence> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SaveNotificationAsync(
        string title,
        string message,
        string severity,
        string type,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        string? metadata = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var notificationSeverity = Enum.Parse<AdminNotificationSeverity>(severity, true);
            var notificationType = type.ToLower() switch
            {
                "integration_failure" => AdminNotificationType.IntegrationFailure,
                "payment_anomaly" => AdminNotificationType.PaymentAnomaly,
                "session_booking_issue" => AdminNotificationType.SessionBookingIssue,
                "health_check_failure" => AdminNotificationType.HealthCheckFailure,
                "system_error" => AdminNotificationType.SystemError,
                _ => AdminNotificationType.Other
            };

            var notification = new AdminNotification(
                title,
                message,
                notificationSeverity,
                notificationType,
                relatedEntityId,
                relatedEntityType,
                metadata
            );

            await _context.AdminNotifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Admin notification persisted: {Title} - Severity: {Severity}",
                title, severity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist admin notification: {Title}", title);
        }
    }
}
