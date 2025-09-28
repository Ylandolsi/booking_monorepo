using Booking.Modules.Mentorships.refactored.Domain.Entities.Sessions;

namespace Booking.Modules.Mentorships.refactored.Domain.Entities;

// TODO :we can add mentor and mentee id here for fast access 
public class Escrow : Entity
{
    public int Id { get; private set; }
    
    public decimal Price { get; private set; }
    public EscrowState State { get; private set; }
    
    public int SessionId { get; private set; }
    public Session Session { get; private set; }

    private Escrow()
    {
        
    }
    public Escrow(decimal price, int sessionId )
    {
        Price = price;
        SessionId = sessionId;
        State = EscrowState.Held; 
    }

    
    public void SetRefunded()
    {
        State = EscrowState.Refunded;
    }

    public void Realese()
    {
        State = EscrowState.Released; 
    }
}

public enum EscrowState
{
    Held , 
    Released ,
    Refunded , 
}
