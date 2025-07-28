import { api } from '@/lib';
import type { User } from '@/types/api';
import * as Endpoints from '@/lib/endpoints';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { profileQueryKeys } from '@/features/profile';

export const userInfo = async (userSlug: string) => {
  if (userSlug === undefined || userSlug === '') {
    throw new Error('userSlug is required');
  }
  return await api.get<User>(Endpoints.GetUser.replace('{userSlug}', userSlug));
};

export function useProfile(
  userSlug: string,
  overrides?: Partial<UseQueryOptions<any, unknown, any>>,
) {
  return useQuery({
    queryKey: profileQueryKeys.slug(userSlug),
    queryFn: () => userInfo(userSlug),
    enabled: !!userSlug,
    ...overrides,
  });
}
