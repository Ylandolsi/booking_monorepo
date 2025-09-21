/**
 * Public stores API
 */

import { useQuery } from '@tanstack/react-query';
import { getPublicStore } from '../stores-api';
import { storeKeys } from '../../stores-keys';

// Re-export for convenience
export { getPublicStore };

export const usePublicStore = (slug: string, enabled = true) => {
  return useQuery({
    queryKey: storeKeys.publicStore(slug),
    queryFn: () => getPublicStore(slug),
    enabled: enabled && !!slug,
    staleTime: 2 * 60 * 1000, // 2 minutes for public stores
  });
};
