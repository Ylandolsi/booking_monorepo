import { storeKeys } from '@/api/stores/stores-keys';
import { api, CatalogEndpoints } from '@/api/utils';
import { useMutation } from '@tanstack/react-query';
import { logger } from '@/lib';

/**
 * Delete a product by slug
 * DELETE /api/products/{productSlug}
 */

export const deleteProduct = async ({ productSlug }: { productSlug: string }) => {
  try {
    const response = await api.delete<void>(CatalogEndpoints.Products.Delete(productSlug));

    return response;
  } catch (error) {
    logger.error('Error deleting product:', error);
    throw error;
  }
};

export const useDeleteProduct = () => {
  return useMutation<void, Error, { productSlug: string }>({
    mutationFn: deleteProduct,
    meta: {
      invalidatesQuery: [storeKeys.all],
      successMessage: 'Product deleted successfully!',
    },
  });
};
