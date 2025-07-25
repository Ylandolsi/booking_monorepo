import type { Language } from '@/features/profile';
import { api } from '@/lib';
import * as Endpoints from '@/lib/endpoints.ts';
export const updateLanguage = async (languages: Language[]) => {
  return await api.put<void>(Endpoints.UpdateUserLanguages, languages);
};
export const getLanguages = async ( userSlug : string ) => {
  return await api.get<Array<Language>>(Endpoints.GetUserLanguages.replace('{userSlug}', userSlug));
};

export const allLanguages = async () => {
  return await api.get<Array<Language>>(Endpoints.GetAllLanguages);
};
