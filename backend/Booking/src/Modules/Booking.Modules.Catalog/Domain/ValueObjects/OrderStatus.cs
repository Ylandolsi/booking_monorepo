namespace Booking.Modules.Catalog.Domain.ValueObjects;

public enum OrderStatus
{
    Pending,
    Paid,
    Completed,
    Failed,
    Cancelled
}

public enum ProductType
{
    Session,
    Course,
    DigitalProduct,
    Physical
}