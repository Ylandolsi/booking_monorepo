import { useMutation } from '@tanstack/react-query';
import { storeKeys, STORE_QUERY_KEY } from '../../stores-keys';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api, validateFile } from '@/lib';
import { patchPostStoreSchemaToFormData, type PatchPostStoreRequest, type PatchPostStoreResponse } from '@/api/stores/stores/private/store-schema';

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
    console.error('Error updating store:', error);
    throw error;
  }
};

export const useUpdateStore = () => {
  return useMutation({
    mutationFn: (data: PatchPostStoreRequest) => updateStore(data),
    meta: {
      invalidatesQuery: [[STORE_QUERY_KEY], storeKeys.myStore()],
      successMessage: 'Store updated successfully!',
    },
  });
};
