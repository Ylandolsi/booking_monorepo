namespace Booking.Modules.Mentorships.Features.__later.Reviews.Get;

public sealed record ReviewResponse(
    int Id,
    int SessionId,
    int MentorId,
    int MenteeId,
    string MenteeName,
    int Rating,
    string Comment,
    DateTime CreatedAt);
