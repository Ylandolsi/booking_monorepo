using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.Me;

public record MeQuery(int Id) : IQuery<MeData>;

