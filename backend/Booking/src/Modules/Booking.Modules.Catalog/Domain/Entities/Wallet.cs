namespace Booking.Modules.Catalog.Domain.Entities;

public class Wallet
{
    public Wallet(int storeId, decimal balance = 0)
    {
        StoreId = storeId;
        Balance = balance;
    }

    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public decimal Balance { get; private set; }
    public decimal PendingBalance { get; private set; }

    public void UpdateBalance(decimal balanceToAdd)
    {
        Balance += balanceToAdd;
    }

    public void UpdatePendingBalance(decimal balanceToAdd)
    {
        PendingBalance += balanceToAdd;
    }
}