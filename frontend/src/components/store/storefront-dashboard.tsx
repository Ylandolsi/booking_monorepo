import { MobileContainer } from './mobile-container';
import { StoreHeader } from './store-header';
import { ScrollableContent } from './scrollable-content';
import { ProductList } from './product-list';
import { EmptyState } from './empty-state';
import { AddProductButton } from './add-product-button';
import type { Product } from '@/types/product';

interface Store {
  name: string;
  description?: string;
  profilePicture?: string;
  socialLinks?: {
    instagram?: string;
    twitter?: string;
    website?: string;
  };
}

interface StorefrontDashboardProps {
  store: Store;
  products: Product[];
  onAddProduct?: () => void;
  onProductClick?: (product: Product) => void;
  isOwner?: boolean;
}

export function StorefrontDashboard({ store, products, onAddProduct, onProductClick, isOwner = false }: StorefrontDashboardProps) {
  return (
    <MobileContainer>
      <StoreHeader store={store} />

      <ScrollableContent>
        {products.length === 0 ? (
          <EmptyState onAddProduct={isOwner ? onAddProduct : undefined} />
        ) : (
          <>
            <ProductList products={products} onProductClick={onProductClick} showActions={isOwner} />

            {isOwner && <AddProductButton onClick={onAddProduct} />}
          </>
        )}
      </ScrollableContent>
    </MobileContainer>
  );
}
