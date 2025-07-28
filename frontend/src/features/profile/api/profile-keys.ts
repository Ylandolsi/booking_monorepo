export const profileQueryKeys = {
  all: () => ['profile'] as const,
  slug: (userSlug: string) => [...profileQueryKeys.all(), userSlug] as const,
  allLanguages: () => ['all-languages'] as const,
  allExpertises: () => ['all-expertises'] as const,

  //   basicInfo: () => [...profileQueryKeys.all(), 'basic-info'] as const,
  //   socialLinks: () => [...profileQueryKeys.all(), 'social-links'] as const,
  //   picture: () => [...profileQueryKeys.all(), 'picture'] as const,
};
