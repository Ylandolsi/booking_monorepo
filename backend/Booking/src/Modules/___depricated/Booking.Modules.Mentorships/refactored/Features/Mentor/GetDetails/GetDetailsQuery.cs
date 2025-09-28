namespace Booking.Modules.Mentorships.refactored.Features.Mentor.GetDetails;

public record GetDetailsQuery(string UserSlug) : IQuery<GetDetailsResponse>;