import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
import type { EducationType } from '@/features/profile';
import { useMutation } from '@tanstack/react-query';
import { authQueryKeys } from '@/features/auth';

export const addEducation = async (education: EducationType) => {
  return await api.post<number>(Endpoints.AddEducation, education);
};

export function useAddEducation() {
  return useMutation({
    mutationFn: ({ education }: { education: EducationType }) =>
      addEducation(education),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Education added successfully',
      errorMessage: 'Failed to add education',
    },
  });
}
