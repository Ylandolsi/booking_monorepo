import { cn } from '@/lib/cn';
import { MobileContainer } from '@/components/store/mobile-container';
import { ProductCard, type ProductCardType } from '@/components/store/product-card';
import { useState } from 'react';
import { Button, LoadingState } from '@/components/ui';
import type { ProductFormData } from '@/features/app/store/products/add-product';
import { CheckoutPageProduct } from '@/features/app/store/products';
import { ErrorComponenet, StoreHeader } from '@/components';
import { useMyStore } from '@/api/stores';

interface ResponsiveBuilderLayoutProps {
  children: React.ReactNode;
  previewData: ProductFormData;
  className?: string;
}

export function ResponsiveBuilderLayout({ children, previewData, className }: ResponsiveBuilderLayoutProps) {
  const { data: store, isLoading, isError } = useMyStore();
  if (isLoading) {
    return <LoadingState type="spinner" />;
  }
  if (isError || !store) {
    return <ErrorComponenet message="Failed to load store data." title="Store Error" />;
  }

  const [viewType, SetViewType] = useState<'checkout' | 'overview'>('overview');
  return (
    <div className={cn(className)}>
      <div className="mx-auto flex max-w-7xl flex-col items-center justify-around lg:flex-row lg:items-start lg:gap-40">
        {/* Form Side */}
        <div className="bg-card border-border min-w-xl flex-1 rounded-xl border shadow-lg">{children}</div>

        {/* Preview Side */}
        <div>
          <div className="lg:sticky lg:top-4">
            <div className={cn('bg-muted/10 flex flex-col gap-2 p-4')}>
              <MobileContainer>
                {/* TODO : add header ?  */}
                {/* TODO : make display mode dynamic ?  */}
                {viewType == 'overview' && (
                  <>
                    <StoreHeader store={store} />
                    <div className="p-6">
                      <ProductCard product={previewData} displayMode="full" />
                    </div>
                  </>
                )}
                {viewType == 'checkout' && <CheckoutPageProduct productData={previewData} f />}
              </MobileContainer>
              <div className="mt-4 flex w-full items-center justify-center gap-2">
                <Button
                  variant={'outline'}
                  className={cn(
                    'rounded-md px-3 py-1',
                    viewType === 'overview' ? 'bg-primary text-primary-foreground hover:bg-primary/60' : 'bg-secondary text-secondary-foreground',
                  )}
                  onClick={() => SetViewType('overview')}
                >
                  Overview
                </Button>
                <Button
                  variant={'outline'}
                  className={cn(
                    'rounded-md px-3 py-1',
                    viewType === 'checkout' ? 'bg-primary text-primary-foreground hover:bg-primary/60' : 'bg-secondary text-secondary-foreground',
                  )}
                  onClick={() => SetViewType('checkout')}
                >
                  Checkout
                </Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
