using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication.ChangePassword;

public static class ChangePasswordErrors
{
    public static readonly Error PasswordIncorrect = Error.Problem(
        "ChangePassword.PasswordIncorrect", "The old password is not correct "
    );

    public static readonly Error PasswordsDoNotMatch = Error.Problem(
        "ChangePassword.PasswordsDoNotMatch",
        "The new password and confirmation password do not match.");

    public static Error ChangePasswordFailed(string errors)
    {
        return Error.Failure(
            "ChangePassword.Failed",
            $"Failed to change password: {errors}");
    }
}