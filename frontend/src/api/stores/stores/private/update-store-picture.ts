import { useMutation, useQueryClient } from '@tanstack/react-query';
import { updateStorePicture } from '../stores-api';
import type { UpdateStorePictureInput } from '../stores-api';
import { storeKeys, STORE_QUERY_KEY } from '../../stores-keys';

// Re-export for backward compatibility
export type { UpdateStorePictureInput };
export { updateStorePicture };

export const useUpdateStorePicture = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateStorePictureInput) => updateStorePicture(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY] });
      queryClient.invalidateQueries({ queryKey: storeKeys.myStore() });
    },
    meta: {
      successMessage: 'Store picture updated successfully!',
    },
  });
};
