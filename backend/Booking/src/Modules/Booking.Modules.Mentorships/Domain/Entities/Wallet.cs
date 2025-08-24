namespace Booking.Modules.Mentorships.Domain.Entities;

public class Wallet
{
    public int Id { get; private set ; }
    public int UserId { get; private set ; }
    public decimal Balance { get; private set; } = 50000; // TODO SET THIS TO ZERO !! 

    public Wallet(int userId, decimal balance = 50000 )
    {
        UserId = userId;
        Balance = balance;
    }

    public void UpdateBalance(decimal balanceToAdd)
    {
        Balance += balanceToAdd;
    }
    
}