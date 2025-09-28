using Booking.Common.Messaging;
using Booking.Modules.Users.Features.Utils;

namespace Booking.Modules.Users.Features.Authentication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;