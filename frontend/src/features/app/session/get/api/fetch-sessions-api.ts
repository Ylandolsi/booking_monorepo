import { api, type RequestOptions } from '@/lib';
import type { Session } from '@/features/app/session/get/types';
import { MentorshipEndpoints } from '@/lib/api/mentor-endpoints';
import {
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
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
  overrides?: Partial<UseQueryOptions<Array<Session>, Error>>,
): UseQueryResult<Array<Session>, Error> {
  const normalizedUpToDate = upToDate
    ? new Date(upToDate).toISOString() // TODO : change it to toLocalISOString(upToDate) : undefined;
    : undefined;

  const dateOnly = normalizedUpToDate?.slice(0, 10); // used for cache
  console.log(dateOnly);

  const options: UseQueryOptions<Array<Session>, Error> = {
    queryKey: sessionQueryKeys.session(dateOnly, timeZoneId),
    queryFn: () => getSessions(normalizedUpToDate, timeZoneId),
    ...overrides,
  };

  return useQuery<Array<Session>, Error>(options);
}
