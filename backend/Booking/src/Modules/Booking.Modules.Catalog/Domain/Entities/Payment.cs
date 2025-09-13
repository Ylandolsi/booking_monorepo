using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Payment : Entity
{
    // TODO : maybe add ProductId ? 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public string Reference { get; private set; }
    public int UserId { get; private set; }
    public int OrderId { get; private set; }
    public decimal Price { get; private set; }
    public PaymentStatus Status { get; private set; }

    private Payment()
    {
    }

    public Payment(int userId, int orderId, int storeId , string reference, decimal price, PaymentStatus status)
    {
        Reference = reference;
        OrderId = orderId;
        UserId = userId;
        StoreId = storeId;
        Price = price;
        Status = status;
    }

    public void SetComplete(decimal? price = null)
    {
        price = price ?? Price;
        Status = PaymentStatus.Completed;
    }

    public void UpdateReference(string reference)
    {
        Reference = reference;
    }
}

public enum PaymentStatus
{
    Pending,
    Completed,
}