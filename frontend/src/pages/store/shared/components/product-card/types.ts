import type { Product, ProductStyle } from '@/api/stores';
import type { Dispatch, SetStateAction } from 'react';

// export type DisplayMode = 'full' | 'compact' | 'minimal';

export type ProductCardType = Pick<
  Product,
  'productSlug' | 'thumbnailPicture' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType' | 'isPublished'
>;

export interface ProductCardProps {
  product: ProductCardType;
  onClick?: () => void;
  displayMode?: ProductStyle;
  className?: string;
  edit?: boolean;
  onActionClick?: () => void;
  setProducts?: Dispatch<SetStateAction<Product[]>>;
}
