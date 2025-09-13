namespace Booking.Modules.Catalog.Domain.ValueObjects;

public enum OrderStatus
{
    Pending,
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
