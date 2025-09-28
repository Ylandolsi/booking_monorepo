using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.Verification.VerifyEmail;

public record VerifyEmailCommand(string Email, string Token) : ICommand;