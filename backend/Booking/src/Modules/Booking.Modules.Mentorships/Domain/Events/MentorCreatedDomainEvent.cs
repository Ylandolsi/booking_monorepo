using Booking.Common.Domain.DomainEvent;

namespace Booking.Modules.Mentorships.Domain.Events;

public sealed class MentorCreatedDomainEvent : DomainEvent
{
    public int MentorId { get; }
    public int UserId { get; }
    public decimal HourlyRate { get; }

    public MentorCreatedDomainEvent(int mentorId, int userId, decimal hourlyRate)
    {
        MentorId = mentorId;
        UserId = userId;
        HourlyRate = hourlyRate;
    }
}
