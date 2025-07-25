import { useMutation } from '@tanstack/react-query';
import {
  addExperience,
  deleteExperience,
  updateExperience,
} from '@/features/profile';
import type { Experience } from '../types';

export function useAddExperience() {
  return useMutation({
    mutationFn: ({ experience }: { experience: Experience }) =>
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
    mutationFn: ({ id, data }: { id: number; data: Experience }) =>
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
