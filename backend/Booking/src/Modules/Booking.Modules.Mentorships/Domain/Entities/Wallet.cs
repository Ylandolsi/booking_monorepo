namespace Booking.Modules.Mentorships.Domain.Entities;

public class Wallet
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public decimal Balance { get; private set; }
    public decimal PendingBalance { get; private set; }

    public Wallet(int userId, decimal balance = 0)
    {
        UserId = userId;
        Balance = balance;
    }

    public void UpdateBalance(decimal balanceToAdd)
    {
        Balance += balanceToAdd;
    }

    public void UpdatePendingBalance(decimal balanceToAdd)
    {
        PendingBalance += balanceToAdd;
    }
}