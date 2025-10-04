import { storeKeys } from '@/api/stores/stores-keys';
import { api, CatalogEndpoints } from '@/api/utils';
import { useMutation } from '@tanstack/react-query';

/**
 * Toggle product published status
 * PATCH /api/products/toggle/{productSlug}
 */

export type TogglePublishedResponse = {
  slug: string;
  isPublished: boolean;
};

export const toggleProduct = async ({ productSlug }: { productSlug: string }) => {
  try {
    const response = await api.patch<TogglePublishedResponse>(CatalogEndpoints.Products.TogglePublished(productSlug));

    return response;
  } catch (error) {
    console.error('Error toggling product published status:', error);
    throw error;
  }
};

export const useToggleProduct = () => {
  return useMutation<TogglePublishedResponse, Error, { productSlug: string }>({
    mutationFn: toggleProduct,
    meta: {
      invalidatesQuery: [storeKeys.all],
      successMessage: 'Product status updated successfully!',
    },
  });
};
