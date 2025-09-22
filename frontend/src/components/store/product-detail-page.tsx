import { cn } from '@/lib/cn';
import { MobileContainer } from './mobile-container';
import type { Product, Store } from '@/api/stores';

interface ProductDetailPageProps {
  product: Product;
  store: Store;
  onPurchase?: () => void;
  className?: string;
}

export function ProductDetailPage({ product, store, onPurchase, className }: ProductDetailPageProps) {
  const fulfillmentNote =
    product.productType === 'Session'
      ? "After payment, you'll receive a calendar invite to choose your preferred time slot."
      : 'Download link will be sent to your email immediately after payment.';

  return (
    <div className={cn('relative', className)}>
      <MobileContainer>
        {/* Back Button */}
        <div className="border-border border-b p-4">
          <button className="text-muted-foreground hover:text-foreground flex items-center space-x-2 transition-colors">
            <span>←</span>
            <span className="text-sm">Back to {store.title}</span>
          </button>
        </div>

        {/* Hero Image */}
        <div className="bg-muted aspect-[4/3] overflow-hidden">
          {product.thumbnail?.mainLink ? (
            <img src={product.thumbnail.mainLink} alt={product.title} className="h-full w-full object-cover" />
          ) : (
            <div className="text-muted-foreground flex h-full w-full items-center justify-center">
              <div className="text-center">
                <div className="mb-4 text-6xl">{product.productType === 'Session' ? '📅' : '📁'}</div>
                <p className="text-sm">No image</p>
              </div>
            </div>
          )}
        </div>

        {/* Content */}
        <div className="px-4 pb-24">
          {/* Title and Price */}
          <div className="border-border border-b py-6">
            <h1 className="text-foreground mb-2 text-2xl font-bold">{product.title}</h1>

            {product.subtitle && <p className="text-muted-foreground mb-4">{product.subtitle}</p>}

            <div className="text-primary text-3xl font-bold">{product.price}</div>
          </div>

          {/* Description */}
          <div className="border-border border-b py-6">
            <h2 className="text-foreground mb-3 font-semibold">Description</h2>
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
            <h2 className="text-foreground mb-3 font-semibold">What happens after purchase?</h2>
            <div className="bg-muted rounded-lg p-4">
              <p className="text-muted-foreground text-sm">{fulfillmentNote}</p>
            </div>
          </div>

          {/* Store Info */}
          <div className="border-border border-t py-6">
            <h2 className="text-foreground mb-3 font-semibold">About {store.title}</h2>
            <div className="flex items-center space-x-3">
              <div className="bg-muted h-12 w-12 overflow-hidden rounded-full">
                {store.picture?.mainLink ? (
                  <img src={store.picture.mainLink} alt={store.title} className="h-full w-full object-cover" />
                ) : (
                  <div className="text-muted-foreground flex h-full w-full items-center justify-center">
                    <span className="font-semibold">{store.title.charAt(0).toUpperCase()}</span>
                  </div>
                )}
              </div>
              <div>
                <p className="text-foreground font-medium">{store.title}</p>
                {store.description && <p className="text-muted-foreground text-sm">{store.description}</p>}
              </div>
            </div>
          </div>
        </div>

        {/* Sticky CTA */}
        <div className="fixed right-0 bottom-0 left-0 z-50">
          <div className="mx-auto max-w-sm">
            <div className="bg-background/95 border-border border-t p-4 backdrop-blur-sm">
              <button
                onClick={onPurchase}
                className="bg-primary text-primary-foreground flex h-12 w-full items-center justify-center space-x-2 rounded-lg text-lg font-semibold transition-opacity hover:opacity-90"
              >
                <span>{product.clickToPay}</span>
                <span>•</span>
                <span>${product.price}</span>
              </button>
            </div>
          </div>
        </div>
      </MobileContainer>
    </div>
  );
}
