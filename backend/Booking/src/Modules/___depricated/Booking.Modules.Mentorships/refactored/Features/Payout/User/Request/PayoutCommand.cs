namespace Booking.Modules.Mentorships.refactored.Features.Payout.User.Request;

public record PayoutCommand(int UserId, decimal Amount) : ICommand;