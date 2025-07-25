import type { Expertise } from '@/features/profile';
import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';

export const updateExpertise = async (expertises: Expertise[]) => {
  return await api.put<void>(Endpoints.UpdateUserExpertise, expertises);
};
export const getExpertises = async (userSlug : string ) => {
  return await api.get<Expertise[]>(Endpoints.GetUserExpertises.replace('{userSlug}', userSlug));
};

export const allExpertises = async () => {
  return await api.get<Expertise[]>(Endpoints.GetAllExpertises);
};
