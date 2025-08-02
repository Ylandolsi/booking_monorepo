namespace Booking.Common.Domain.DomainEvent;

public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
