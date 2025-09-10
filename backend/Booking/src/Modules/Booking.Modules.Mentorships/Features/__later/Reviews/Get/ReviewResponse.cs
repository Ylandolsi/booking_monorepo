namespace Booking.Modules.Mentorships.Features.Reviews.Get;

public sealed record ReviewResponse(
    int Id,
    int SessionId,
    int MentorId,
    int MenteeId,
    string MenteeName,
    int Rating,
    string Comment,
    DateTime CreatedAt);
