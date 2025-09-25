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
    <div className={cn('w-full space-y-4', className)}>
      {products.map((product, index) => (
        <div
          key={product.storeSlug + '-' + product.storeSlug}
          className={cn('relative transition-all duration-200', 'scale-95 opacity-50', 'translate-y-1 transform', 'cursor-move')}
        >
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
