namespace Booking.Modules.Mentorships.refactored.Features.Payment.Wallet;

public record GetWalletQuery(int UserId) : IQuery<GetWalletResponse>;
