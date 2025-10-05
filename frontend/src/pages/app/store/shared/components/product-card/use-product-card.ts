import { useDeleteProduct, useToggleProduct, type Product } from '@/api/stores';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { useState, type Dispatch, type SetStateAction } from 'react';
import type { ProductCardType } from './types';

interface UseProductCardOptions {
  product: ProductCardType;
  edit?: boolean;
  setProducts?: Dispatch<SetStateAction<Product[]>>;
}

export const useProductCard = ({ product, edit = false, setProducts }: UseProductCardOptions) => {
  const [isPopoverOpen, setIsPopoverOpen] = useState<boolean>(false);

  // Drag and drop
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: product.productSlug || '',
    disabled: !edit || !product.productSlug,
  });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  // Mutations
  const deleteProductMutation = useDeleteProduct();
  const toggleProductMutation = useToggleProduct();

  // Handlers
  const handleTogglePublished = (checked: boolean) => {
    if (!edit || !product.productSlug) return;

    toggleProductMutation.mutate({ productSlug: product.productSlug });

    // Optimistic update
    if (setProducts) {
      setProducts((prevProducts) => {
        const index = prevProducts.findIndex((p) => p.productSlug === product.productSlug);
        if (index !== -1) {
          const updatedProduct = { ...prevProducts[index], isPublished: checked };
          return [...prevProducts.slice(0, index), updatedProduct, ...prevProducts.slice(index + 1)];
        }
        return prevProducts;
      });
    }
  };

  const handleDeleteProduct = () => {
    if (!edit || !product.productSlug) return;

    deleteProductMutation.mutate({ productSlug: product.productSlug });

    // Optimistic update
    if (setProducts) {
      setProducts((prevProducts) => prevProducts.filter((p) => p.productSlug !== product.productSlug));
    }
  };

  return {
    // State
    isPopoverOpen,
    setIsPopoverOpen,
    isDragging,

    // Drag and drop
    dragHandleProps: { ...attributes, ...listeners },
    style,
    setNodeRef,

    // Mutations
    handleTogglePublished,
    handleDeleteProduct,
    isDeleting: deleteProductMutation.isPending,
    isToggling: toggleProductMutation.isPending,
  };
};
