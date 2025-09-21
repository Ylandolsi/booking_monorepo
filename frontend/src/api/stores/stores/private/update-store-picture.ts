import { STORE_QUERY_KEY, storeKeys } from '@/api/stores/stores-keys';
import { api, toFormData, validateFile } from '@/lib';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { useMutation } from '@tanstack/react-query';

export interface UpdateStorePictureInput {
  picture: File;
}
export interface UpdateStorePictureResponse {
  picture: File;
}

export const updateStorePicture = async (data: UpdateStorePictureInput): Promise<UpdateStorePictureResponse> => {
  // Validate the picture file
  const validation = validateFile(data.picture, {
    maxSizeInMB: 5,
    allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
    required: true,
  });

  if (!validation.isValid) {
    throw new Error(validation.error || 'Invalid file');
  }

  // Create FormData for the request
  const formData = toFormData({
    file: data.picture,
  });

  try {
    const response = await api.patch<UpdateStorePictureResponse>(CatalogEndpoints.Stores.UpdatePicture, formData);

    return response;
  } catch (error) {
    console.error('Error updating store picture:', error);
    throw error;
  }
};

export const useUpdateStorePicture = () => {
  return useMutation({
    mutationFn: (data: UpdateStorePictureInput) => updateStorePicture(data),

    meta: {
      invalidatesQuery: [[STORE_QUERY_KEY], storeKeys.myStore()],
      successMessage: 'Store picture updated successfully!',
    },
  });
};
