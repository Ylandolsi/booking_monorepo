export const mentorQueryKeys = {
  all: () => ['profile'] as const,
  mentorProfile: (userSlug?: string | null) =>
    ['mentor', 'details', userSlug] as const,
};
