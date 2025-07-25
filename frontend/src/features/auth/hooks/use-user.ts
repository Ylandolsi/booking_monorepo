import { useQuery, queryOptions } from '@tanstack/react-query';
import { getCurrentUser } from '@/features/auth';

export const getUserQueryOptions = () => {
  return queryOptions({
    queryKey: ['user'],
    queryFn: getCurrentUser,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useUser = () => useQuery(getUserQueryOptions());
