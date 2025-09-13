using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payout.User.Request;

public record PayoutCommand(int UserId, decimal Amount) : ICommand;