import { api } from '@/lib';
import * as Endpoints from '@/lib/user-endpoints.ts';
import { useMutation } from '@tanstack/react-query';

export const updateLanguage = async (languageIds: number[]) => {
  return await api.put<void>(Endpoints.UpdateUserLanguages, { languageIds });
};

export function useUpdateLanguages() {
  return useMutation({
    mutationFn: ({ languages }: { languages: number[] }) =>
      updateLanguage(languages),

    meta: {
      invalidatesQuery: [['languages']],
      successMessage: 'Languages updated successfully',
      errorMessage: 'Failed to update languages',
    },
  });
}
