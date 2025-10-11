import { useMutation } from '@tanstack/react-query';
import { storeKeys, STORE_QUERY_KEY } from '../../stores-keys';
import { CatalogEndpoints } from '@/api/utils/catalog-endpoints';
import { api, validateFile } from '@/api/utils';
import { patchPostStoreSchemaToFormData, type PatchPostStoreRequest, type PatchPostStoreResponse } from '@/api/stores/stores/private/store-schema';
import { logger } from '@/lib';

export const updateStore = async (data: PatchPostStoreRequest): Promise<PatchPostStoreResponse> => {
  if (data.file) {
    const validation = validateFile(data.file);

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = patchPostStoreSchemaToFormData(data);

  try {
    const response = await api.put<PatchPostStoreResponse>(CatalogEndpoints.Stores.Update, formData);

    return response;
  } catch (error) {
    logger.error('Error updating store:', error);
    throw error;
  }
};

export const useUpdateStore = () => {
  return useMutation({
    mutationFn: (data: PatchPostStoreRequest) => updateStore(data),
    meta: {
      invalidatesQuery: [storeKeys.all],
      successMessage: 'Store updated successfully!',
    },
  });
};
