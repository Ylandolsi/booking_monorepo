import { cn } from '@/lib/cn';
import { type ProductCardType } from '@/components/store/product-card';
import { LazyImage } from '@/utils';
import { BookingPage } from '@/features/app/store/products/components/checkout/book';
import calendarImage from '@/assets/calendar-image.jpeg';

export function CheckoutPageProduct({ productData }: { productData: ProductCardType }) {
  return (
    <div className="flex w-full flex-col items-start justify-center gap-5 px-1">
      <div className={cn('bg-accent flex h-60 w-full flex-shrink-0 items-center justify-center overflow-hidden rounded-lg', '')}>
        {!productData.thumbnail?.mainLink && (
          <>
            {productData.productType === 'Session' && (
              <LazyImage src={calendarImage} placeholder={calendarImage} alt={productData.title} className={'h-full w-full object-cover'}></LazyImage>
            )}
          </>
        )}
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
      {productData.productType == 'Session' && <BookingPage />}
    </div>
  );
}
