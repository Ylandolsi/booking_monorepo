namespace Booking.Modules.Mentorships.Features;

public static class MentorshipsEndpoints
{
    // Mentor Profile Management
    public const string BecomeMentor = "mentorships/become-mentor";
    public const string UpdateMentorProfile = "mentorships/mentor/profile";
    public const string DeactivateMentor = "mentorships/mentor/deactivate";
    public const string GetMentorProfile = "mentorships/mentor/{mentorSlug}";

    // Availability Management
    public const string SetAvailability = "mentorships/availability";
    public const string UpdateAvailability = "mentorships/availability/{availabilityId}";
    public const string DeleteAvailability = "mentorships/availability/{availabilityId}";
    public const string GetMentorAvailability = "mentorships/availability/{mentorSlug}";

    // Session Management
    public const string BookSession = "mentorships/sessions/book";
    public const string CancelSession = "mentorships/sessions/{sessionId}/cancel";
    public const string ConfirmSession = "mentorships/sessions/{sessionId}/confirm";
    public const string GetSession = "mentorships/sessions/{sessionId}";
    public const string GetMentorSessions = "mentorships/sessions/mentor";
    public const string GetMenteeSessions = "mentorships/sessions/mentee";

    // Review Management
    public const string AddReview = "mentorships/reviews";
    public const string UpdateReview = "mentorships/reviews/{reviewId}";
    public const string DeleteReview = "mentorships/reviews/{reviewId}";
    public const string GetMentorReviews = "mentorships/reviews/{mentorSlug}";

    // Mentorship Relationship
    public const string RequestMentorship = "mentorships/relationships/request";
    public const string AcceptMentorship = "mentorships/relationships/{relationshipId}/accept";
    public const string RejectMentorship = "mentorships/relationships/{relationshipId}/reject";
    public const string EndMentorship = "mentorships/relationships/{relationshipId}/end";
    public const string GetMentorshipRelationships = "mentorships/relationships";
}
