using Booking.Modules.Mentorships.Domain.Entities;

namespace Booking.Modules.Mentorships.Features.Payout.Admin.GetAll;

public record GetAllPayoutsResponse
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string KonnectWalletId { get; init; }
    public int WalletId { get; init; }
    public decimal Amount { get; init; }
    public string PaymentRef { get; init; }
    public PayoutStatus Status { get; init; }
}