import { authQueryKeys } from '@/features/auth';
import { api } from '@/lib';
import * as Endpoints from '@/lib/api/user-endpoints';
import { useMutation } from '@tanstack/react-query';

export const deleteExperience = async (experienceId: number) => {
  await api.delete<void>(
    Endpoints.DeleteExperience.replace('{experienceId}', String(experienceId)),
  );
};

export function useDeleteExperience() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteExperience(id),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Experience deleted successfully',
      errorMessage: 'Failed to delete experience',
    },
  });
}
