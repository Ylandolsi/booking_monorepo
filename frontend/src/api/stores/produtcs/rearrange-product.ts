import { storeKeys } from '@/api/stores/stores-keys';
import { api, CatalogEndpoints } from '@/api/utils';
import { useMutation } from '@tanstack/react-query';
import { logger } from '@/lib';

/**
 * Rearrange products order
 * PATCH /api/products/arrange
 */

export type RearrangeProductsInput = {
  orders: Record<string, number>; // productSlug: displayOrder
};

export const rearrangeProducts = async ({ orders }: RearrangeProductsInput) => {
  try {
    const response = await api.patch<void>(CatalogEndpoints.Products.Arrange, {
      orders,
    });

    return response;
  } catch (error) {
    logger.error('Error rearranging products:', error);
    throw error;
  }
};

export const useRearrangeProducts = () => {
  return useMutation<void, Error, RearrangeProductsInput>({
    mutationFn: rearrangeProducts,
    meta: {
      invalidatesQuery: [storeKeys.all],
      successMessage: 'Products reordered successfully!',
    },
  });
};
