using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain;

public static class UserErrors
{
    public static readonly Error EmailIsNotVerified = Error.Problem(
        "Users.EmailIsNotVerified",
        "The email address is not verified. Please verify your email before proceeding.");

    public static readonly Error IncorrectEmailOrPassword = Error.Problem(
        "Users.IncorrectEmailOrPassword",
        "The provided email or password is incorrect. Please try again.");

    public static readonly Error AccountLockedOut = Error.Unauthorized(
        "Users.AccountLockedOut",
        "This account has been locked out due to too many failed login attempts. Please try again later.");
    

    // New validation errors
    public static readonly Error InvalidGender = Error.Problem(
        "User.InvalidGender",
        $"Gender must be one of: {string.Join(", ", Genders.ValidGenders)}");

    public static Error NotFoundById(int userId)
    {
        return Error.NotFound(
            "Users.NotFound",
            $"The user with the Id = '{userId}' was not found");
    }

    public static Error InvalidTimeZone(string timezone)
    {
        return Error.Problem("Invalid.TimeZone", $"Invalid timezone '{timezone}' ");
    }

    public static Error NotFoundByEmail(string email)
    {
        return Error.NotFound(
            "Users.NotFoundByEmail",
            $"The user with the email = '{email}' was not found");
    }

    public static Error NotFoundBySlug(string slug)
    {
        return Error.NotFound(
            "Users.NotFoundBySlug",
            $"The user with the slug = '{slug}' was not found");
    }

    public static Error Unauthorized()
    {
        return Error.Unauthorized(
            "Users.Unauthorized",
            "You are not authorized to perform this action.");
    }
}