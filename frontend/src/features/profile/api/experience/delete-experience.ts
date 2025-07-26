import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints';
import { useMutation } from '@tanstack/react-query';

export const deleteExperience = async (experienceId: Number) => {
  await api.delete<void>(
    Endpoints.DeleteExperience.replace('{experienceId}', String(experienceId)),
  );
};

export function useDeleteExperience() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteExperience(id),
    meta: {
      invalidatesQuery: [['experiences'], ['users']],
      successMessage: 'Experience deleted successfully',
      errorMessage: 'Failed to delete experience',
    },
  });
}
