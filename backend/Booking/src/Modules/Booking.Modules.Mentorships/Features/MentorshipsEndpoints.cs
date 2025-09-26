namespace Booking.Modules.Mentorships.Features;

public static class MentorshipEndpoints
{
    private const string Base = "/mentorships";

    public static class Availability
    {
        public const string SetBulk = $"{Base}/availability/bulk";
        public const string GetDaily = $"{Base}/availability"; // Query: mentorSlug, date
        public const string GetMonthly = $"{Base}/availability/month";
        public const string GetSchedule = $"{Base}/availability/schedule";
    }

    public static class Sessions
    {
        public const string Book = $"{Base}/sessions";

        /*
        public const string Cancel = $"{Base}/sessions/{{sessionId}}/cancel";
        public const string GetDetails = $"{Base}/sessions/{{sessionId}}";
        */
        public const string GetSessions = $"{Base}/sessions"; // 
    }

    public static class Payouts
    {
        public const string Payout = $"{Base}/payout";
        public const string PayoutHistory = $"{Base}/payout";

        public static class Admin
        {
            public const string ApprovePayout = $"{Base}/admin/payout/approve"; // body : payoutId
            public const string WebhookPayout = $"{Base}/admin/payout/webhook";
            public const string RejectPayout = $"{Base}/admin/payout/reject"; // body : payoutId 
            public const string GetAllPayouts = $"{Base}/admin/payout"; // query : status (Pending, Approved, Rejected , Completed) , upToDate , timeZoneId
            // public const string GetDetailled = $"{Base}/admin/payout/{{payoutId}}";
        }
    }


    public static class Payment
    {
        public const string Create = $"{Base}/payments";
        public const string GetWallet = $"{Base}/payments/wallet";
        public const string Webhook = $"{Base}/payments/webhook"; //  payment_ref=5f9498735289e405fc7c18ac
    }


    public static class Mentors
    {
        public const string Become = $"{Base}/mentors/become";
        public const string UpdateProfile = $"{Base}/mentors/profile";

        public const string GetProfile = $"{Base}/mentors/{{userSlug}}";
        //public const string Search = $"{Base}/mentors/search";
    }


    public static class Reviews
    {
        public const string Submit = $"{Base}/reviews";

        public const string GetMentorReviews = $"{Base}/reviews/{{userSlug}}";
        //public const string GetDetails = $"{Base}/reviews/{{reviewId}}";
    }


    /*public static class Relationships
    {
        public const string Request = $"{Base}/relationships";
        public const string Accept = $"{Base}/relationships/{{relationshipId}}/accept";
        public const string Reject = $"{Base}/relationships/{{relationshipId}}/reject";
        public const string End = $"{Base}/relationships/{{relationshipId}}/end";
        public const string GetMine = $"{Base}/relationships/me";
        public const string GetMentorRelationships = $"{Base}/relationships/mentor";
    }*/
}