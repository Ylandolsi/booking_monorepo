import { storeKeys } from '@/api/stores/stores-keys';
import { api } from '@/lib';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { useQuery } from '@tanstack/react-query';

export interface SlugAvailabilityResponse {
  isAvailable: boolean;
  slug: string;
}

export const checkSlugAvailability = async (slug: string): Promise<SlugAvailabilityResponse> => {
  try {
    const response = await api.get<SlugAvailabilityResponse>(CatalogEndpoints.Stores.CheckSlugAvailability(slug));
    return response;
  } catch (error) {
    console.error(`Error checking slug availability for ${slug}:`, error);
    throw error;
  }
};

export function useCheckSlugAvailability(slug: string, enabled = true) {
  return useQuery({
    queryKey: storeKeys.slugAvailability(slug),
    queryFn: () => checkSlugAvailability(slug),
    enabled: enabled && !!slug,
    staleTime: 30000, // 30 seconds
    retry: false, // Don't retry slug availability checks
  });
}
