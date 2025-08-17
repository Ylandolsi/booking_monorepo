import { api } from '@/lib';
import * as Endpoints from '@/lib/user-endpoints.ts';
import type { LanguageType } from '../../types';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { profileQueryKeys } from '@/features/app/profile';

export const allLanguages = async () => {
  return await api.get<Array<LanguageType>>(Endpoints.GetAllLanguages);
};

export function useAllLanguages(
  overrides?: Partial<UseQueryOptions<any, unknown, any>>,
) {
  return useQuery({
    queryKey: profileQueryKeys.allLanguages(),
    queryFn: allLanguages,
    ...overrides,
  });
}
