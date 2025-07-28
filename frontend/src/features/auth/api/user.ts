import { useQuery, queryOptions } from '@tanstack/react-query';
import type { User } from '@/types/api';
import { api, Endpoints } from '@/lib';
import { authQueryKeys } from '@/features/auth';

export const getCurrentUser = async (): Promise<User> => {
  const response = await api.get<User>(Endpoints.GetCurrentUser);
  return response;
};

export const getUserQueryOptions = () => {
  return queryOptions({
    queryKey: authQueryKeys.currentUser(),
    queryFn: getCurrentUser,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useUser = () => useQuery(getUserQueryOptions());
