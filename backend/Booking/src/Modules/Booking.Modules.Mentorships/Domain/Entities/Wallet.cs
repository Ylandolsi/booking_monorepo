namespace Booking.Modules.Mentorships.Domain.Entities;

public class Wallet
{
    public int Id { get; private set ; }
    public int UserId { get; private set ; }
    public decimal Balance { get; private set ; } = 0; 

    public Wallet(int userId, decimal balance)
    {
        UserId = userId;
        Balance = balance;
    }

    public void UpdateBalance(int balanceToAdd)
    {
        Balance += balanceToAdd;
    }
    
}