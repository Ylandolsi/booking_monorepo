using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.Google;

public static class GoogleErrors
{
    public static Error EmailAlreadyTaken(string message)
    {
        return Error.Failure("EmailAlreadyTaken", message);
    }

    public static Error UserRegistrationFailed(string message)
    {
        return Error.Failure("UserRegistrationFailed", message);
    }

    public static Error UserIntegrationFailed(string message)
    {
        return Error.Failure("UserIntegrationFailed", message);
    }
}