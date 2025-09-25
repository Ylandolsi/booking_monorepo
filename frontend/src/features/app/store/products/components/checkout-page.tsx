import { type ProductCardType } from '@/components/store/product-card';
import { BookingPage } from '@/features/app/store/products/components/checkout/book';
import { ProductCheckout } from '@/routes/app/store/checkout';
import type { ProductFormData } from '@/features/app/store/products/add-product';
import type { Product } from '@/api/stores';

export function CheckoutPageProduct({ productData }: { productData: ProductCardType | ProductFormData }) {
  return (
    <div className="flex w-full flex-col items-start justify-center gap-5 px-1">
      <ProductCheckout product={productData as Product}>{productData.productType == 'Session' && <BookingPage />}</ProductCheckout>
    </div>
  );
}
