using Booking.Common.Domain.Entity;

namespace Booking.Modules.Mentorships.Domain.Entities.Payments;

public class Payment : Entity
{
    public int Id { get; private set; }
    public string Reference { get; private set; }

    public int UserId { get; private set; }
    public int MentorId { get; private set; }
    public int SessionId { get; private set; }
    public decimal Price { get; private set; }
    public PaymentStatus Status { get; private set; }

    private Payment()
    {
    }

    public Payment(int userId, string reference, decimal price, int sessionId, int mentorId, PaymentStatus status)
    {
        Reference = reference;
        UserId = userId;
        MentorId = mentorId;
        SessionId = sessionId;
        Price = price;
        Status = status;
    }

    public void SetComplete( decimal? price = null)
    {
        price = price ?? Price;
        Status = PaymentStatus.Completed;
        Raise(new PaymentCompletedDomainEvent(UserId, MentorId, SessionId, Price));
    }

    public void UpdateReference(string reference)
    {
        Reference = reference;
    }
}

public enum PaymentStatus
{
    Pending,
    Completed,
}