import { useMutation, useQuery } from '@tanstack/react-query';
import { allExpertises, updateExpertise } from '@/features/profile';

export function useAllExpertises() {
  return useQuery({
    queryKey: ['all-expertises'],
    queryFn: allExpertises,
    staleTime: 240 * 60 * 1000, // 4 hours
    gcTime: 240 * 60 * 1000, // 4 hours (previously cacheTime)
    refetchOnWindowFocus: false, // Don't refetch on window focus
    refetchOnMount: false, // Don't refetch on mount if data exists
  });
}

export function useUpdateExpertises() {
  return useMutation({
    mutationFn: ({ expertises }: { expertises: number[] }) =>
      updateExpertise(expertises),
    meta: {
      invalidatesQuery: [['user']],
      successMessage: 'Expertises updated successfully',
      errorMessage: 'Failed to update expertises',
    },
  });
}
