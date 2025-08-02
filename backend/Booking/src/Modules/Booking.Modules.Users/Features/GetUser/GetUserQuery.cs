using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.GetUser;

public record GetUserQuery(string UserSlug) : IQuery<UserResponse>;