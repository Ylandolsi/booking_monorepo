import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import type { Mentor } from '@/features/mentor/types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';

export const becomeMentor = async (mentor: Mentor) => {
  return await api.put<void>(MentorshipEndpoints.Mentors.Become, mentor);
};

export function useBecomeMentor() {
  return useMutation({
    mutationFn: ({ mentor }: { mentor: Mentor }) => becomeMentor(mentor),
    meta: {
      //invalidatesQuery: [['languages']],
      successMessage: 'Successfully become mentor',
      errorMessage: 'Failed to become mentor',
    },
  });
}
