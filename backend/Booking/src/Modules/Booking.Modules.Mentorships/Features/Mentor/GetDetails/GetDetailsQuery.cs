using Booking.Common.Messaging;

namespace Booking.Modules.Mentorships.Features.Mentor.GetDetails;

public record GetDetailsQuery(string UserSlug) : IQuery<GetDetailsResponse>;