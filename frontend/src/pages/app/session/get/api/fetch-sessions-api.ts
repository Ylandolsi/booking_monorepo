import { api, CatalogEndpoints, type RequestOptions } from '@/api/utils';
import type { Session } from '@/pages/app/session/get/types';
import { useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { sessionQueryKeys } from '@/pages/app/session/get/api/sessions-get-keys';
import { toLocalISOString } from '@/utils';

export const getSessions = async (upToDate?: string, timeZoneId?: string) => {
  return await api.get<Array<Session>>(CatalogEndpoints.Products.Sessions.GetSessions, {
    params: { upToDate, timeZoneId },
  } as RequestOptions);
};

export function useGetSessions(
  upToDate?: Date,
  timeZoneId?: string,
  overrides?: Partial<UseQueryOptions<Array<Session>, Error>>,
): UseQueryResult<Array<Session>, Error> {
  const normalizedUpToDate = upToDate ? toLocalISOString(upToDate) : undefined; // c# dateTIme

  const dateOnly = normalizedUpToDate?.slice(0, 13); // used for cache (removed minutes and seconds  )
  console.log(dateOnly);

  const options: UseQueryOptions<Array<Session>, Error> = {
    queryKey: sessionQueryKeys.session(dateOnly, timeZoneId),
    queryFn: () => getSessions(normalizedUpToDate, timeZoneId),
    ...overrides,
  };

  return useQuery<Array<Session>, Error>(options);
}
