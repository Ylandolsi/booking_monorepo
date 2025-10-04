using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Payout : Entity
{
    public Payout(int storeId, string konnectWalletId, int walletId, decimal amount)
    {
        StoreId = storeId;
        KonnectWalletId = konnectWalletId;
        WalletId = walletId;
        Amount = amount;
        Status = PayoutStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int StoreId { get; private set; }
    public string KonnectWalletId { get; private set; }
    public int WalletId { get; private set; }
    public decimal Amount { get; private set; }

    public string PaymentRef { get; private set; } = ""; // from Konnect
    public PayoutStatus Status { get; private set; }

    // nav
    public Store Store { get; private set; } = default!;

    public void Complete()
    {
        Status = PayoutStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = PayoutStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve(string paymentRef)
    {
        PaymentRef = paymentRef;
        Status = PayoutStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Pending()
    {
        Status = PayoutStatus.Pending;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum PayoutStatus
{
    Pending,
    Approved,
    Rejected,
    Completed
}