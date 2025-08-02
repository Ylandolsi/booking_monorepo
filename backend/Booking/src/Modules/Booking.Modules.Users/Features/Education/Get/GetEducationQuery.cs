using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Education.Get;
public sealed record GetEducationQuery(string UserSlug) : IQuery<List<GetEducationResponse>>;

