using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Booking.Common.Domain.DomainEvent;

namespace Booking.Common.Domain.Entity;

public abstract class Entity : IEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow!;

    private readonly List<IDomainEvent> _domainEvents = [];

    [NotMapped] // for ef core 
    [JsonIgnore] // even though it wont get mapped , we need to ignore it for serialization
    public List<IDomainEvent> DomainEvents => [.. _domainEvents];


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
