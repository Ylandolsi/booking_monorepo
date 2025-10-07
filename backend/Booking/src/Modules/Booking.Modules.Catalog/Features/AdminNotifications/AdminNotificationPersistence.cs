using Booking.Common.RealTime;
using Booking.Modules.Catalog.Domain.Entities;
using Booking.Modules.Catalog.Persistence;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Catalog.Features.AdminNotifications;

public class AdminNotificationPersistence(
    CatalogDbContext context,
    ILogger<AdminNotificationPersistence> logger) : IAdminNotificationPersistence
{
    private readonly CatalogDbContext _context = context;
    private readonly ILogger<AdminNotificationPersistence> _logger = logger;

    public async Task SaveNotificationAsync(
        string title,
        string message,
        AdminAlertSeverity severity,
        AdminAlertType type,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        string? metadata = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Map from AdminAlertSeverity (Common) to AdminNotificationSeverity (Domain)
            var notificationSeverity = severity switch
            {
                AdminAlertSeverity.Info => AdminNotificationSeverity.Info,
                AdminAlertSeverity.Warning => AdminNotificationSeverity.Warning,
                AdminAlertSeverity.Error => AdminNotificationSeverity.Error,
                AdminAlertSeverity.Critical => AdminNotificationSeverity.Critical,
                _ => AdminNotificationSeverity.Info
            };

            // Map from AdminAlertType (Common) to AdminNotificationType (Domain)
            var notificationType = type switch
            {
                AdminAlertType.IntegrationFailure => AdminNotificationType.IntegrationFailure,
                AdminAlertType.PaymentAnomaly => AdminNotificationType.PaymentAnomaly,
                AdminAlertType.SessionBookingIssue => AdminNotificationType.SessionBookingIssue,
                AdminAlertType.SystemError => AdminNotificationType.SystemError,
                AdminAlertType.HealthCheckFailure => AdminNotificationType.HealthCheckFailure,
                AdminAlertType.Other => AdminNotificationType.Other,
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
                "Admin notification persisted: {Title} - Type: {Type} - Severity: {Severity}",
                title, type, severity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist admin notification: {Title}", title);
        }
    }
}
