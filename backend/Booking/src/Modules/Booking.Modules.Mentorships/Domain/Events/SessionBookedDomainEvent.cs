using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Mentorships.Domain.Events;

public sealed class SessionBookedDomainEvent : DomainEvent
{
    public int SessionId { get; }
    public int MentorId { get; }
    public int MenteeId { get; }
    public DateTime ScheduledAt { get; }
    public decimal Price { get; }

    public SessionBookedDomainEvent(int sessionId, int mentorId, int menteeId, DateTime scheduledAt, decimal price)
    {
        SessionId = sessionId;
        MentorId = mentorId;
        MenteeId = menteeId;
        ScheduledAt = scheduledAt;
        Price = price;
    }
}
