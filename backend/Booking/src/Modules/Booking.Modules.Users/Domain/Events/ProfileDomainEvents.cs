using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Users.Domain.Events;

public sealed class ProfileCompletedDomainEvent : DomainEvent
{
    public int UserId { get; }

    public ProfileCompletedDomainEvent(int userId)
    {
        UserId = userId;
    }
}

public sealed class ProfileHalfCompletedDomainEvent : DomainEvent
{
    public int UserId { get; }

    public ProfileHalfCompletedDomainEvent(int userId)
    {
        UserId = userId;
    }
}

public sealed class UserBecameMentorDomainEvent : DomainEvent
{
    public int UserId { get; }

    public UserBecameMentorDomainEvent(int userId)
    {
        UserId = userId;
    }
}
