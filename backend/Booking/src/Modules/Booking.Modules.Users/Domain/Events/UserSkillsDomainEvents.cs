using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Users.Domain.Events;

public sealed class UserExpertiseAddedDomainEvent : DomainEvent
{
    public int UserId { get; }
    public int ExpertiseId { get; }

    public UserExpertiseAddedDomainEvent(int userId, int expertiseId)
    {
        UserId = userId;
        ExpertiseId = expertiseId;
    }
}

public sealed class UserExpertiseRemovedDomainEvent : DomainEvent
{
    public int UserId { get; }
    public int ExpertiseId { get; }

    public UserExpertiseRemovedDomainEvent(int userId, int expertiseId)
    {
        UserId = userId;
        ExpertiseId = expertiseId;
    }
}

public sealed class UserLanguageAddedDomainEvent : DomainEvent
{
    public int UserId { get; }
    public int LanguageId { get; }

    public UserLanguageAddedDomainEvent(int userId, int languageId)
    {
        UserId = userId;
        LanguageId = languageId;
    }
}

public sealed class UserLanguageRemovedDomainEvent : DomainEvent
{
    public int UserId { get; }
    public int LanguageId { get; }

    public UserLanguageRemovedDomainEvent(int userId, int languageId)
    {
        UserId = userId;
        LanguageId = languageId;
    }
}
