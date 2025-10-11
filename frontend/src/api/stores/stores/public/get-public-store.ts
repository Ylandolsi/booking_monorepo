import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/api/utils/catalog-endpoints';
import { api } from '@/api/utils';
import type { Store } from '@/api/stores';
import { logger } from '@/lib';

export const GetPublicStore = async ({ storeSlug }: { storeSlug: string }): Promise<Store> => {
  try {
    const response = await api.get<Store>(CatalogEndpoints.Stores.GetPublic(storeSlug));
    return response;
  } catch (error) {
    logger.error(`Error fetching store with slug ${storeSlug}:`, error);
    throw error;
  }
};
export function usePublicStore(storeSlug: string, overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<Store, Error> {
  return useQuery(
    queryOptions({
      queryKey: ['public-store', storeSlug],
      queryFn: () => GetPublicStore({ storeSlug }),
      ...overrides,
    }),
  );
}
