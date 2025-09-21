import { cn } from '@/lib/cn';
import type { Product } from '@/types/product';
import { Button } from '../ui';

type DisplayMode = 'full' | 'compact';

interface ProductCardProps {
  product: Product;
  onClick?: () => void;
  showActions?: boolean;
  displayMode?: DisplayMode;
  onEdit?: () => void;
  onDelete?: () => void;
  className?: string;
}

export function ProductCard({ product, onClick, showActions = false, displayMode = 'full', onEdit, onDelete, className }: ProductCardProps) {
  // Determine the effective thumbnail mode
  const thumbnailMode = product.thumbnailMode || 'expanded';

  if (displayMode === 'compact') {
    return (
      <div
        className={cn(
          'bg-card rounded-xl p-4 shadow-sm border border-border',
          'hover:shadow-md transition-shadow',
          onClick && 'cursor-pointer',
          className,
        )}
        onClick={onClick}
      >
        <div className="flex flex-col items-stretch gap-4">
          {/* Top row: thumbnail | title/subtitle (flexible) | price (fixed) */}
          <div className="flex items-start w-full gap-3">
            <div
              className={cn(
                'rounded-lg bg-muted flex items-center justify-center flex-shrink-0 overflow-hidden',
                thumbnailMode === 'compact' ? 'w-8 h-8' : 'w-12 h-12',
              )}
            >
              {product.coverImage ? (
                <img src={product.coverImage} alt={product.title} className="w-full h-full object-cover" />
              ) : (
                <span className={cn(thumbnailMode === 'compact' ? 'text-base' : 'text-2xl')}>{product.type === 'booking' ? '📅' : '📁'}</span>
              )}
            </div>

            <div className="flex-1 min-w-0">
              <h3 className="font-semibold text-foreground  overflow-hidden break-words">{product.title}</h3>
              {product.subtitle && <p className="text-sm text-muted-foreground break-words overflow-hidden">{product.subtitle}</p>}
            </div>

            <div className="flex-none ml-2 text-right">
              <span className="text-lg font-bold text-primary">{product.price}</span>
            </div>
          </div>

          <Button className="bg-primary text-primary-foreground px-3 py-1.5 rounded-lg text-sm font-semibold hover:opacity-90 transition-opacity w-full">
            {product.ctaText}
          </Button>
        </div>

        {showActions && (
          <div className="mt-3 pt-3 border-t border-border flex justify-end space-x-2">
            <button
              onClick={(e) => {
                e.stopPropagation();
                onEdit?.();
              }}
              className="text-xs text-muted-foreground hover:text-foreground px-2 py-1 rounded"
            >
              Edit
            </button>
            <button
              onClick={(e) => {
                e.stopPropagation();
                onDelete?.();
              }}
              className="text-xs text-muted-foreground hover:text-destructive px-2 py-1 rounded"
            >
              Delete
            </button>
          </div>
        )}
      </div>
    );
  }

  // Full display mode with thumbnail mode support
  return (
    <div
      className={cn(
        'bg-card rounded-xl p-4 shadow-sm border border-border',
        'hover:shadow-md transition-shadow',
        onClick && 'cursor-pointer',
        className,
      )}
      onClick={onClick}
    >
      {/* Cover Image with conditional layout */}
      {thumbnailMode === 'expanded' ? (
        <div className="aspect-square rounded-lg mb-3 overflow-hidden bg-muted">
          {product.coverImage ? (
            <img src={product.coverImage} alt={product.title} className="w-full h-full object-cover" />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-muted-foreground">
              <div className="text-center">
                <div className="text-3xl mb-2">{product.type === 'booking' ? '📅' : '📁'}</div>
                <div className="text-xs">No image</div>
              </div>
            </div>
          )}
        </div>
      ) : (
        <div className="flex items-start space-x-3 mb-3">
          {/* Compact thumbnail in full mode */}
          <div className="w-12 h-12 rounded-lg bg-muted flex items-center justify-center flex-shrink-0">
            {product.coverImage ? (
              <img src={product.coverImage} alt={product.title} className="w-full h-full object-cover rounded-lg" />
            ) : (
              <span className="text-2xl">{product.type === 'booking' ? '📅' : '📁'}</span>
            )}
          </div>

          {/* Title and subtitle aligned with thumbnail */}
          <div className="flex-1 min-w-0 pt-1">
            <h3 className="font-semibold text-foreground mb-1 line-clamp-2">{product.title}</h3>
            {product.subtitle && <p className="text-sm text-muted-foreground line-clamp-2">{product.subtitle}</p>}
          </div>
        </div>
      )}

      {/* Content - price and CTA always at bottom */}
      <div className={cn(thumbnailMode === 'compact' && 'space-y-0', thumbnailMode === 'expanded' && 'space-y-2')}>
        {thumbnailMode === 'expanded' && (
          <>
            <h3 className="font-semibold text-foreground mb-1 line-clamp-2">{product.title}</h3>
            {product.subtitle && <p className="text-sm text-muted-foreground mb-2 line-clamp-2">{product.subtitle}</p>}
          </>
        )}

        {/* Price and CTA */}
        <div className="flex items-center justify-between pt-2">
          <span className="text-lg font-bold text-primary">{product.price}</span>

          <button className="bg-primary text-primary-foreground px-4 py-2 rounded-lg text-sm font-semibold hover:opacity-90 transition-opacity">
            {product.ctaText}
          </button>
        </div>
      </div>

      {/* Actions Menu (if enabled) */}
      {showActions && (
        <div className="mt-3 pt-3 border-t border-border flex justify-end space-x-2">
          <button
            onClick={(e) => {
              e.stopPropagation();
              onEdit?.();
            }}
            className="text-xs text-muted-foreground hover:text-foreground px-2 py-1 rounded"
          >
            Edit
          </button>
          <button
            onClick={(e) => {
              e.stopPropagation();
              onDelete?.();
            }}
            className="text-xs text-muted-foreground hover:text-destructive px-2 py-1 rounded"
          >
            Delete
          </button>
        </div>
      )}
    </div>
  );
}
