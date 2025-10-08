using Booking.Modules.Notifications.Contracts;

namespace Booking.Modules.Notifications.Abstractions;

/// <summary>
/// Abstraction for sending real-time notifications via SignalR
/// Supports both user-specific and group-based notifications (including admin alerts)
/// </summary>
public interface ISignalRSender
{


    /// <summary>
    /// Sends a real-time notification using a structured request
    /// Automatically routes to user or group based on the request properties
    /// </summary>
    /// <param name="request">The SignalR notification request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure</returns>
    Task<SignalRSendResult> SendAsync(
        SignalRSendRequest request,
        CancellationToken cancellationToken = default);
}