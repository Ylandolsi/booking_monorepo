import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import type { StoreResponse } from '@/types/catalog-api';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api } from '@/lib/api/api-client';
import { storeKeys } from '@/api/stores/stores-keys';

export const getMyStore = async (): Promise<StoreResponse> => {
  try {
    const response = await api.get<StoreResponse>(CatalogEndpoints.Stores.GetMy);
    return response;
  } catch (error) {
    console.error('Error fetching my store:', error);
    throw error;
  }
};
export function useMyStore(overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<StoreResponse, Error> {
  return useQuery(
    queryOptions({
      queryKey: storeKeys.myStore(),
      queryFn: () => getMyStore(),
      ...overrides,
    }),
  );
}
