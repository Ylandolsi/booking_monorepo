using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payout.User.History;

public record GetPayoutHistoryQuery(int UserId) : IQuery<List<Domain.Entities.Payout>>; 