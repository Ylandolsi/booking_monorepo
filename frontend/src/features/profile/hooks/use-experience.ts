import { useMutation } from '@tanstack/react-query';
import {
  addExperience,
  deleteExperience,
  updateExperience,
  type Experience,
} from '@/features/profile';

export function useAddExperience() {
  return useMutation({
    mutationFn: ({ experience }: { experience: Experience }) =>
      addExperience(experience),
    meta: {
      invalidatesQuery: [['experiences']],
      successMessage: 'Experience added successfully',
      errorMessage: 'Failed to add experience',
    },
  });
}

export function useUpdateExperience() {
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: Experience }) =>
      updateExperience(id, data),
    meta: {
      invalidatesQuery: [['experiences']],
      successMessage: 'Experience updated successfully',
      errorMessage: 'Failed to update experience',
    },
  });
}

export function useDeleteExperience() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteExperience(id),
    meta: {
      invalidatesQuery: [['experiences']],
      successMessage: 'Experience deleted successfully',
      errorMessage: 'Failed to delete experience',
    },
  });
}
