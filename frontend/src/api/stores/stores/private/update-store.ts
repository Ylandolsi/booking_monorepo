import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updateStore, validateUpdateStoreInput } from '../stores-api';
import type { UpdateStoreInput } from '../stores-api';
import { storeKeys, STORE_QUERY_KEY } from '../../stores-keys';

// Re-export for backward compatibility
export type { UpdateStoreInput };
export { updateStore, validateUpdateStoreInput };

export const useUpdateStore = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateStoreInput) => updateStore(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: storeKeys.myStore() });
    },
    meta: {
      successMessage: 'Store updated successfully!',
    },
  });
};
