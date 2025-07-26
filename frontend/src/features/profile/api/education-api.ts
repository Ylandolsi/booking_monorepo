import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
import type { EducationType } from '../types';

export const getEducations = async () => {
  return await api.get<Array<EducationType>>(Endpoints.GetUserEducations);
};
export const addEducation = async (education: EducationType) => {
  return await api.post<number>(Endpoints.AddEducation, education);
};
export const updateEducation = async (
  educationId: Number,
  education: EducationType,
) => {
  await api.put<void>(
    Endpoints.UpdateEducation.replace('{educationId}', String(educationId)),
    education,
  );
};
export const deleteEducation = async (educationId: Number) => {
  await api.delete<void>(
    Endpoints.DeleteEducation.replace('{educationId}', String(educationId)),
  );
};
