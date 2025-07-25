import { useMutation } from '@tanstack/react-query';
import {
  addEducation,
  deleteEducation,
  updateEducation,
} from '@/features/profile';
import type { Education } from '../types';

export function useAddEducation() {
  return useMutation({
    mutationFn: ({ education }: { education: Education }) =>
      addEducation(education),
    meta: {
      invalidatesQuery: [['educations'], ['user']],
      successMessage: 'Education added successfully',
      errorMessage: 'Failed to add education',
    },
  });
}

export function useUpdateEducation() {
  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: Education }) =>
      updateEducation(id, data),
    meta: {
      invalidatesQuery: [['educations'], ['user']],
      successMessage: 'Education updated successfully',
      errorMessage: 'Failed to update education',
    },
  });
}

export function useDeleteEducation() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteEducation(id),
    meta: {
      invalidatesQuery: [['educations'], ['user']],
      successMessage: 'Education deleted successfully',
      errorMessage: 'Failed to delete education',
    },
  });
}
