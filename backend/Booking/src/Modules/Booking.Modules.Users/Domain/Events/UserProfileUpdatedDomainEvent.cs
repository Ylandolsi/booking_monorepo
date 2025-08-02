using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Users.Domain.Events;

public sealed class UserProfileUpdatedDomainEvent : DomainEvent
{
    public int UserId { get; }

    public UserProfileUpdatedDomainEvent(int userId)
    {
        UserId = userId;
    }
}
