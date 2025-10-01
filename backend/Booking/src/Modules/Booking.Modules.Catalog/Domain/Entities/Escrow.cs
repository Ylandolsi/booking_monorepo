using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Escrow : Entity
{
    private Escrow()
    {
    }

    public Escrow(decimal price, int orderId)
    {
        Price = price;
        OrderId = orderId;
        State = EscrowState.Held;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public decimal Price { get; private set; }
    public EscrowState State { get; private set; }

    public int OrderId { get; private set; }

    public DateTime ReleaseAt { get; private set; }

    public Order Order { get; private set; }

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
    Held,
    Released,
    Refunded
}