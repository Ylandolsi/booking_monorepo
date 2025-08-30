import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import type { Mentor } from '@/features/app/mentor/become/types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { mentorQueryKeys } from '@/features/app/mentor/become/api';
import { useUser } from '@/features/auth';

export const becomeMentor = async (mentor: Omit<Mentor, 'createdAt'>) => {
  return await api.post<void>(MentorshipEndpoints.Mentors.Become, mentor);
};

export function useBecomeMentor() {
  const { data: user } = useUser();
  return useMutation({
    mutationFn: ({ mentor }: { mentor: Omit<Mentor, 'createdAt'> }) =>
      becomeMentor(mentor),
    meta: {
      invalidatesQuery: [mentorQueryKeys.mentorProfile(user?.slug)],
      successMessage: 'Successfully become mentor',
      // errorMessage: 'Failed to become mentor',
    },
  });
}
