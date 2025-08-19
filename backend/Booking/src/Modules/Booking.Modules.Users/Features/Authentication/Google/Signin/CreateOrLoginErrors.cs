using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.Google;

public static class CreateOrLoginErrors
{
    public static Error UserRegistrationFailed(string message) => Error.Failure("UserRegistrationFailed", message);
    public static Error UserIntegrationFailed(string message ) => Error.Failure("UserIntegrationFailed", message);
}