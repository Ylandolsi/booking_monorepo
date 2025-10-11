import { useQuery, queryOptions } from '@tanstack/react-query';
import { api, Endpoints } from '@/api/utils';
import { authQueryKeys } from '@/api/auth';
import { AuthEndpoints } from '../utils/auth-endpoints';

export const getCurrentUser = async (): Promise<User> => {
  const response = await api.get<User>(AuthEndpoints.User.Current);
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

export type User = {
  slug: string;
  firstName: string;
  lastName: string;
  email: string;
  gender: 'Male' | 'Female';
  integratedWithGoogle: boolean;
  googleEmail?: string | null;
  konnectWalletId?: string | null;
  roles: string[];
};
