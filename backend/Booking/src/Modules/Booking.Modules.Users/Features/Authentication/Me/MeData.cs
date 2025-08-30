using Booking.Modules.Users.Domain.ValueObjects;

namespace Booking.Modules.Users.Features.Authentication.Me;

public record MeData(
    string Slug,
    string? FirstName,
    string? LastName,
    string? Email,
    Status Status,
    ProfilePicture ProfilePicture,
    string Gender,
    SocialLinks SocialLinks,
    string Bio,
    List<Domain.Entities.Experience> Experiences,
    List<Domain.Entities.Education> Educations,
    List<Domain.Entities.Expertise> Expertises,
    List<Domain.Entities.Language> Languages,
    ProfileCompletionStatus ProfileCompletionStatus,
    bool IntegratedWithGoogle,
    string? GoogleEmail,
    string KonnectWalletId
);