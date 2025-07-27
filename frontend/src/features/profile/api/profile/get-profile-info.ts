import { api } from '@/lib';
import type { User } from '@/types/api';
import * as Endpoints from '@/lib/endpoints';
import { useQuery } from '@tanstack/react-query';

export const userInfo = async (userSlug: string) => {
  if (userSlug === undefined || userSlug === '') {
    throw new Error('userSlug is required');
  }
  return await api.get<User>(Endpoints.GetUser.replace('{userSlug}', userSlug));
};

export function useProfile(userSlug: string) {
  return useQuery({
    queryKey: ['profile', userSlug],
    queryFn: () => userInfo(userSlug),
    enabled: !!userSlug,
  });
}
