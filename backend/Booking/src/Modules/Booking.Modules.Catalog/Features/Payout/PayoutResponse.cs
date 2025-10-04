using System.Text.Json.Serialization;
using Booking.Modules.Catalog.Domain.Entities;

namespace Booking.Modules.Catalog.Features.Payout;

public record PayoutResponse
{
    // public int Id { get; init; }
    //public int StoreSlug { get; init; }
    public string KonnectWalletId { get; init; }
    public int WalletId { get; init; }
    public decimal Amount { get; init; }
    public string PaymentRef { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))] // convert enum to string
    public PayoutStatus Status { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record PayoutResponseAdmin : PayoutResponse
{
    public int Id { get; init; }
    public int StoreId { get; init; }
}