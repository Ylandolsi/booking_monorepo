export const availabilityQueryKeys = {
  monthlyAvailability: (mentorSlug?: string, year?: number, month?: number) =>
    ['booking', 'availability', 'monthly', mentorSlug, year, month] as const,
};
