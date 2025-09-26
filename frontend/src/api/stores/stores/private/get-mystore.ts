import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api } from '@/lib/api/api-client';
import type { Store } from '@/api/stores';

export const getMyStore = async (): Promise<Store> => {
  try {
    const response = await api.get<Store>(CatalogEndpoints.Stores.GetMy);
    return response;
  } catch (error) {
    console.error('Error fetching my store:', error);
    throw error;
  }
};
export function useMyStore(overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<Store, Error> {
  return useQuery(
    queryOptions({
      // TODO : handle this queryKey: storeKeys.myStore(),
      queryKey: [],
      queryFn: () => getMyStore(),
      ...overrides,
    }),
  );
}
