using Booking.Modules.Notifications.Abstractions;
using Booking.Modules.Notifications.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Booking.Modules.Notifications.Infrastructure.Adapters.SignalR;

/// <summary>
/// Implementation of ISignalRSender that sends real-time notifications through SignalR
/// Supports both user-specific and group-based notifications
/// </summary>
public sealed class SignalRSender : ISignalRSender
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<SignalRSender> _logger;

    public SignalRSender(
        IHubContext<NotificationHub> hubContext,
        ILogger<SignalRSender> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Sends a real-time notification using a structured request
    /// </summary>
    public async Task<SignalRSendResult> SendAsync(
        SignalRSendRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate the request
            if (!request.IsValid())
            {
                var error = "Invalid SignalR request: Must specify either UserSlug or GroupName, but not both";
                _logger.LogError(error);
                return SignalRSendResult.Failed(error);
            }

            var payload = request.GetNotificationPayload();

            // Route to appropriate method based on request
            if (!string.IsNullOrEmpty(request.UserSlug))
            {
                _logger.LogDebug("Routing SignalR notification to user {UserSlug} via method {Method}",
                    request.UserSlug, request.Method);

                await _hubContext.Clients.User(request.UserSlug)
                    .SendAsync(request.Method, payload, cancellationToken);

                _logger.LogInformation("Successfully sent SignalR notification to user {UserSlug}", request.UserSlug);
                return SignalRSendResult.Success(clientCount: 1);
            }

            if (!string.IsNullOrEmpty(request.GroupName))
            {
                _logger.LogDebug("Routing SignalR notification to group {GroupName} via method {Method}",
                    request.GroupName, request.Method);

                await _hubContext.Clients.Group(request.GroupName)
                    .SendAsync(request.Method, payload, cancellationToken);

                _logger.LogInformation("Successfully sent SignalR notification to group {GroupName}", request.GroupName);
                return SignalRSendResult.Success(clientCount: null);
            }

            // This should never happen due to validation, but just in case
            var fallbackError = "No valid target specified in SignalR request";
            _logger.LogError(fallbackError);
            return SignalRSendResult.Failed(fallbackError);
        }
        catch (Exception ex)
        {
            var target = !string.IsNullOrEmpty(request.UserSlug) ? $"user {request.UserSlug}" : $"group {request.GroupName}";
            _logger.LogError(ex, "Failed to send SignalR notification to {Target}", target);
            return SignalRSendResult.Failed($"Failed to send to {target}: {ex.Message}");
        }
    }
}

/// <summary>
/// SignalR Hub for handling notification connections and group management
/// </summary>
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Called when a client connects to the hub
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        _logger.LogDebug("User {UserId} connected to notification hub with connection {ConnectionId}",
            userId, Context.ConnectionId);

        // Auto-join user to their personal group for user-specific notifications
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects from the hub
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        _logger.LogDebug("User {UserId} disconnected from notification hub with connection {ConnectionId}",
            userId, Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Allows clients to join specific notification groups
    /// </summary>
    /// <param name="groupName">Name of the group to join</param>
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogDebug("Connection {ConnectionId} joined group {GroupName}",
            Context.ConnectionId, groupName);
    }

    /// <summary>
    /// Allows clients to leave specific notification groups
    /// </summary>
    /// <param name="groupName">Name of the group to leave</param>
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogDebug("Connection {ConnectionId} left group {GroupName}",
            Context.ConnectionId, groupName);
    }
}