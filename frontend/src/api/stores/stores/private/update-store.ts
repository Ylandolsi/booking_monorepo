import { useMutation } from '@tanstack/react-query';
import { storeKeys, STORE_QUERY_KEY } from '../../stores-keys';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api } from '@/lib';
import type { SocialLink } from '@/api/stores/stores';

export interface UpdateStoreResponse {
  slug: string;
}

export interface UpdateStoreRequest {
  title: string;
  slug: string;
  description?: string;
  socialLinks?: SocialLink[];
}

export const updateStore = async (data: UpdateStoreRequest): Promise<UpdateStoreResponse> => {
  try {
    const response = await api.put<UpdateStoreResponse>(CatalogEndpoints.Stores.Update, {
      title: data.title,
      slug: data.slug,
      description: data.description || '',
      socialLinks: data.socialLinks || [],
    });

    return response;
  } catch (error) {
    console.error('Error updating store:', error);
    throw error;
  }
};

export const useUpdateStore = () => {
  return useMutation({
    mutationFn: (data: UpdateStoreRequest) => updateStore(data),
    meta: {
      invalidatesQuery: [[STORE_QUERY_KEY], storeKeys.myStore()],
      successMessage: 'Store updated successfully!',
    },
  });
};
