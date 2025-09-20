import { cn } from '@/lib/cn';
import { ProductCard } from './product-card';
import type { Product } from '@/types/product';

type DisplayMode = 'full' | 'compact';

interface ProductListProps {
  products: Product[];
  onProductClick?: (product: Product) => void;
  onProductEdit?: (product: Product) => void;
  onProductDelete?: (product: Product) => void;
  onReorder?: (productIds: string[]) => void;
  showActions?: boolean;
  displayMode?: DisplayMode;
  className?: string;
}

export function ProductList({
  products,
  onProductClick,
  onProductEdit,
  onProductDelete,
  onReorder,
  showActions = false,
  displayMode = 'full',
  className,
}: ProductListProps) {
  return (
    <div className={cn('space-y-4', className)}>
      {products.map((product) => (
        <ProductCard
          key={product.id}
          product={product}
          onClick={() => onProductClick?.(product)}
          onEdit={() => onProductEdit?.(product)}
          onDelete={() => onProductDelete?.(product)}
          showActions={showActions}
          displayMode={displayMode}
        />
      ))}
    </div>
  );
}
