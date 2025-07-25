import type { Experience } from '@/features/profile';
import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';


export const getExperiences = async (userSlug: string) => {
  return await api.get<Array<Experience>>(
    Endpoints.GetUserExperiences.replace('{userSlug}', userSlug),
  );
};
export const addExperience = async (experience: Experience) => {
  return await api.post<number>(Endpoints.AddExperience, experience);
};
export const updateExperience = async (
  experienceId: Number,
  experience: Experience,
) => {
  await api.put<void>(
    Endpoints.UpdateExperience.replace('{experienceId}', String(experienceId)),
    experience,
  );
};
export const deleteExperience = async (experienceId: Number) => {
  await api.delete<void>(
    Endpoints.DeleteExperience.replace('{experienceId}', String(experienceId)),
  );
};
