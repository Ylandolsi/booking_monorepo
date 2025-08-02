using Booking.Common.Messaging;

namespace Booking.Modules.Users.Features.Authentication.Verification.ReSendVerification;

public sealed record ReSendVerificationCommand(string Email)
    : ICommand;
