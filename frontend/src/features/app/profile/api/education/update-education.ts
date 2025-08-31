import { api } from '@/lib';
import * as Endpoints from '@/lib/user-endpoints.ts';
import type { EducationType } from '@/features/app/profile';
import { useMutation } from '@tanstack/react-query';
import { authQueryKeys } from '@/features/auth';

export const updateEducation = async (
  educationId: number,
  education: EducationType,
) => {
  await api.put<void>(
    Endpoints.UpdateEducation.replace('{educationId}', String(educationId)),
    education,
  );
};

export function useUpdateEducation() {
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: EducationType }) =>
      updateEducation(id, data),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Education updated successfully',
      errorMessage: 'Failed to update education',
    },
  });
}
