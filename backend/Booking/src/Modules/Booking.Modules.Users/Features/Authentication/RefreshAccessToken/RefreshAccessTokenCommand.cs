using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.RefreshAccessToken; 
public record RefreshAccessTokenCommand(string RefreshToken) : ICommand;
