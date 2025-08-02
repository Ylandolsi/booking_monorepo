using Booking.Common.Domain.DomainEvent;

namespace Booking.Common.Domain.Entity;


public interface IEntity
{
  List<IDomainEvent> DomainEvents { get; }
  void ClearDomainEvents();
  void Raise(IDomainEvent domainEvent);

}
