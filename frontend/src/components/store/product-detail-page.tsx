import { cn } from '@/lib/cn';
import { MobileContainer } from './mobile-container';
import type { Product } from '@/types/product';

interface Store {
  name: string;
  description?: string;
  profilePicture?: string;
}

interface ProductDetailPageProps {
  product: Product;
  store: Store;
  onPurchase?: () => void;
  className?: string;
}

export function ProductDetailPage({ product, store, onPurchase, className }: ProductDetailPageProps) {
  const fulfillmentNote =
    product.type === 'booking'
      ? "After payment, you'll receive a calendar invite to choose your preferred time slot."
      : 'Download link will be sent to your email immediately after payment.';

  return (
    <div className={cn('relative', className)}>
      <MobileContainer>
        {/* Back Button */}
        <div className="p-4 border-b border-border">
          <button className="flex items-center space-x-2 text-muted-foreground hover:text-foreground transition-colors">
            <span>←</span>
            <span className="text-sm">Back to {store.name}</span>
          </button>
        </div>

        {/* Hero Image */}
        <div className="aspect-[4/3] overflow-hidden bg-muted">
          {product.coverImage ? (
            <img src={product.coverImage} alt={product.title} className="w-full h-full object-cover" />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-muted-foreground">
              <div className="text-center">
                <div className="text-6xl mb-4">{product.type === 'booking' ? '📅' : '📁'}</div>
                <p className="text-sm">No image</p>
              </div>
            </div>
          )}
        </div>

        {/* Content */}
        <div className="px-4 pb-24">
          {/* Title and Price */}
          <div className="py-6 border-b border-border">
            <h1 className="text-2xl font-bold text-foreground mb-2">{product.title}</h1>

            {product.subtitle && <p className="text-muted-foreground mb-4">{product.subtitle}</p>}

            <div className="text-3xl font-bold text-primary">{product.price}</div>
          </div>

          {/* Description */}
          <div className="py-6 border-b border-border">
            <h2 className="font-semibold text-foreground mb-3">Description</h2>
            <div className="prose prose-sm text-foreground leading-relaxed">
              {product.description?.split('\n').map((paragraph, index) => (
                <p key={index} className="mb-3 last:mb-0">
                  {paragraph}
                </p>
              ))}
            </div>
          </div>

          {/* Fulfillment Information */}
          <div className="py-6">
            <h2 className="font-semibold text-foreground mb-3">What happens after purchase?</h2>
            <div className="p-4 bg-muted rounded-lg">
              <p className="text-sm text-muted-foreground">{fulfillmentNote}</p>
            </div>
          </div>

          {/* Store Info */}
          <div className="py-6 border-t border-border">
            <h2 className="font-semibold text-foreground mb-3">About {store.name}</h2>
            <div className="flex items-center space-x-3">
              <div className="w-12 h-12 rounded-full overflow-hidden bg-muted">
                {store.profilePicture ? (
                  <img src={store.profilePicture} alt={store.name} className="w-full h-full object-cover" />
                ) : (
                  <div className="w-full h-full flex items-center justify-center text-muted-foreground">
                    <span className="font-semibold">{store.name.charAt(0).toUpperCase()}</span>
                  </div>
                )}
              </div>
              <div>
                <p className="font-medium text-foreground">{store.name}</p>
                {store.description && <p className="text-sm text-muted-foreground">{store.description}</p>}
              </div>
            </div>
          </div>
        </div>

        {/* Sticky CTA */}
        <div className="fixed bottom-0 left-0 right-0 z-50">
          <div className="max-w-sm mx-auto">
            <div className="p-4 bg-background/95 border-t border-border backdrop-blur-sm">
              <button
                onClick={onPurchase}
                className="w-full h-12 bg-primary text-primary-foreground rounded-lg font-semibold text-lg hover:opacity-90 transition-opacity flex items-center justify-center space-x-2"
              >
                <span>{product.ctaText}</span>
                <span>•</span>
                <span>{product.price}</span>
              </button>
            </div>
          </div>
        </div>
      </MobileContainer>
    </div>
  );
}
