using Booking.Modules.Mentorships.Domain.Enums;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

public sealed record SessionResponse(
    int Id,
    int MentorId,
    string MentorName,
    int MenteeId,
    string MenteeName,
    DateTime ScheduledAt,
    int DurationMinutes,
    decimal Price,
    string Note,
    SessionStatus Status,
    string? GoogleMeetLink,
    DateTime CreatedAt);
