import { api } from '@/lib';
import * as Endpoints from '@/lib/user-endpoints.ts';
import type { ExperienceType } from '../../types';
import { useMutation } from '@tanstack/react-query';
import { authQueryKeys } from '@/features/auth';

export const updateExperience = async (
  experienceId: number,
  experience: ExperienceType,
) => {
  await api.put<void>(
    Endpoints.UpdateExperience.replace('{experienceId}', String(experienceId)),
    experience,
  );
};

export function useUpdateExperience() {
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: ExperienceType }) =>
      updateExperience(id, data),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Experience updated successfully',
      errorMessage: 'Failed to update experience',
    },
  });
}
