namespace Booking.Modules.Users.Features.Utils;

public sealed record LoginResponse(
    string UserSlug,
    string FirstName,
    string LastName,
    string Email);