import { BookingPage } from '@/features/app/store/products/components/checkout/book';
import { ProductCheckout } from '@/routes/app/store/checkout';
import type { ProductFormData } from '@/features/app/store/products/add-product';
import type { Product } from '@/api/stores';

export function CheckoutPageProduct({ productData }: { productData: ProductFormData }) {
  const productDataToProduct = {
    ...productData,
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    clickToPay: 'Buy now',
    isPublished: true,
    displayOrder: 1,
    productType: 'Session',
    storeSlug: 'sophia-carter-store',
    productSlug: 'ultimate-guide-design',
  } as Product;
  return (
    <div className="flex w-full flex-col items-start justify-center gap-5">
      <ProductCheckout product={productDataToProduct}>{productData.productType == 'Session' && <BookingPage />}</ProductCheckout>
    </div>
  );
}
