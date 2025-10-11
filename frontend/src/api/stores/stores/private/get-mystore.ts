import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/api/utils/catalog-endpoints';
import { api } from '@/api/utils';
import { storeKeys, type Store } from '@/api/stores';
import { logger } from '@/lib';

export const getMyStore = async (): Promise<Store> => {
  try {
    const response = await api.get<Store>(CatalogEndpoints.Stores.GetMy);
    return response;
  } catch (error) {
    logger.error('Error fetching my store:', error);
    throw error;
  }
};
export function useMyStore(overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<Store, Error> {
  return useQuery(
    queryOptions({
      // TODO : handle this queryKey: storeKeys.myStore(),
      queryKey: storeKeys.all,
      queryFn: () => getMyStore(),
      ...overrides,
    }),
  );
}
