import { useMutation } from '@tanstack/react-query';
import {
  addEducation,
  deleteEducation,
  updateEducation,
  type Education,
} from '@/features/profile';

export function useAddEducation() {
  return useMutation({
    mutationFn: ({ education }: { education: Education }) =>
      addEducation(education),
    meta: {
      invalidatesQuery: [['educations']],
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
      invalidatesQuery: [['educations']],
      successMessage: 'Education updated successfully',
      errorMessage: 'Failed to update education',
    },
  });
}

export function useDeleteEducation() {
  return useMutation({
    mutationFn: ({ id }: { id: number }) => deleteEducation(id),
    meta: {
      invalidatesQuery: [['educations']],
      successMessage: 'Education deleted successfully',
      errorMessage: 'Failed to delete education',
    },
  });
}
