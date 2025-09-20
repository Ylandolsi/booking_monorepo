import { cn } from '@/lib/cn';
import { ProductCard } from './product-card';
import type { Product } from '@/types/product';

interface ProductListProps {
  products: Product[];
  onProductClick?: (product: Product) => void;
  onReorder?: (productIds: string[]) => void;
  showActions?: boolean;
  className?: string;
}

export function ProductList({ products, onProductClick, onReorder, showActions = false, className }: ProductListProps) {
  return (
    <div className={cn('space-y-4', className)}>
      {products.map((product) => (
        <ProductCard key={product.id} product={product} onClick={() => onProductClick?.(product)} showActions={showActions} />
      ))}
    </div>
  );
}
