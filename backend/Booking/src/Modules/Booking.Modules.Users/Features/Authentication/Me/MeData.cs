using Booking.Modules.Users.Domain.ValueObjects;

namespace Booking.Modules.Users.Features.Authentication.Me;

public record MeData(
    string Slug,
    string? FirstName,
    string? LastName,
    string? Email,
    string Gender,
    bool IntegratedWithGoogle,
    string? GoogleEmail,
    string KonnectWalletId,
    List<string> Roles
);