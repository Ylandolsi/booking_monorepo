namespace Booking.Modules.Catalog.Domain.ValueObjects;

// TODO : document this in readme file 
public enum OrderStatus
{
    Pending, // Awaiting payment
    Paid, // Payment received
    Completed, // Escrow created and session confirmed
    Failed, // Payment failed
    Cancelled, // user cancelled
}

public enum ProductType
{
    Session,
    Course,
    DigitalProduct,
    Physical
}