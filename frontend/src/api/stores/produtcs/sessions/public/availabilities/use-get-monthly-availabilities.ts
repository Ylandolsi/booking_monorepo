import type { MonthAvailabilityType } from '@/api/stores/produtcs';
import { api, buildUrlWithParams, CatalogEndpoints, QueryBuilders } from '@/lib';
import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';

export const getMonthlyAvailability = async (productSlug: string, year: number, month: number): Promise<MonthAvailabilityType> => {
  if (!productSlug || !year || !month) {
    throw new Error('productSlug, year, and month are required');
  }
  // TODO : add timeZoneId param
  // by default it will be 'Africa/Tunis'
  // later we can get the user's timezone from the browser using Intl.DateTimeFormat().resolvedOptions().timeZone
  // and pass it as a param to get the availability in the user's timezone
  // for now we will keep it simple and use the default timezone

  const queryParams = QueryBuilders.Products.Sessions.monthlyAvailability(year, month); // timeZoneId is optional and defaults to 'Africa/Tunis'
  const urlEndpoint = buildUrlWithParams(CatalogEndpoints.Products.Sessions.GetMonthlyAvailability(productSlug), queryParams);

  return await api.get<MonthAvailabilityType>(urlEndpoint);
};

export function useMonthlyAvailability(
  productSlug?: string,
  year?: number,
  month?: number,
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<MonthAvailabilityType, Error> {
  return useQuery(
    queryOptions({
      // queryKey: availabilityQueryKeys.monthlyAvailability(productSlug, year, month),
      queryKey: ['monthly-availability', productSlug, year, month],
      queryFn: () => getMonthlyAvailability(productSlug!, year!, month!),
      enabled: !!productSlug && !!year && !!month,
      ...overrides,
    }),
  );
}
