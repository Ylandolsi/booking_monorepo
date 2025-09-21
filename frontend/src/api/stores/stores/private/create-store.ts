import { STORE_QUERY_KEY } from '@/api/stores/stores-keys';
import { api, toFormData, validateFile } from '@/lib';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { useMutation, useQueryClient } from '@tanstack/react-query';

export interface CreateStoreResponse {
  slug: string;
}

export interface CreateStoreInput {
  title: string;
  slug: string;
  description?: string;
  picture?: File;
  socialLinks?: Array<{ platform: string; url: string }>;
}

export const createStore = async (data: CreateStoreInput): Promise<CreateStoreResponse> => {
  if (data.picture) {
    const validation = validateFile(data.picture, {
      maxSizeInMB: 5,
      allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
      required: false,
    });

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = toFormData({
    title: data.title,
    slug: data.slug,
    file: data.picture || new File([], ''), // Backend expects a file field
    description: data.description || '',
    socialLinks: data.socialLinks || [],
  });

  try {
    const response = await api.post<CreateStoreResponse>(CatalogEndpoints.Stores.Create, formData);

    return response;
  } catch (error) {
    console.error('Error creating store:', error);
    throw error;
  }
};

export const useCreateStore = () => {
  return useMutation({
    mutationFn: (data: CreateStoreInput) => createStore(data),
    meta: {
      invalidatesQuery: [STORE_QUERY_KEY],
    },
  });
};
