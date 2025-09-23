import { cn } from '@/lib/cn';
import { MobileContainer } from '@/components/store/mobile-container';
import { ProductCard, type ProductCardType } from '@/components/store/product-card';
import { useState } from 'react';
import { Button } from '@/components/ui';
import { LazyImage } from '@/utils';
import type { ProductFormData } from '@/features/app/store/products/add-product';
import { BookingPage } from '@/features/app/store/products/components/checkout/book';
import calendarImage from '@/assets/calendar-image.jpeg';
interface ResponsiveBuilderLayoutProps {
  children: React.ReactNode;
  previewData: ProductFormData;
  className?: string;
}
export function CheckoutPageProduct({ productData }: { productData: ProductCardType }) {
  return (
    <div className="flex w-full flex-col items-start justify-center gap-5 px-1">
      <div className={cn('bg-accent flex h-60 w-full flex-shrink-0 items-center justify-center overflow-hidden rounded-lg', '')}>
        {!productData.thumbnail?.mainLink && (
          <LazyImage src={calendarImage} placeholder={calendarImage} alt={productData.title} className={'h-full w-full object-cover'}></LazyImage>
        )}{' '}
        {productData.thumbnail?.mainLink && (
          <LazyImage
            src={productData.thumbnail?.mainLink || ''}
            placeholder={productData.thumbnail?.mainLink || ''}
            alt={productData.title}
            className={'object-cover'}
          ></LazyImage>
        )}
      </div>

      <h3 className="text-foreground line-clamp-2 max-w-20 min-w-full text-left text-2xl font-bold break-words">{productData.title}</h3>
      <h3 className="text-primary text-4xl font-bold break-words">{productData.price.toFixed(2)}$</h3>
      <p className="text-foreground text-md max-w-20 min-w-full text-left font-medium break-words">{productData.description}</p>
      <BookingPage />
    </div>
  );
}

export function ResponsiveBuilderLayout({ children, previewData, className }: ResponsiveBuilderLayoutProps) {
  const productData = { ...previewData, thumbnail: previewData.ui?.picture } as ProductCardType;
  console.log('productData in ResponsiveBuilderLayout: ', productData);
  console.log('before useState in ResponsiveBuilderLayout, previewdata: ', previewData);
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
                {viewType == 'overview' && <ProductCard product={productData} displayMode="full" />}
                {viewType == 'checkout' && <CheckoutPageProduct productData={productData} />}
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
