using System.Security.Claims;
using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.Google.Integrate;

public record IntegrateAccountCommand(ClaimsPrincipal Principal, GoogleTokens GoogleTokens, int UserId) : ICommand;