using Booking.Modules.Mentorships.Domain.Enums;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

// TODO : change id to slug !! 
public sealed record SessionResponse
{
    public int Id { get; init; }
    public int MentorId { get; init;  }
    public string? MentorFirstName { get; set; }
    public string? MentorLastName { get; set; }

    public string? MentorEmail { get; set; }
    public string? MentorProfilePicture { get; set; }
    public string? MentorProfilePictureBlurry { get; set; }
    public decimal Price { get; set; }
    public SessionStatus Status { get; set; }
    public string? GoogleMeetLink { get; set; }
    public DateTime CreatedAt { get; set; }
}

