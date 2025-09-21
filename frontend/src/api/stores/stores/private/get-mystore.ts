import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { getMyStore } from '../stores-api';
import { storeKeys } from '../../stores-keys';
import type { StoreResponse } from '@/types/catalog-api';

// Re-export for backward compatibility
export { getMyStore };

export function useMyStore(overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<StoreResponse, Error> {
  return useQuery(
    queryOptions({
      queryKey: storeKeys.myStore(),
      queryFn: () => getMyStore(),
      ...overrides,
    }),
  );
}
