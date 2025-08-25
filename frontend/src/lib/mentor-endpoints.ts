export const MentorshipEndpoints = {
  Base: 'mentorships',

  Availability: {
    Set: 'mentorships/availability',
    SetBulk: 'mentorships/availability/bulk',
    Remove: 'mentorships/availability/{availabilityId}',
    Update: 'mentorships/availability/{availabilityId}',
    ToggleAvailability: 'mentorships/availability/{availabilityId}/toggle',
    ToggleDay: 'mentorships/availability/day/toggle', // Query: dayOfWeek
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

  Relationships: {
    Request: 'mentorships/relationships',
    Accept: 'mentorships/relationships/{relationshipId}/accept',
    Reject: 'mentorships/relationships/{relationshipId}/reject',
    End: 'mentorships/relationships/{relationshipId}/end',
    GetMine: 'mentorships/relationships/me',
    GetMentorRelationships: 'mentorships/relationships/mentor',
  },

  Reviews: {
    Submit: 'mentorships/reviews',
    GetMentorReviews: 'entorships/reviews/{userSlug}',
    // GetDetails: "mentorships/reviews/{reviewId}",
  },
};
