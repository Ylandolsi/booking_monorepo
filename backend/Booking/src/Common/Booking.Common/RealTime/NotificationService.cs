using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Booking.Common.RealTime;

public class NotificationService
{
    private readonly IHubContext<NotificationHub>? _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IHubContext<NotificationHub>? hubContext,
        ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
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
}

public record NotificationDto(
    string Id,
    string Type,
    string Title,
    string Message,
    DateTime CreatedAt,
    object? Data = null
);