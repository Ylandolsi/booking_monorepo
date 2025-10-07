using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Booking.Common.RealTime;

public interface IAdminNotificationPersistence
{
    Task SaveNotificationAsync(
        string title,
        string message,
        AdminAlertSeverity severity,
        AdminAlertType type,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        string? metadata = null,
        CancellationToken cancellationToken = default);
}

public class NotificationService
{
    private readonly IHubContext<NotificationHub>? _hubContext;
    private readonly ILogger<NotificationService> _logger;
    private readonly IAdminNotificationPersistence? _adminNotificationPersistence;
    private const string AdminGroupName = "admins";

    public NotificationService(
        IHubContext<NotificationHub>? hubContext,
        ILogger<NotificationService> logger,
        IAdminNotificationPersistence? adminNotificationPersistence = null)
    {
        _hubContext = hubContext;
        _logger = logger;
        _adminNotificationPersistence = adminNotificationPersistence;
    }

    public async Task SendNotificationAsync(string userSlug, NotificationDto notification)
    {
        try
        {
            if (_hubContext != null)
            {
                await _hubContext.Clients.Group(userSlug)
                    .SendAsync("ReceiveNotification", notification);

                _logger.LogInformation("Real-time notification sent successfully to user {userSlug}", userSlug);
            }
            else
            {
                _logger.LogWarning("Hub context is null, cannot send real-time notification to user {userSlug}",
                    userSlug);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send real-time notification to user {userSlug}", userSlug);
        }
    }

    /// <summary>
    /// Sends a critical alert to all connected admin users
    /// </summary>
    public async Task SendAdminAlertAsync(
        string title,
        string message,
        AdminAlertSeverity severity = AdminAlertSeverity.Error,
        AdminAlertType type = AdminAlertType.SystemError,
        object? metadata = null,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var alert = new AdminAlertDto(
                Id: Guid.NewGuid().ToString(),
                Type: type.ToString(),
                Title: title,
                Message: message,
                Severity: severity.ToString(),
                CreatedAt: DateTime.UtcNow,
                Metadata: metadata
            );


            // Send real-time notification to connected admins
            if (_hubContext != null)
            {
                await _hubContext.Clients.Group(AdminGroupName)
                    .SendAsync("ReceiveAdminAlert", alert, cancellationToken);

                _logger.LogInformation(
                    "Admin alert sent: {Title} - Type: {Type} - Severity: {Severity}",
                    title, type, severity);
            }
            else
            {
                _logger.LogError("Hub context is null, cannot send admin alert: {Title}", title);
            }

            // Persist to database for admin dashboard
            if (_adminNotificationPersistence != null)
            {
                await _adminNotificationPersistence.SaveNotificationAsync(
                    title,
                    message,
                    severity,
                    type,
                    relatedEntityId,
                    relatedEntityType,
                    metadata != null ? System.Text.Json.JsonSerializer.Serialize(metadata) : null,
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send admin alert: {Title}", title);
        }
    }

    /// <summary>
    /// Sends an integration failure alert to admins
    /// </summary>
    public async Task SendIntegrationFailureAlertAsync(
        string integrationName,
        string orderId,
        string errorMessage,
        object? additionalData = null)
    {
        var metadata = new
        {
            IntegrationName = integrationName,
            OrderId = orderId,
            ErrorMessage = errorMessage,
            AdditionalData = additionalData,
            Timestamp = DateTime.UtcNow
        };

        await SendAdminAlertAsync(
            title: $"{integrationName} Integration Failed",
            message: $"Order {orderId}: {errorMessage}",
            severity: AdminAlertSeverity.Critical,
            type: AdminAlertType.IntegrationFailure,
            metadata: metadata,
            relatedEntityId: orderId,
            relatedEntityType: "Order"
        );
    }
}

public record NotificationDto(
    string Id,
    string Type,
    string Title,
    string Message,
    DateTime CreatedAt,
    object? Data = null
);

public record AdminAlertDto(
    string Id,
    string Type,
    string Title,
    string Message,
    string Severity,
    DateTime CreatedAt,
    object? Metadata = null
);

public enum AdminAlertSeverity
{
    Info,
    Warning,
    Error,
    Critical
}

public enum AdminAlertType
{
    IntegrationFailure,
    PaymentAnomaly,
    SessionBookingIssue,
    SystemError,
    HealthCheckFailure,
    Other
}