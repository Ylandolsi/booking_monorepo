import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints';
import type { LanguageType } from '../../types';
import { useQuery } from '@tanstack/react-query';
import { profileQueryKeys } from '@/features/profile';

export const allLanguages = async () => {
  return await api.get<Array<LanguageType>>(Endpoints.GetAllLanguages);
};

export function useAllLanguages() {
  return useQuery({
    queryKey: profileQueryKeys.allLanguages(),
    queryFn: allLanguages,
  });
}
