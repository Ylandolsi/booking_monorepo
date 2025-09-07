import { api } from '@/lib';
import type { User } from '@/types/api';
import * as Endpoints from '@/lib/api/user-endpoints';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { profileQueryKeys } from '@/features/app/profile';
import { useQueryState } from '@/hooks';

export const userInfo = async (userSlug?: string) => {
  if (userSlug === undefined || userSlug === '') {
    throw new Error('userSlug is required');
  }
  return await api.get<User>(Endpoints.GetUser.replace('{userSlug}', userSlug));
};

export function useProfile(
  userSlug?: string,
  overrides?: Partial<UseQueryOptions<User, Error, User>>,
) {
  return useQuery({
    queryKey: profileQueryKeys.slug(userSlug!),
    queryFn: () => userInfo(userSlug),
    enabled: !!userSlug,
    ...overrides,
  });
}
