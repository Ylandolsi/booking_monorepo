using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Sessions.Cancel;

public sealed record CancelSessionCommand(
    int SessionId,
    int UserId,
    string? CancellationReason) : ICommand;
