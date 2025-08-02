using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Expertise.Get;

public record GetUserExpertisesQuery(string UserSlug) : IQuery<List<ExpertiseResponse>>;