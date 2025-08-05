using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

public sealed record GetMenteeSessionsQuery(int MenteeId) : IQuery<List<SessionResponse>>;
