using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Payment.Wallet;

public record GetWalletQuery(int UserId) : IQuery<GetWalletResponse>;
