import { cn } from '@/lib/cn';
import { MobileContainer } from './mobile-container';
import { StoreHeader } from './store-header';
import { ProductCard } from './product-card';

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

interface ProductData {
  title: string;
  subtitle: string;
  price: string;
  description: string;
  coverImage: File | null;
  ctaText: string;
  type: 'booking' | 'digital';
}

interface LivePreviewProps {
  productData: Partial<ProductData>;
  storeData: Store;
  className?: string;
}

export function LivePreview({ productData, storeData, className }: LivePreviewProps) {
  // Create a preview product object
  const previewProduct = {
    id: 'preview',
    title: productData.title || 'Your Product Title',
    subtitle: productData.subtitle || 'Your product subtitle',
    price: productData.price ? `$${productData.price}` : '$0',
    coverImage: productData.coverImage ? URL.createObjectURL(productData.coverImage) : '',
    ctaText: productData.ctaText || (productData.type === 'booking' ? 'Book Now' : 'Buy Now'),
    type: productData.type || 'digital',
    description: productData.description || '',
  };

  return (
    <div className={cn('sticky top-4 bg-card rounded-xl shadow-lg border border-border overflow-hidden', className)}>
      {/* Preview Header */}
      <div className="px-4 py-3 border-b border-border bg-muted/50">
        <div className="flex items-center space-x-2">
          <div className="w-2 h-2 rounded-full bg-destructive"></div>
          <div className="w-2 h-2 rounded-full bg-yellow-500"></div>
          <div className="w-2 h-2 rounded-full bg-green-500"></div>
          <div className="flex-1 text-center">
            <span className="text-xs text-muted-foreground font-mono">Live Preview</span>
          </div>
        </div>
      </div>

      {/* Mobile Preview */}
      <div className="bg-background">
        <MobileContainer className="shadow-none rounded-none max-w-none">
          <StoreHeader store={storeData} />
          <div className="p-4">
            <ProductCard product={previewProduct} />
          </div>
        </MobileContainer>
      </div>

      {/* Preview Info */}
      <div className="px-4 py-3 border-t border-border bg-muted/30">
        <p className="text-xs text-muted-foreground text-center">This is how your product will appear to customers</p>
      </div>
    </div>
  );
}
