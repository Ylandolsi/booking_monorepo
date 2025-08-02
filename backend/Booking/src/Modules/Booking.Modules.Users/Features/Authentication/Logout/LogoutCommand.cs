using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.Logout;


public sealed record LogoutCommand(int UserId) : ICommand<bool>;

