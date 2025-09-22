import type { Product } from '@/api/stores';
import { MobileContainer } from './mobile-container';
import { ProductCard } from './product-card';

interface FocusedProductPreviewProps {
  productData: Partial<Product>;
  storeData: {
    name: string;
    description?: string;
    profilePicture?: string;
  };
  className?: string;
}

export function FocusedProductPreview({ productData, storeData, className }: FocusedProductPreviewProps) {
  // Create a preview product with defaults
  const previewProduct: Product = {
    productSlug: 'preview',
    storeSlug: 'preview-store',
    title: productData.title || 'Your Product Title',
    subtitle: productData.subtitle || 'Add a compelling subtitle',
    price: productData.price ? Number(productData.price) : 0,
    thumbnail: productData.thumbnail || undefined,
    clickToPay: productData.clickToPay || 'Buy Now',
    productType: productData.productType || 'DigitalDownload',
    description: productData.description || 'Add a detailed description...',
    displayOrder: 1,
    isPublished: true,
    createdAt: new Date().toISOString(),
  };

  return (
    <div className="bg-muted/10 p-4">
      <MobileContainer>
        {/* TODO : add header ?  */}
        <div className="p-4">
          {/* TODO : make display mode dynamic ?  */}

          <ProductCard product={previewProduct} displayMode="full" />
        </div>
      </MobileContainer>
    </div>
  );
}
