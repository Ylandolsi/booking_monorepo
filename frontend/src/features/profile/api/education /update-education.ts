import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
import type { EducationType } from '@/features/profile';
import { useMutation } from '@tanstack/react-query';

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
      invalidatesQuery: [['user']],
      successMessage: 'Education updated successfully',
      errorMessage: 'Failed to update education',
    },
  });
}
