using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Mentorships.Domain.Events;

public sealed class SessionCompletedDomainEvent : DomainEvent
{
    public int SessionId { get; }
    public int MentorId { get; }
    public int MenteeId { get; }
    public DateTime CompletedAt { get; }
    public decimal Price { get; }

    public SessionCompletedDomainEvent(int sessionId, int mentorId, int menteeId, DateTime completedAt, decimal price)
    {
        SessionId = sessionId;
        MentorId = mentorId;
        MenteeId = menteeId;
        CompletedAt = completedAt;
        Price = price;
    }
}
