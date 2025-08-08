export const mentorQueryKeys = {
  all: () => ['profile'] as const,
  mentorProfile: (userSlug: string) => ['mentor', userSlug] as const,
  allLanguages: () => ['all-languages'] as const,
  allExpertises: () => ['all-expertises'] as const,

  //   basicInfo: () => [...profileQueryKeys.all(), 'basic-info'] as const,
  //   socialLinks: () => [...profileQueryKeys.all(), 'social-links'] as const,
  //   picture: () => [...profileQueryKeys.all(), 'picture'] as const,
};
