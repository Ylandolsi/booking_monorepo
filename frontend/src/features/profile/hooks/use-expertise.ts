import { useMutation, useQuery } from '@tanstack/react-query';
import {
  allExpertises,
  updateExpertise,
  type Expertise,
} from '@/features/profile';

export function useAllExpertises() {
  return useQuery({
    queryKey: ['all-expertises'],
    queryFn: allExpertises,
  });
}

export function useUpdateExpertises() {
  return useMutation({
    mutationFn: ({ expertises }: { expertises: Expertise[] }) =>
      updateExpertise(expertises),
    meta: {
      invalidatesQuery: [['expertises']],
      successMessage: 'Expertises updated successfully',
      errorMessage: 'Failed to update expertises',
    },
  });
}
