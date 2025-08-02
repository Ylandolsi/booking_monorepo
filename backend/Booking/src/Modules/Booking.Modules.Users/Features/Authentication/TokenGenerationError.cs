using Booking.Common.Results;

namespace Booking.Modules.Users.Features.Authentication;

public static class TokenGenerationError
{
    public static readonly Error TokenGenerationFailed = Error.Failure(
        "Users.TokenGenerationFailed",
        "Failed to generate a token for the user. Please try again later.");
}