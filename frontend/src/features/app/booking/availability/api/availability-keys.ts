export const availabilityQueryKeys = {
  availability: (mentorSlug?: string) =>
    ['booking', 'availability', mentorSlug] as const,
  dailyAvailability: (mentorSlug?: string, date?: string) =>
    ['booking', 'availability', 'daily', mentorSlug, date] as const,
  monthlyAvailability: (mentorSlug?: string, year?: number, month?: number) =>
    ['booking', 'availability', 'monthly', mentorSlug, year, month] as const,
};
