using Booking.Common.Domain.Entity;

namespace Booking.Modules.Mentorships.Domain.Entities;

public class Payout : Entity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int WalletId { get; private set; }
    public decimal Amount { get; private set; }

    public PayoutStatus  Status { get; private set; }

    public Payout(int userId, int walletId, decimal amount)
    {
        UserId = userId;
        WalletId = walletId;
        Amount = amount;
        Status = PayoutStatus.Pending;
        CreatedAt =  DateTime.UtcNow;        
        UpdatedAt = DateTime.UtcNow;
    }

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
    
    
}

public enum PayoutStatus
{
    Pending,
    Completed,
    Rejected,
}