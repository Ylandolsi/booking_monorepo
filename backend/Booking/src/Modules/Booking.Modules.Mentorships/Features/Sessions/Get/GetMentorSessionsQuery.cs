using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

public sealed record GetMentorSessionsQuery(int MentorId) : IQuery<List<SessionResponse>>;
