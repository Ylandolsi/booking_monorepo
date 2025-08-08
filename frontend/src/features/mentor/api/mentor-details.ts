import { api } from '@/lib';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import type { Mentor } from '@/features/mentor/types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { mentorQueryKeys } from '@/features/mentor/api/';

export const mentorDetails = async (userSlug: string) => {
  if (userSlug === undefined || userSlug === '') {
    throw new Error('userSlug is required');
  }
  return await api.get<Mentor>(MentorshipEndpoints.Mentors.GetProfile.replace('{mentorSlug}', userSlug));
};

export function useMentorDetails (
  userSlug: string,
  overrides?: Partial<UseQueryOptions<any, unknown, any>>,
) {
  return useQuery({
    queryKey: mentorQueryKeys.mentorProfile(userSlug),
    queryFn: () => mentorDetails(userSlug),
    enabled: !!userSlug,
    ...overrides,
  });
}
