import { authQueryKeys } from '@/features/auth';
import { api } from '@/lib';
import * as Endpoints from '@/lib/api/user-endpoints';
import { useMutation } from '@tanstack/react-query';

export const updateExpertise = async (expertises: number[]) => {
  return await api.put<void>(Endpoints.UpdateUserExpertise, {
    expertiseIds: expertises,
  });
};

export function useUpdateExpertises() {
  return useMutation({
    mutationFn: ({ expertises }: { expertises: number[] }) =>
      updateExpertise(expertises),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Expertises updated successfully',
      errorMessage: 'Failed to update expertises',
    },
  });
}
