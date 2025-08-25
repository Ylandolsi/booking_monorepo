import { api, type RequestOptions } from '@/lib';
import type { Session } from '@/features/app/session/get/types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { sessionQueryKeys } from '@/features/app/session/get/api/sessions-get-keys';

export const getSessions = async (upToDate?: string, timeZoneId?: string) => {
  return await api.get<Array<Session>>(
    MentorshipEndpoints.Sessions.GetSessions,
    {
      params: { upToDate, timeZoneId },
    } as RequestOptions,
  );
};

export function useGetSessions(
  upToDate?: Date,
  timeZoneId?: string,
  overrides?: Partial<UseQueryOptions<Array<Session>, unknown, Array<Session>>>,
) {
  const normalizedUpToDate = upToDate
    ? new Date(upToDate).toISOString()
    : undefined;

  const dateOnly = normalizedUpToDate?.slice(0, 10); // used for cache

  return useQuery<Array<Session>, unknown, Array<Session>>({
    queryKey: sessionQueryKeys.session(dateOnly, timeZoneId),
    queryFn: () => getSessions(normalizedUpToDate, timeZoneId),
    ...overrides,
  });
}
