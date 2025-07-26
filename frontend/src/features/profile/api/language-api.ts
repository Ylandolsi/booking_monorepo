import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
import type { LanguageType } from '../types';

export const updateLanguage = async (languageIds: number[]) => {
  return await api.put<void>(Endpoints.UpdateUserLanguages, { languageIds });
};

export const getLanguages = async (userSlug: string) => {
  return await api.get<Array<LanguageType>>(
    Endpoints.GetUserLanguages.replace('{userSlug}', userSlug),
  );
};

export const allLanguages = async () => {
  return await api.get<Array<LanguageType>>(Endpoints.GetAllLanguages);
};
