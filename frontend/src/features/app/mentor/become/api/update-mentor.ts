import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import type { Mentor } from '@/features/app/mentor/become/types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { mentorQueryKeys } from '@/features/app/mentor/become/api';
import { useUser } from '@/features/auth';

export const updateMentor = async (
  mentor: Omit<Mentor, 'createdAt' | 'konnectWalletId'>,
) => {
  return await api.put<void>(MentorshipEndpoints.Mentors.UpdateProfile, mentor);
};

export function useUpdateMentor() {
  const { data: user } = useUser();
  return useMutation({
    mutationFn: (mentor: Omit<Mentor, 'createdAt' | 'konnectWalletId'>) =>
      updateMentor(mentor),
    meta: {
      // TODO : invalidate mentor-data-slug key

      invalidatesQuery: [mentorQueryKeys.mentorProfile(user?.slug)],
      successMessage: 'Successfully updated profile',
      errorMessage: 'Failed to update profile',
    },
  });
}
