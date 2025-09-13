using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payout.User.History;

public record GetPayoutHistoryQuery(int UserId) : IQuery<List<PayoutResponse>>; 