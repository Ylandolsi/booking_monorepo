import { useQuery } from '@tanstack/react-query';
import { checkSlugAvailability } from '../stores-api';
import { storeKeys } from '../../stores-keys';
import type { SlugAvailabilityResponse } from '../stores-api';

// Re-export for backward compatibility
export { checkSlugAvailability };
export type { SlugAvailabilityResponse };

export function useCheckSlugAvailability(slug: string, enabled = true) {
  return useQuery({
    queryKey: storeKeys.slugAvailability(slug),
    queryFn: () => checkSlugAvailability(slug),
    enabled: enabled && !!slug,
    staleTime: 30000, // 30 seconds
    retry: false, // Don't retry slug availability checks
  });
}
