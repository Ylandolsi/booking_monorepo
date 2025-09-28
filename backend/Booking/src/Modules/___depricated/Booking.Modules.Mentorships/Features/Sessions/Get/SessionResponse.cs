using Booking.Modules.Mentorships.Domain.Enums;

namespace Booking.Modules.Mentorships.Features.Sessions.Get;

// TODO : change id to slug !! 
public sealed record SessionResponse
{
    public int Id { get; init; }
    public int MentorId { get; init; }
    public string? MentorFirstName { get; set; }
    public string? MentorLastName { get; set; }

    public string? MentorEmail { get; set; }
    public string? MentorProfilePicture { get; set; }
    public string? MentorProfilePictureBlurry { get; set; }
    public int MenteeId { get; init; }
    public string? MenteeFirstName { get; set; }
    public string? MenteeLastName { get; set; }

    public string? MenteeEmail { get; set; }
    public string? MenteeProfilePicture { get; set; }
    public string? MenteeProfilePictureBlurry { get; set; }
    public decimal Price { get; set; }
    public SessionStatus Status { get; set; }
    public string? GoogleMeetLink { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationInMinutes { get; set; }
    public DateTime UpdatedAt { get; set; }

    public DateTime? CompletedAt { get;  set; }

    public DateTime? CancelledAt { get;  set; }
    public DateTime CreatedAt { get; set; }

    public bool IamMentor { get; set; }
}