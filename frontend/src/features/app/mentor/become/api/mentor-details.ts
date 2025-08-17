import { api } from '@/lib';
import {
  queryOptions,
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import type { Mentor } from '@/features/app/mentor/become/types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { mentorQueryKeys } from '@/features/app/mentor/become/api';

export const mentorDetails = async (
  userSlug: string | undefined | null,
): Promise<Mentor> => {
  if (userSlug === undefined || userSlug === '') {
    throw new Error('userSlug is required');
  }
  return await api.get<Mentor>(
    MentorshipEndpoints.Mentors.GetProfile.replace('{mentorSlug}', userSlug!),
  );
};

export function useMentorDetails(
  userSlug?: string | null,
  overrides?: Partial<UseQueryOptions<any, unknown>>,
): UseQueryResult<Mentor, unknown> {
  return useQuery(
    queryOptions({
      queryKey: mentorQueryKeys.mentorProfile(userSlug),
      queryFn: () => mentorDetails(userSlug),
      enabled: !!userSlug,
      ...overrides,
    }),
  );
}
