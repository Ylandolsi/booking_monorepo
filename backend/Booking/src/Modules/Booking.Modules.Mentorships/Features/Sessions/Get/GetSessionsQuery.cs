using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

public sealed record GetSessionsQuery(int MenteeId , int DaysFromNow ) : IQuery<List<SessionResponse>>;
