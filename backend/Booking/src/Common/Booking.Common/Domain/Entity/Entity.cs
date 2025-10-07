namespace Booking.Common.Domain.Entity;

public abstract class Entity : IEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow!;
    public bool IsRemoved { get; set; } = false;
}