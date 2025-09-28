using Booking.Common.Messaging;

namespace Booking.Modules.Catalog.Features.Payment.Wallet;

public record GetWalletQuery(int UserId) : IQuery<GetWalletResponse>;
