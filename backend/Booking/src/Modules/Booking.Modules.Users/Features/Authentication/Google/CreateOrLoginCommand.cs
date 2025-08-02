using System.Security.Claims;
using Booking.Common.Messaging;
using Booking.Modules.Users.Features.Utils;

namespace Booking.Modules.Users.Features.Authentication.Google;

public record CreateOrLoginCommand(ClaimsPrincipal principal) : ICommand<LoginResponse>;