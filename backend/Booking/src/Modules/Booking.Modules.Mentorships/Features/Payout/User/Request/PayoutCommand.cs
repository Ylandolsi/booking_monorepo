using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payout.User.Request;

public record PayoutCommand(int UserId, decimal Amount) : ICommand;