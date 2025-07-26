import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints';
import type { ExperienceType } from '../../types';
import { useMutation } from '@tanstack/react-query';

export const addExperience = async (experience: ExperienceType) => {
  return await api.post<number>(Endpoints.AddExperience, experience);
};

export function useAddExperience() {
  return useMutation({
    mutationFn: ({ experience }: { experience: ExperienceType }) =>
      addExperience(experience),
    meta: {
      invalidatesQuery: [['experiences'], ['users']],
      successMessage: 'Experience added successfully',
      errorMessage: 'Failed to add experience',
    },
  });
}
