export const bookingQueryKeys = {
  all: () => ['booking'] as const,
  availability: (mentorSlug?: string) =>
    ['booking', 'availability', mentorSlug] as const,
  dailyAvailability: (mentorSlug?: string, date?: string) =>
    ['booking', 'availability', 'daily', mentorSlug, date] as const,
  monthlyAvailability: (mentorSlug?: string, year?: number, month?: number) =>
    ['booking', 'availability', 'monthly', mentorSlug, year, month] as const,
  sessions: () => ['booking', 'sessions'] as const,
  myBookings: () => ['booking', 'sessions', 'me'] as const,
  sessionDetails: (sessionId?: string) =>
    ['booking', 'sessions', sessionId] as const,
};
