import { api, CatalogEndpoints } from '@/api/utils';
import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';

export type StatsType = 'revenue' | 'visitors' | 'customers' | 'all';

export type ChartDataPoint = {
  date: string;
  revenue: number;
  sales: number;
  customers: number;
  visitors: number;
};

export type ProductDataPoint = {
  productSlug: string;
  name: string;
  sales: number;
  revenue: number;
};

export type StatsTotals = {
  revenue: number;
  sales: number;
  customers: number;
  visitors: number;
  averageRevenue: number;
  averageSales: number;
  conversionRate: string;
};

export type StatsResponse = {
  chartData: ChartDataPoint[];
  productData: ProductDataPoint[];
  totals: StatsTotals;
};

export type GetStatsParams = {
  type?: StatsType;
  startsAt?: string; // ISO date string
  endsAt?: string; // ISO date string
};

async function getStats(params?: GetStatsParams): Promise<StatsResponse> {
  const queryParams: Record<string, string | undefined> = {
    type: params?.type,
    startsAt: params?.startsAt,
    endsAt: params?.endsAt,
  };

  const res = await api.get<StatsResponse>(CatalogEndpoints.Statistics.GetStats, {
    params: queryParams,
  });
  return res;
}

export const StatsKeys = {
  all: () => ['stats'] as const,
  filtered: (params?: GetStatsParams) => ['stats', params] as const,
};

export function useGetStats(
  params?: GetStatsParams,
  overrides?: Partial<UseQueryOptions<StatsResponse, Error>>,
): UseQueryResult<StatsResponse, Error> {
  return useQuery<StatsResponse, Error>(
    queryOptions({
      queryFn: () => getStats(params),
      queryKey: StatsKeys.filtered(params),
      ...overrides,
    }),
  );
}
