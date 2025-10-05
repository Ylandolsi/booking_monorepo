import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints, QueryBuilders } from '@/api/utils/catalog-endpoints';
import { api, buildUrlWithParams } from '@/api/utils';

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

export const getOrders = async ({ startsAt, endsAt }: { startsAt?: Date; endsAt?: Date }): Promise<OrderResponse[]> => {
  try {
    const queryParams = QueryBuilders.Orders.getOrders(startsAt, endsAt);
    const urlEndpoint = buildUrlWithParams(CatalogEndpoints.Orders.GetOrders, queryParams);

    const response = await api.get<OrderResponse[]>(urlEndpoint);

    return response;
  } catch (error) {
    console.error('Error fetching orders:', error);
    throw error;
  }
};

export function useGetOrders(
  { startsAt, endsAt }: { startsAt?: Date; endsAt?: Date } = {},
  overrides?: Partial<UseQueryOptions<OrderResponse[], Error>>,
): UseQueryResult<OrderResponse[], Error> {
  return useQuery(
    queryOptions({
      queryKey: ['orders', { startsAt: startsAt?.toISOString(), endsAt: endsAt?.toISOString() }],
      queryFn: () => getOrders({ startsAt, endsAt }),
      ...overrides,
    }),
  );
}
