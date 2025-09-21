import { useMutation } from '@tanstack/react-query';
import { storeKeys, STORE_QUERY_KEY } from '../../stores-keys';
import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
import { api } from '@/lib';
import z from 'zod';

export interface UpdateStoreResponse {
  slug: string;
}

export const updateStoreSchema = z.object({
  title: z.string().min(3, 'Store name must be at least 3 characters'),
  slug: z
    .string()
    .min(3, 'Slug must be at least 3 characters')
    .regex(/^[a-z0-9-]+$/, 'Slug can only contain lowercase letters, numbers, and hyphens'),
  description: z.string().optional(),
  socialLinks: z
    .array(
      z.object({
        platform: z.string(),
        url: z.string().url('Invalid URL'),
      }),
    )
    .optional(),
});

export type UpdateStoreInput = z.infer<typeof updateStoreSchema>;

export const updateStore = async (data: UpdateStoreInput): Promise<UpdateStoreResponse> => {
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
    mutationFn: (data: UpdateStoreInput) => updateStore(data),
    meta: {
      invalidatesQuery: [[STORE_QUERY_KEY], storeKeys.myStore()],
      successMessage: 'Store updated successfully!',
    },
  });
};
