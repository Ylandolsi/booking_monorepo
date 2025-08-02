using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Users.Domain;

public sealed class UserRegisteredDomainEvent : DomainEvent
{
    public int UserId { get; }

    public UserRegisteredDomainEvent(int userId)
    {
        UserId = userId;
    }
}