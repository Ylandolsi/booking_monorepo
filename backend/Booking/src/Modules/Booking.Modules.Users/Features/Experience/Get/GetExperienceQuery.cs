using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Experience.Get;

public sealed record GetExperienceQuery(string UserSlug) : IQuery<List<GetExperienceResponse>>;