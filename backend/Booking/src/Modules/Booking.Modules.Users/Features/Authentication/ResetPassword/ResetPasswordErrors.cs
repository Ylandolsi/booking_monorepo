using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.ResetPassword;

public static class ResetPasswordErrors
{
    public static readonly Error GenericError =
        Error.Problem("ResetPassword.Failed", "The provided token or email is invalid");
}