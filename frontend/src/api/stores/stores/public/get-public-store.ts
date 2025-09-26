import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api } from '@/lib/api/api-client';
import type { Store } from '@/api/stores';

export const GetPublicStore = async ({ storeSlug }: { storeSlug: string }): Promise<Store> => {
  try {
    const response = await api.get<Store>(CatalogEndpoints.Stores.GetPublic(storeSlug));
    return response;
  } catch (error) {
    console.error(`Error fetching store with slug ${storeSlug}:`, error);
    throw error;
  }
};
export function usePublicStore(storeSlug: string, overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<Store, Error> {
  return useQuery(
    queryOptions({
      queryKey: [],
      queryFn: () => GetPublicStore({ storeSlug }),
      ...overrides,
    }),
  );
}
