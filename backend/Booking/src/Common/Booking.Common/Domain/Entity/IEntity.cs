
namespace Booking.Common.Domain.Entity;

public interface IEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}