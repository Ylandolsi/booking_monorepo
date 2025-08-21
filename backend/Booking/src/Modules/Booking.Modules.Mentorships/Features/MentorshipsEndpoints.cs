namespace Booking.Modules.Mentorships.Features;

public static class MentorshipEndpoints
{
    private const string Base = "/mentorships";
    
    public static class Availability
    {
        public const string Set = $"{Base}/availability";
        public const string SetBulk = $"{Base}/availability/bulk"; 
        public const string Remove = $"{Base}/availability/{{availabilityId}}";
        public const string Update = $"{Base}/availability/{{availabilityId}}";
        public const string ToggleAvailability = $"{Base}/availability/{{availabilityId}}/toggle"; 
        public const string ToggleDay = $"{Base}/availability/day/toggle"; // Query: dayOfWeek
        public const string GetDaily = $"{Base}/availability"; // Query: mentorSlug, date
        public const string GetMonthly = $"{Base}/availability/month"; // Query: mentorSlug, year, month
        public const string GetSchedule = $"{Base}/availability/schedule";
        
    }
    
    public static class Sessions
    {
        public const string Book = $"{Base}/sessions";
        public const string Cancel = $"{Base}/sessions/{{sessionId}}/cancel";
        public const string GetDetails = $"{Base}/sessions/{{sessionId}}";
        public const string GetSessions = $"{Base}/sessions/me";
    }
    
    public static class Relationships
    {
        public const string Request = $"{Base}/relationships";
        public const string Accept = $"{Base}/relationships/{{relationshipId}}/accept";
        public const string Reject = $"{Base}/relationships/{{relationshipId}}/reject";
        public const string End = $"{Base}/relationships/{{relationshipId}}/end";
        public const string GetMine = $"{Base}/relationships/me";
        public const string GetMentorRelationships = $"{Base}/relationships/mentor";
    }
    
    public static class Reviews
    {
        public const string Submit = $"{Base}/reviews";
        public const string GetMentorReviews = $"{Base}/reviews/{{userSlug}}";
        //public const string GetDetails = $"{Base}/reviews/{{reviewId}}";
    }
    
    public static class Mentors
    {
        public const string Become = $"{Base}/mentors/become";
        public const string UpdateProfile = $"{Base}/mentors/profile";
        public const string GetProfile = $"{Base}/mentors/{{userSlug}}";
        //public const string Search = $"{Base}/mentors/search";
    }
}