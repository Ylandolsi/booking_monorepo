import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api } from '@/lib/api/api-client';
import { type CreateSessionProductRequest } from '@/api/stores';

export const getMyProductSession = async (slug: string): Promise<CreateSessionProductRequest> => {
  try {
    const response = await api.get<CreateSessionProductRequest>(CatalogEndpoints.Products.Sessions.GetMy(slug));
    return response;
  } catch (error) {
    console.error('Error fetching my store:', error);
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
      queryKey: [],
      queryFn: () => getMyProductSession(productSlug!),
      ...overrides,
    }),
  );
}
