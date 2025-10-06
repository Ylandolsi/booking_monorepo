using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Booking.Common.RealTime;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        _logger.LogInformation(
            "User {UserId} joined their personal group with connection {ConnectionId}",
            userId, Context.ConnectionId);
    }

    public async Task LeaveUserGroup(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        _logger.LogInformation(
            "User {UserId} left their personal group with connection {ConnectionId}",
            userId, Context.ConnectionId);
    }

    /// <summary>
    /// Allows admin users to join the admin group for receiving critical alerts
    /// This should be called after authenticating and verifying admin role
    /// </summary>
    public async Task JoinAdminGroup()
    {
        // Get user claims to verify admin role
        var userRole = Context.User?.FindFirst("role")?.Value;
        var isAdmin = userRole?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

        if (isAdmin)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
            _logger.LogInformation(
                "Admin user joined admin group with connection {ConnectionId}",
                Context.ConnectionId);
        }
        else
        {
            _logger.LogWarning(
                "Non-admin user attempted to join admin group with connection {ConnectionId}",
                Context.ConnectionId);
            throw new HubException("Unauthorized: Only admins can join the admin group");
        }
    }

    /// <summary>
    /// Allows admin users to leave the admin group
    /// </summary>
    public async Task LeaveAdminGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admins");
        _logger.LogInformation(
            "Admin user left admin group with connection {ConnectionId}",
            Context.ConnectionId);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier ?? Context.User?.Identity?.Name;

        _logger.LogInformation(
            "User {UserId} connected to notification hub with connection {ConnectionId}",
            userId ?? "Unknown", Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier ?? Context.User?.Identity?.Name;

        if (exception != null)
        {
            _logger.LogWarning(exception,
                "User {UserId} disconnected from notification hub with error",
                userId ?? "Unknown");
        }
        else
        {
            _logger.LogInformation(
                "User {UserId} disconnected from notification hub",
                userId ?? "Unknown");
        }

        await base.OnDisconnectedAsync(exception);
    }
}