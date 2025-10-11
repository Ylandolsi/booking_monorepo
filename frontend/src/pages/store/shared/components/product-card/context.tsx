import { createContext, useContext, type ReactNode } from 'react';
import type { ProductCardType } from './types';
import type { ProductStyle } from '@/api/stores';

interface ProductCardContextValue {
  product: ProductCardType;
  edit: boolean;
  onActionClick?: () => void;
}

const ProductCardContext = createContext<ProductCardContextValue | null>(null);

export const useProductCardContext = () => {
  const context = useContext(ProductCardContext);
  if (!context) {
    throw new Error('ProductCard compound components must be used within ProductCard');
  }
  return context;
};

interface ProductCardProviderProps extends ProductCardContextValue {
  children: ReactNode;
}

export const ProductCardProvider = ({ children, ...value }: ProductCardProviderProps) => {
  return <ProductCardContext.Provider value={value}>{children}</ProductCardContext.Provider>;
};
