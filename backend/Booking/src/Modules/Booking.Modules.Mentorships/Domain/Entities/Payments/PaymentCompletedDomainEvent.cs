using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Mentorships.Domain.Entities.Payments;

public class PaymentCompletedDomainEvent : DomainEvent
{
    public int MenteeId { get; }
    public int MentorId { get; }
    public int SessionId { get; }
    public decimal Price { get; }

    public PaymentCompletedDomainEvent(int menteeId, int mentorId, int sessionId, decimal price)
    {
        MenteeId = menteeId;
        MentorId = mentorId;
        SessionId = sessionId;
        Price = price;
    }
}