export const MentorshipEndpoints = {
  Base: 'mentorships',

  Availability: {
    SetBulk: 'mentorships/availability/bulk',
    GetDaily: 'mentorships/availability', // Query: mentorSlug, date
    GetMonthly: 'mentorships/availability/month', // Query: mentorSlug, year, month
    GetSchedule: 'mentorships/availability/schedule',
  },

  Sessions: {
    Book: 'mentorships/sessions',
    GetSessions: 'mentorships/sessions', // Query : daysFromNow  , timeZoneId
    // Cancel: 'mentorships/sessions/{sessionId}/cancel',
  },

  Mentors: {
    Become: 'mentorships/mentors/become',
    UpdateProfile: 'mentorships/mentors/profile',
    GetProfile: 'mentorships/mentors/{mentorSlug}',
    // Search: "mentorships/mentors/search",
  },

  Payouts: {
    Payout: 'mentorships/payout',
    PayoutHistory: 'mentorships/payout',

    Admin: {
      ApprovePayout: 'mentorships/admin/payout/approve', // body : payoutId
      WebhookPayout: 'mentorships/admin/payout/webhook',
      RejectPayout: 'mentorships/admin/payout/reject', // body : payoutId
      GetAllPayouts: 'mentorships/admin/payout', // query : status (Pending, Approved, Rejected , Completed) , upToDate , timeZoneId
      // GetDetailled: 'mentorships/admin/payout/{payoutId}',
    },
  },

  Payment: {
    GetWallet: 'mentorships/payments/wallet',
    //Create: 'mentorships/payments',
    //Webhook: 'mentorships/payments/webhook', // payment_ref=5f9498735289e405fc7c18ac
  },

  // Relationships: {
  //   Request: 'mentorships/relationships',
  //   Accept: 'mentorships/relationships/{relationshipId}/accept',
  //   Reject: 'mentorships/relationships/{relationshipId}/reject',
  //   End: 'mentorships/relationships/{relationshipId}/end',
  //   GetMine: 'mentorships/relationships/me',
  //   GetMentorRelationships: 'mentorships/relationships/mentor',
  // },

  // Reviews: {
  //   Submit: 'mentorships/reviews',
  //   GetMentorReviews: 'entorships/reviews/{userSlug}',
  //   // GetDetails: "mentorships/reviews/{reviewId}",
  // },
};
