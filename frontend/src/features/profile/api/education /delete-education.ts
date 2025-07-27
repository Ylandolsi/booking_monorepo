import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';

export const deleteEducation = async (educationId: number) => {
  await api.delete<void>(
    Endpoints.DeleteEducation.replace('{educationId}', String(educationId)),
  );
};

export function useDeleteEducation() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteEducation(id),
    meta: {
      invalidatesQuery: [['educations'], ['user']],
      successMessage: 'Education deleted successfully',
      errorMessage: 'Failed to delete education',
    },
  });
}
