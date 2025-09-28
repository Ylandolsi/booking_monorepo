using System.Text.Json.Serialization;
using Booking.Modules.Mentorships.refactored.Domain.Entities;

namespace Booking.Modules.Mentorships.refactored.Features.Payout;

public record PayoutResponse
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string KonnectWalletId { get; init; }
    public int WalletId { get; init; }
    public decimal Amount { get; init; }
    public string PaymentRef { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PayoutStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}