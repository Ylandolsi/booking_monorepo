import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints, QueryBuilders } from '@/api/utils/catalog-endpoints';
import { api, buildUrlWithParams } from '@/api/utils';
import type { PaginatedResult } from '@/types';

export interface OrderResponse {
  id: number;
  storeId: number;
  storeSlug: string;
  customerEmail: string;
  customerName: string;
  customerPhone?: string;
  productId: number;
  productType: 'Session' | 'Product';
  amount: number;
  amountPaid: number;
  status: 'Pending' | 'Paid' | 'Completed' | 'Failed' | 'Cancelled';
  paymentRef?: string;
  scheduledAt?: string;
  sessionEndTime?: string;
  timeZoneId?: string;
  note?: string;
  completedAt?: string;
  createdAt: string;
  updatedAt: string;
}

export const getOrders = async ({
  startsAt,
  endsAt,
  page,
  limit,
}: {
  startsAt?: Date;
  endsAt?: Date;
  page?: number;
  limit?: number;
}): Promise<PaginatedResult<OrderResponse>> => {
  try {
    const queryParams = QueryBuilders.Orders.getOrders(startsAt, endsAt, page, limit);
    const urlEndpoint = buildUrlWithParams(CatalogEndpoints.Orders.GetOrders, queryParams);

    const response = await api.get<PaginatedResult<OrderResponse>>(urlEndpoint);

    return response;
  } catch (error) {
    console.error('Error fetching orders:', error);
    throw error;
  }
};

export function useGetOrders(
  { startsAt, endsAt, page, limit }: { startsAt?: Date; endsAt?: Date; page?: number; limit?: number } = {},
  overrides?: Partial<UseQueryOptions<PaginatedResult<OrderResponse>, Error>>,
): UseQueryResult<PaginatedResult<OrderResponse>, Error> {
  return useQuery(
    queryOptions({
      queryKey: ['orders', { startsAt: startsAt?.toISOString(), endsAt: endsAt?.toISOString(), page, limit }],
      queryFn: () => getOrders({ startsAt, endsAt, page, limit }),
      ...overrides,
    }),
  );
}
