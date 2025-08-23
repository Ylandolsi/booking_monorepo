using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

public sealed record GetSessionsQuery(
    int MenteeId, 
    string? UpToDate , 
    string TimeZoneId) : IQuery<List<SessionResponse>>;