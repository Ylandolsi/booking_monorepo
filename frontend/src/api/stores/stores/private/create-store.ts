import { STORE_QUERY_KEY } from '@/api/stores/stores-keys';
import { patchPostStoreSchemaToFormData, type PatchPostStoreRequest, type PatchPostStoreResponse } from '@/api/stores/stores/private/store-schema';
import { api, validateFile } from '@/lib';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { useMutation } from '@tanstack/react-query';

export const createStore = async (data: PatchPostStoreRequest): Promise<PatchPostStoreResponse> => {
  if (data.file) {
    const validation = validateFile(data.file);

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = patchPostStoreSchemaToFormData(data);

  try {
    const response = await api.post<PatchPostStoreResponse>(CatalogEndpoints.Stores.Create, formData);

    return response;
  } catch (error) {
    console.error('Error creating store:', error);
    throw error;
  }
};

export const useCreateStore = () => {
  return useMutation({
    mutationFn: (data: PatchPostStoreRequest) => createStore(data),
    meta: {
      invalidatesQuery: [STORE_QUERY_KEY],
    },
  });
};
