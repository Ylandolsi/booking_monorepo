namespace Booking.Modules.Mentorships.refactored.Features.Payout.User.History;

public record GetPayoutHistoryQuery(int UserId) : IQuery<List<PayoutResponse>>; 