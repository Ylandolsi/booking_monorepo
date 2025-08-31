using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payment.Payout.Request;

public record PayoutCommand(int UserId, int Amount) : ICommand;