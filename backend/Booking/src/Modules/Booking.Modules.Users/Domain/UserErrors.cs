using Booking.Common.Results;
using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Domain;

public static class UserErrors
{
    public static Error NotFoundById(int userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error NotFoundByEmail(string email) => Error.NotFound(
        "Users.NotFoundByEmail",
        $"The user with the email = '{email}' was not found");

    public static Error NotFoundBySlug(string slug) => Error.NotFound(
        "Users.NotFoundBySlug",
        $"The user with the slug = '{slug}' was not found");


    public static readonly Error EmailIsNotVerified = Error.Problem(
        "Users.EmailIsNotVerified",
        "The email address is not verified. Please verify your email before proceeding.");

    public static readonly Error IncorrectEmailOrPassword = Error.Problem(
    "Users.IncorrectEmailOrPassword",
    "The provided email or password is incorrect. Please try again.");

    public static readonly Error AccountLockedOut = Error.Unauthorized(
        "Users.AccountLockedOut",
        "This account has been locked out due to too many failed login attempts. Please try again later.");

    public static Error Unauthorized() => Error.Unauthorized(
    "Users.Unauthorized",
    "You are not authorized to perform this action.");

    public static readonly Error ExpertiseLimitExceeded =
    Error.Problem("User.ExpertiseLimitExceeded",
        $"User expertises should not exceed {UserConstraints.MaxExpertises}");

    public static readonly Error LanguageLimitExceeded =
    Error.Problem("User.LanguageLimitExceeded",
        $"User Languages should not exceed {UserConstraints.MaxLanguages}");

    // New validation errors
    public static readonly Error InvalidGender = Error.Problem(
        "User.InvalidGender",
        $"Gender must be one of: {string.Join(", ", Genders.ValidGenders)}");

    public static readonly Error BioTooLong = Error.Problem(
        "User.BioTooLong",
        $"Bio cannot exceed {UserConstraints.MaxBioLength} characters");

    public static readonly Error InvalidSocialLinks = Error.Problem(
        "User.InvalidSocialLinks",
        "Social links cannot be null");

    public static readonly Error ExpertiseAlreadyExists = Error.Problem(
        "User.ExpertiseAlreadyExists",
        "User already has this expertise");

    public static readonly Error ExpertiseNotFound = Error.Problem(
        "User.ExpertiseNotFound",
        "User does not have this expertise");

    public static readonly Error LanguageAlreadyExists = Error.Problem(
        "User.LanguageAlreadyExists",
        "User already has this language");

    public static readonly Error LanguageNotFound = Error.Problem(
        "User.LanguageNotFound",
        "User does not have this language");
}
