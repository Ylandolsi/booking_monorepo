import { cn } from '@/lib/cn';
import { Button } from '../ui';
import type { Picture, Product } from '@/api/stores';

type DisplayMode = 'full' | 'compact';

export type ProductCardType = Pick<Product, 'thumbnail' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType'>;

interface ProductCardProps {
  product: ProductCardType;
  onClick?: () => void;
  showActions?: boolean;
  displayMode?: DisplayMode;
  onEdit?: () => void;
  onDelete?: () => void;
  className?: string;
}

export function ProductCard({ product, onClick, showActions = false, displayMode = 'full', onEdit, onDelete, className }: ProductCardProps) {
  // Determine the effective thumbnail mode
  // const thumbnailMode = product.thumbnailMode || 'expanded';

  // if (displayMode === 'compact') {
  if (true)
    return (
      <div
        className={cn(
          'bg-card border-border w-full rounded-xl border p-4 shadow-sm',
          'transition-shadow hover:shadow-md',
          onClick && 'cursor-pointer',
          className,
        )}
        onClick={onClick}
      >
        <div className="flex flex-col gap-4">
          {/* Top row: thumbnail | title/subtitle (flexible) | price (fixed) */}
          <div className="flex w-full items-start justify-between">
            <div className="flex min-w-0 items-start gap-3">
              <div className={cn('flex h-12 w-12 flex-shrink-0 items-center justify-center overflow-hidden rounded-lg')}>
                {/* <img src={product.thumbnail?.mainLink} alt={product.title} className="h-full w-full object-cover" /> */}
                <span className="text-4xl">{product.productType === 'Session' ? '📅' : '📁'}</span>
              </div>

              <div className="min-w-0">
                <h3 className="text-foreground line-clamp-2 font-semibold break-words">{product.title}</h3>
                {product.subtitle && <p className="text-muted-foreground line-clamp-2 text-sm break-words">{product.subtitle}</p>}
              </div>
            </div>

            <div className="ml-4 flex-none text-right">
              <span className="text-primary text-lg font-bold">${product.price}</span>
            </div>
          </div>

          <Button className="bg-primary text-primary-foreground w-full rounded-lg px-3 py-1.5 text-sm font-semibold transition-opacity hover:opacity-90">
            {product.clickToPay ? product.clickToPay : 'Buy Now'}
          </Button>
        </div>
      </div>
    );

  // Full display mode with thumbnail mode support
  return (
    <div
      className={cn(
        'bg-card border-border rounded-xl border p-4 shadow-sm',
        'transition-shadow hover:shadow-md',
        onClick && 'cursor-pointer',
        className,
      )}
      onClick={onClick}
    >
      {/* Cover Image with conditional layout
      {thumbnailMode === 'expanded' ? (
        <div className="bg-muted mb-3 aspect-square overflow-hidden rounded-lg">
          {product.thumbnail?.mainLink ? (
            <img src={product.thumbnail.mainLink} alt={product.title} className="h-full w-full object-cover" />
          ) : (
            <div className="text-muted-foreground flex h-full w-full items-center justify-center">
              <div className="text-center">
                <div className="mb-2 text-3xl">{product.productType === 'Session' ? '📅' : '📁'}</div>
                <div className="text-xs">No image</div>
              </div>
            </div>
          )}
        </div>
      ) : (
        <div className="mb-3 flex items-start space-x-3">
          <div className="bg-muted flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-lg">
            {product.thumbnail?.mainLink ? (
              <img src={product.thumbnail.mainLink} alt={product.title} className="h-full w-full rounded-lg object-cover" />
            ) : (
              <span className="text-2xl">{product.type === 'booking' ? '📅' : '📁'}</span>
            )}
          </div>

          <div className="min-w-0 flex-1 pt-1">
            <h3 className="text-foreground mb-1 line-clamp-2 font-semibold">{product.title}</h3>
            {product.subtitle && <p className="text-muted-foreground line-clamp-2 text-sm">{product.subtitle}</p>}
          </div>
        </div>
      )} */}
      {/* Content - price and CTA always at bottom */}
      <div className="space-y-2">
        <>
          <h3 className="text-foreground mb-1 line-clamp-2 font-semibold">{product.title}</h3>
          {product.subtitle && <p className="text-muted-foreground mb-2 line-clamp-2 text-sm">{product.subtitle}</p>}
        </>

        {/* Price and CTA */}
        <div className="flex items-center justify-between pt-2">
          <span className="text-primary text-lg font-bold">${product.price}</span>

          <button className="bg-primary text-primary-foreground rounded-lg px-4 py-2 text-sm font-semibold transition-opacity hover:opacity-90">
            {product.clickToPay ? product.clickToPay : 'Buy Now'}
          </button>
        </div>
      </div>
      {/* Actions Menu (if enabled) */}
      {showActions && (
        <div className="border-border mt-3 flex justify-end space-x-2 border-t pt-3">
          <button
            onClick={(e) => {
              e.stopPropagation();
              onEdit?.();
            }}
            className="text-muted-foreground hover:text-foreground rounded px-2 py-1 text-xs"
          >
            Edit
          </button>
          <button
            onClick={(e) => {
              e.stopPropagation();
              onDelete?.();
            }}
            className="text-muted-foreground hover:text-destructive rounded px-2 py-1 text-xs"
          >
            Delete
          </button>
        </div>
      )}
    </div>
  );
}
