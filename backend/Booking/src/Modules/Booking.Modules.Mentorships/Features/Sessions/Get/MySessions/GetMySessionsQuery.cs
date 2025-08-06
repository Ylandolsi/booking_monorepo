using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

public sealed record GetMySessionsQuery(int MenteeId) : IQuery<List<SessionResponse>>;
