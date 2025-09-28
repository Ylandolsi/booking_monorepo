namespace Booking.Modules.Mentorships.refactored.Features.Sessions.Get;

public sealed record GetSessionsQuery(
    int MenteeId, 
    string? UpToDate , 
    string TimeZoneId) : IQuery<List<SessionResponse>>;