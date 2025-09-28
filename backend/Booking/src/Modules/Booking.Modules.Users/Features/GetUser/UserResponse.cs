using Booking.Modules.Users.Domain.ValueObjects;

namespace Booking.Modules.Users.Features.GetUser;

// mentor and mentee retrieve them manually 
// TODO : ADD completetion status 
public record UserResponse
{
    public string Slug { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Status Status { get; init; }
    public ProfilePicture ProfilePicture { get; init; }
    public string Gender { get; init; }
    public SocialLinks SocialLinks { get; init; }
    public string Bio { get; init; }
    public string TimeZoneId { get; init; }
    public List<Domain.Entities.Experience> Experiences { get; init; } = new();
    public List<Domain.Entities.Education> Educations { get; init; } = new();
    public List<Domain.Entities.Expertise> Expertises { get; init; } = new();
    public List<Domain.Entities.Language> Languages { get; init; } = new();
}