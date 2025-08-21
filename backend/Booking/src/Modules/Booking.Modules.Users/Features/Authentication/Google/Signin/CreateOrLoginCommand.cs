using System.Security.Claims;
using Booking.Common.Messaging;
using Booking.Modules.Users.Contracts;
using Booking.Modules.Users.Features.Utils;

namespace Booking.Modules.Users.Features.Authentication.Google.Signin;

public record CreateOrLoginCommand(ClaimsPrincipal Principal , GoogleTokens GoogleTokens) : ICommand<LoginResponse>;