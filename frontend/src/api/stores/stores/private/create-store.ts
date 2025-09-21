import { useMutation, useQueryClient } from '@tanstack/react-query';
import { createStore, validateCreateStoreInput } from '../stores-api';
import type { CreateStoreInput } from '../stores-api';
import { STORE_QUERY_KEY } from '../../stores-keys';

export interface CreateStoreResponse {
  slug: string;
}

// Re-export for backward compatibility
export type { CreateStoreInput };
export { createStore, validateCreateStoreInput };

export const useCreateStore = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateStoreInput) => createStore(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY] });
    },
  });
};
