import type { Education } from '@/features/profile';
import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
export const getEducations = async () => {
  return await api.get<Array<Education>>(Endpoints.GetUserEducations);
};
export const addEducation = async (education: Education) => {
  return await api.post<number>(Endpoints.AddEducation, education);
};
export const updateEducation = async (
  educationId: Number,
  education: Education,
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
