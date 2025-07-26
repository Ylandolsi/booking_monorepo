import { useMutation, useQuery } from '@tanstack/react-query';
import {
  allLanguages,
  updateLanguage,
  type LanguageType,
} from '@/features/profile';

export function useAllLanguages() {
  return useQuery({
    queryKey: ['all-languages'],
    queryFn: allLanguages,
  });
}

export function useUpdateLanguages() {
  return useMutation({
    mutationFn: ({ languages }: { languages: LanguageType[] }) =>
      updateLanguage(languages),
    meta: {
      invalidatesQuery: [['languages']],
      successMessage: 'Languages updated successfully',
      errorMessage: 'Failed to update languages',
    },
  });
}
