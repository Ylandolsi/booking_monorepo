import { useState } from 'react';
import { MobileContainer } from './mobile-container';
import { StoreHeader } from './store-header';
import { ScrollableContent } from './scrollable-content';
import { DraggableProductList } from './draggable-product-list';
import { EmptyState } from './empty-state';
import { AddProductButton } from './add-product-button';
import { DisplayModeSelector } from './display-mode-selector';
import type { Product } from '@/types/product';
import { IPhoneMockup } from 'react-device-mockup';
import { cn } from '@/lib/cn';

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

type DisplayMode = 'full' | 'compact';

interface EnhancedStorefrontDashboardProps {
  store: Store;
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
  store,
  products,
  onAddProduct,
  onProductClick,
  onProductEdit,
  onProductDelete,
  onProductsReorder,
  isOwner = false,
  className,
}: EnhancedStorefrontDashboardProps) {
  const [displayMode, setDisplayMode] = useState<DisplayMode>('full');

  return (
    <IPhoneMockup screenWidth={400}>
      <MobileContainer className={cn('items-center space-y-4', className)}>
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

            {isOwner && <AddProductButton onClick={onAddProduct} />}
          </>
        )}
      </MobileContainer>
    </IPhoneMockup>
  );
}
