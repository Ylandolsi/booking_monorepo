using Booking.Modules.Users.Domain.ValueObjects;

namespace Booking.Modules.Users.Features.GetUser;

// mentor and mentee retrieve them manually 
// TODO : ADD completetion status 
public record UserResponse
{
    public string Slug { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Gender { get; init; }
    public string TimeZoneId { get; init; }
}