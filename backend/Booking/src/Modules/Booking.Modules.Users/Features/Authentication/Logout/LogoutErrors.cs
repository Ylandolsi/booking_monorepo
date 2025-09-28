using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.Logout;

public static class LogoutErrors
{
    public static readonly Error NoActiveSession =
        Error.Problem("No.Refresh.Token", "No active session found for the user.");
}