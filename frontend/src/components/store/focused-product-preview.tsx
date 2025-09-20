import { cn } from '@/lib/cn';
import { MobileContainer } from './mobile-container';
import { ProductCard } from './product-card';
import type { Product } from '@/types/product';

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
    <div className={cn('bg-card rounded-xl shadow-lg border border-border overflow-hidden', className)}>
      {/* Header */}
      <div className="p-4 border-b border-border bg-muted/30">
        <div className="flex items-center space-x-2">
          <div className="w-2 h-2 rounded-full bg-destructive"></div>
          <div className="w-2 h-2 rounded-full bg-yellow-500"></div>
          <div className="w-2 h-2 rounded-full bg-green-500"></div>
          <div className="flex-1 text-center">
            <span className="text-xs text-muted-foreground font-mono">{storeData.name} Preview</span>
          </div>
        </div>
      </div>

      {/* Mobile Preview */}
      <div className="p-4 bg-muted/10">
        <MobileContainer className="bg-background shadow-xl border border-border/50">
          {/* Mini Store Header */}
          <div className="p-4 border-b border-border bg-card">
            <div className="flex items-center space-x-3">
              <div className="w-8 h-8 rounded-full bg-muted flex items-center justify-center">
                {storeData.profilePicture ? (
                  <img src={storeData.profilePicture} alt={storeData.name} className="w-full h-full object-cover rounded-full" />
                ) : (
                  <span className="text-xs font-semibold text-muted-foreground">{storeData.name.charAt(0).toUpperCase()}</span>
                )}
              </div>
              <div>
                <p className="text-sm font-semibold text-foreground">{storeData.name}</p>
                <p className="text-xs text-muted-foreground">Product Preview</p>
              </div>
            </div>
          </div>

          {/* Product Preview */}
          <div className="p-4">
            <ProductCard product={previewProduct} displayMode="full" />

            {/* Live Preview Notice */}
            <div className="mt-4 p-3 bg-primary/10 rounded-lg border border-primary/20">
              <p className="text-xs text-primary text-center">✨ Live Preview - Changes update in real-time</p>
            </div>
          </div>
        </MobileContainer>
      </div>
    </div>
  );
}
