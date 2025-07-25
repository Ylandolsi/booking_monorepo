import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
import type { Expertise } from '../types';

export const updateExpertise = async (expertises: number[]) => {
  return await api.put<void>(Endpoints.UpdateUserExpertise, {
    expertiseIds: expertises,
  });
};
export const getExpertises = async (userSlug: string) => {
  return await api.get<Expertise[]>(
    Endpoints.GetUserExpertises.replace('{userSlug}', userSlug),
  );
};

export const allExpertises = async () => {
  return await api.get<Expertise[]>(Endpoints.GetAllExpertises);
};
