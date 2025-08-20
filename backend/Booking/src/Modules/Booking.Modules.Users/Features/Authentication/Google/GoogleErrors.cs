using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.Google;

public static class GoogleErrors
{
    public static Error EmailAlreadyTaken(string message) => Error.Failure("EmailAlreadyTaken", message);
    public static Error UserRegistrationFailed(string message) => Error.Failure("UserRegistrationFailed", message);
    public static Error UserIntegrationFailed(string message) => Error.Failure("UserIntegrationFailed", message);
}