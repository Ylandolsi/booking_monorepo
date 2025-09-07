import { api } from '@/lib';
import * as Endpoints from '@/lib/api/user-endpoints';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { profileQueryKeys, type ExpertiseType } from '@/features/app/profile';

export async function allExpertises() {
  return await api.get<ExpertiseType[]>(Endpoints.GetAllExpertises);
}

export function useAllExpertises(
  overrides?: Partial<UseQueryOptions<any, Error, any>>,
) {
  return useQuery({
    queryKey: profileQueryKeys.allExpertises(),
    queryFn: allExpertises,
    staleTime: Infinity, // default values to select from
    refetchOnWindowFocus: false, // Don't refetch on window focus
    refetchOnMount: false, // Don't refetch on mount if data exists
    ...overrides,
  });
}
