import { cn } from '@/lib/cn';
import { ProductCard } from './product-card';
import type { Product } from '@/api/stores';

type DisplayMode = 'full' | 'compact';

interface DraggableProductListProps {
  products: Product[];
  onProductClick?: (product: Product) => void;
  onProductEdit?: (product: Product) => void;
  onProductDelete?: (product: Product) => void;
  onReorder: (reorderedProducts: Product[]) => void;
  showActions?: boolean;
  displayMode?: DisplayMode;
  className?: string;
}

export function DraggableProductList({
  products,
  onProductClick,
  onProductEdit,
  onProductDelete,
  showActions = false,
  displayMode = 'full',
  className,
}: DraggableProductListProps) {
  return (
    <div className={cn('space-y-4', className)}>
      {products.map((product, index) => (
        <div
          key={product.storeSlug + '-' + product.storeSlug}
          className={cn('relative transition-all duration-200', 'scale-95 opacity-50', 'translate-y-1 transform', 'cursor-move')}
        >
          {/* Drag handle (only visible when actions are shown) */}
          {showActions && (
            <div className="bg-card border-border absolute top-4 left-2 z-10 rounded border p-2 opacity-0 shadow-sm transition-opacity group-hover:opacity-100">
              <div className="grid h-3 w-3 grid-cols-2 gap-0.5">
                <div className="bg-muted-foreground h-1 w-1 rounded-full"></div>
                <div className="bg-muted-foreground h-1 w-1 rounded-full"></div>
                <div className="bg-muted-foreground h-1 w-1 rounded-full"></div>
                <div className="bg-muted-foreground h-1 w-1 rounded-full"></div>
                <div className="bg-muted-foreground h-1 w-1 rounded-full"></div>
                <div className="bg-muted-foreground h-1 w-1 rounded-full"></div>
              </div>
            </div>
          )}

          <div className={cn(showActions && 'group')}>
            <ProductCard
              product={product}
              onClick={() => onProductClick?.(product)}
              onEdit={() => onProductEdit?.(product)}
              onDelete={() => onProductDelete?.(product)}
              showActions={showActions}
              displayMode={displayMode}
              className={showActions ? '' : ''}
            />
          </div>
        </div>
      ))}
    </div>
  );
}
