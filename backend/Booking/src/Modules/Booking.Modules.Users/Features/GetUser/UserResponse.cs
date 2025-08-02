using Booking.Modules.Users.Domain.ValueObjects;

namespace Booking.Modules.Users.Features.GetUser;

// mentor and mentee retrieve them manually 
public record UserResponse(
    string Slug,
    string FirstName,
    string LastName,
    Status Status,
    ProfilePicture ProfilePicture,
    string Gender,
    SocialLinks SocialLinks,
    string Bio,
    List<Domain.Entities.Experience> Experiences,
    List<Domain.Entities.Education> Educations,
    List<Domain.Entities.Expertise> Expertises,
    List<Domain.Entities.Language> Languages);