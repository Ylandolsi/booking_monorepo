import { STORE_QUERY_KEY } from '@/api/stores/stores-keys';
import { api, toFormData, validateFile } from '@/lib';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { useMutation } from '@tanstack/react-query';
import { z } from 'zod';
export interface CreateStoreResponse {
  slug: string;
}

export type createStoreInput = z.infer<typeof createStoreSchema>;

export const createStoreSchema = z.object({
  title: z.string().min(3, 'Store name must be at least 3 characters'),
  slug: z
    .string()
    .min(3, 'Slug must be at least 3 characters')
    .regex(/^[a-z0-9-]+$/, 'Slug can only contain lowercase letters, numbers, and hyphens'),
  description: z.string().optional(),
  picture: z.instanceof(File).optional(),
  socialLinks: z
    .array(
      z.object({
        platform: z.string(),
        url: z.string().url('Invalid URL'),
      }),
    )
    .optional(),
});

export const createStore = async (data: createStoreInput): Promise<CreateStoreResponse> => {
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
    mutationFn: (data: createStoreInput) => createStore(data),
    meta: {
      invalidatesQuery: [STORE_QUERY_KEY],
    },
  });
};
