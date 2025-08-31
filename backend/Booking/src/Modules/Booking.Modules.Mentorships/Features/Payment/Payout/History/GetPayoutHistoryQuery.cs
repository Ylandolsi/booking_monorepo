using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payment.Payout.History;

public record GetPayoutHistoryQuery(int UserId) : IQuery<List<Domain.Entities.Payout>>; 