import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/api/utils/catalog-endpoints';
import { api } from '@/api/utils';
import { type CreateSessionProductRequest } from '@/api/stores';
import { logger } from '@/lib';

export const getMyProductSession = async (slug: string): Promise<CreateSessionProductRequest> => {
  try {
    const response = await api.get<CreateSessionProductRequest>(CatalogEndpoints.Products.Sessions.GetMy(slug));
    return response;
  } catch (error) {
    logger.error('Error fetching my store:', error);
    throw error;
  }
};
export function useMyProductSession(
  productSlug: string | undefined,
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<CreateSessionProductRequest, Error> {
  return useQuery(
    queryOptions({
      // TODO : add product slug to the key
      queryKey: ['my-product-session', productSlug],
      queryFn: () => getMyProductSession(productSlug!),
      ...overrides,
    }),
  );
}
