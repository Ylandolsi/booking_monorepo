export const bookingQueryKeys = {
  sessions: () => ['booking', 'sessions'] as const,
  myBookings: () => ['booking', 'sessions', 'me'] as const,
  sessionDetails: (sessionId?: string) =>
    ['booking', 'sessions', sessionId] as const,
};
