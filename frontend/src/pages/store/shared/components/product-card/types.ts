import type { Product, ProductStyle } from '@/api/stores';
import type { Dispatch, SetStateAction } from 'react';

export type ProductCardType = Pick<
  Product,
  'productSlug' | 'productStyle' | 'thumbnailPicture' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType' | 'isPublished'
>;

export interface ProductCardProps {
  product: ProductCardType;
  onClick?: () => void;
  className?: string;
  edit?: boolean;
  onActionClick?: () => void;
  setProducts?: Dispatch<SetStateAction<Product[]>>;
}
