import { MobileContainer } from './mobile-container';
import { ProductCard } from './product-card';
import type { Product } from '@/types/product';
import { IPhoneMockup } from 'react-device-mockup';

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
    id: 'preview',
    title: productData.title || 'Your Product Title',
    subtitle: productData.subtitle || 'Add a compelling subtitle',
    price: productData.price ? `$${productData.price}` : '$0',
    coverImage:
      typeof productData.coverImage === 'string'
        ? productData.coverImage
        : productData.coverImage
          ? URL.createObjectURL(productData.coverImage as File)
          : '',
    ctaText: productData.ctaText || (productData.type === 'booking' ? 'Book Now' : 'Buy Now'),
    type: productData.type || 'digital',
    description: productData.description || 'Add a detailed description...',
  };

  return (
    <div className="bg-muted/10 p-4">
      <IPhoneMockup screenWidth={320}>
        <MobileContainer>
          {/* TODO : add header ?  */}
          <div className="p-4">
            {/* TODO : make display mode dynamic ?  */}

            <ProductCard product={previewProduct} displayMode="full" />
          </div>
        </MobileContainer>
      </IPhoneMockup>
    </div>
  );
}
