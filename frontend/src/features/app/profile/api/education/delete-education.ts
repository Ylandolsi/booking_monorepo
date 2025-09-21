import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib';
import * as Endpoints from '@/api/auth/auth-endpoints';
import { authQueryKeys } from '@/api/auth';

export const deleteEducation = async (educationId: number) => {
  await api.delete<void>(Endpoints.DeleteEducation.replace('{educationId}', String(educationId)));
};

export function useDeleteEducation() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteEducation(id),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Education deleted successfully',
      errorMessage: 'Failed to delete education',
    },
  });
}
