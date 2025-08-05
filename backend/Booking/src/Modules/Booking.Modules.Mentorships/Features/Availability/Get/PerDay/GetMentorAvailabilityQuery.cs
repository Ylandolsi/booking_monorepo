using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Availability.Get;

public sealed record GetMentorAvailabilityQuery(string MentorSlug) : IQuery<List<AvailabilityResponse>>;
