import { useMutation } from '@tanstack/react-query';
import {
  addExperience,
  deleteExperience,
  updateExperience,
  type ExperienceType,
} from '@/features/profile';

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

export function useUpdateExperience() {
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: ExperienceType }) =>
      updateExperience(id, data),
    meta: {
      invalidatesQuery: [['experiences'], ['users']],
      successMessage: 'Experience updated successfully',
      errorMessage: 'Failed to update experience',
    },
  });
}

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
