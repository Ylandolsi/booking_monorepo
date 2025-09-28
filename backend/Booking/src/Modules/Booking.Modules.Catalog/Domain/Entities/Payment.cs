using System.ComponentModel.DataAnnotations.Schema;
using Booking.Common.Domain.Entity;

namespace Booking.Modules.Catalog.Domain.Entities;

public class Payment : Entity
{
    private Payment()
    {
    }

    public Payment(
        int orderId,
        int storeId,
        int productId,
        string reference,
        decimal price,
        PaymentStatus status)
    {
        Reference = reference;
        OrderId = orderId;
        ProductId = productId;
        StoreId = storeId;
        Price = price;
        Status = status;
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; private set; }

    public int StoreId { get; private set; }
    public string Reference { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public decimal Price { get; }
    public PaymentStatus Status { get; private set; }

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
    Completed
}