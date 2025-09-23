import { useState } from 'react';
import { MobileContainer } from './mobile-container';
import { StoreHeader } from './store-header';
import { DraggableProductList } from './draggable-product-list';
import { EmptyState } from './empty-state';
import { AddProductButton } from './add-product-button';
import { cn } from '@/lib/cn';
import { initialStore, useMyStore, type Product } from '@/api/stores';
import { LoadingState } from '@/components/ui';
import { ErrorComponenet } from '@/components/errors';

type DisplayMode = 'full' | 'compact';

interface EnhancedStorefrontDashboardProps {
  products: Product[];
  onAddProduct?: () => void;
  onProductClick?: (product: Product) => void;
  onProductEdit?: (product: Product) => void;
  onProductDelete?: (product: Product) => void;
  onProductsReorder?: (products: Product[]) => void;
  isOwner?: boolean;
  className?: string;
}

export function EnhancedStorefrontDashboard({
  products,
  onAddProduct,
  onProductClick,
  onProductEdit,
  onProductDelete,
  onProductsReorder,
  isOwner = false,
  className,
}: EnhancedStorefrontDashboardProps) {
  let { data: store, isLoading, isError } = useMyStore();
  store = initialStore; // for testing
  isError = false;
  isLoading = false;
  const [displayMode, setDisplayMode] = useState<DisplayMode>('full');

  if (isLoading) {
    return <LoadingState type="spinner" />;
  }

  if (isError || !store) {
    return <ErrorComponenet message="Failed to load store data." title="Store Error" />;
  }

  return (
    <MobileContainer screenWidth={340} className={cn('items-center space-y-4', className)}>
      <StoreHeader store={store} />

      {/* Controls (only visible to owner) */}
      {/* {isOwner && products.length > 0 && (
          <div className="bg-muted/30 b mt-4">
            <DisplayModeSelector displayMode={displayMode} onChange={setDisplayMode} />
          </div>
        )} */}

      {products.length === 0 ? (
        <EmptyState onAddProduct={isOwner ? onAddProduct : undefined} />
      ) : (
        <>
          <DraggableProductList
            products={products}
            onProductClick={onProductClick}
            onProductEdit={onProductEdit}
            onProductDelete={onProductDelete}
            onReorder={onProductsReorder || (() => {})}
            showActions={isOwner}
            displayMode={displayMode}
          />
        </>
      )}
    </MobileContainer>
  );
}
