import { BookingPage } from '@/pages/app/store/private/products/book';
import type { Product } from '@/api/stores';
import { ProductCheckout } from '@/pages/app/store/shared/components';

// TODO : needs to be generic to handle both form / api data

export type ProductCheckoutType = Pick<Product, 'thumbnailPicture' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType'>;

export function CheckoutPageProduct({ productData }: { productData: ProductCheckoutType }) {
  return (
    <div className="flex w-full flex-col items-start justify-center gap-5">
      <ProductCheckout product={productData}>{productData.productType == 'Session' && <BookingPage product={productData} />}</ProductCheckout>
    </div>
  );
}
