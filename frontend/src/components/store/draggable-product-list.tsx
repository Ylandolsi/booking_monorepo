import { useState } from 'react';
import { cn } from '@/lib/cn';
import { ProductCard } from './product-card';
import type { Product } from '@/types/product';

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
  onReorder,
  showActions = false,
  displayMode = 'full',
  className,
}: DraggableProductListProps) {
  const [draggedItem, setDraggedItem] = useState<string | null>(null);
  const [dragOverIndex, setDragOverIndex] = useState<number | null>(null);

  const handleDragStart = (e: React.DragEvent, productId: string) => {
    setDraggedItem(productId);
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/plain', productId);
  };

  const handleDragOver = (e: React.DragEvent, index: number) => {
    e.preventDefault();
    e.dataTransfer.dropEffect = 'move';
    setDragOverIndex(index);
  };

  const handleDragLeave = () => {
    setDragOverIndex(null);
  };

  const handleDrop = (e: React.DragEvent, dropIndex: number) => {
    e.preventDefault();

    if (!draggedItem) return;

    const draggedIndex = products.findIndex((p) => p.id === draggedItem);
    if (draggedIndex === -1 || draggedIndex === dropIndex) {
      setDraggedItem(null);
      setDragOverIndex(null);
      return;
    }

    const reorderedProducts = [...products];
    const [draggedProduct] = reorderedProducts.splice(draggedIndex, 1);
    reorderedProducts.splice(dropIndex, 0, draggedProduct);

    onReorder(reorderedProducts);
    setDraggedItem(null);
    setDragOverIndex(null);
  };

  const handleDragEnd = () => {
    setDraggedItem(null);
    setDragOverIndex(null);
  };

  return (
    <div className={cn('space-y-4', className)}>
      {products.map((product, index) => (
        <div
          key={product.id}
          draggable={showActions}
          onDragStart={(e) => handleDragStart(e, product.id)}
          onDragOver={(e) => handleDragOver(e, index)}
          onDragLeave={handleDragLeave}
          onDrop={(e) => handleDrop(e, index)}
          onDragEnd={handleDragEnd}
          className={cn(
            'relative transition-all duration-200',
            draggedItem === product.id && 'opacity-50 scale-95',
            dragOverIndex === index && 'transform translate-y-1',
            showActions && 'cursor-move',
          )}
        >
          {/* Drop indicator */}
          {dragOverIndex === index && draggedItem && draggedItem !== product.id && (
            <div className="absolute -top-2 left-0 right-0 h-1 bg-primary rounded-full z-10" />
          )}

          {/* Drag handle (only visible when actions are shown) */}
          {showActions && (
            <div className="absolute left-2 top-4 z-10 p-2 rounded bg-card border border-border shadow-sm opacity-0 group-hover:opacity-100 transition-opacity">
              <div className="grid grid-cols-2 gap-0.5 w-3 h-3">
                <div className="w-1 h-1 bg-muted-foreground rounded-full"></div>
                <div className="w-1 h-1 bg-muted-foreground rounded-full"></div>
                <div className="w-1 h-1 bg-muted-foreground rounded-full"></div>
                <div className="w-1 h-1 bg-muted-foreground rounded-full"></div>
                <div className="w-1 h-1 bg-muted-foreground rounded-full"></div>
                <div className="w-1 h-1 bg-muted-foreground rounded-full"></div>
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
              className={showActions ? 'pl-10' : ''}
            />
          </div>
        </div>
      ))}

      {/* Final drop zone */}
      {draggedItem && (
        <div
          onDragOver={(e) => handleDragOver(e, products.length)}
          onDragLeave={handleDragLeave}
          onDrop={(e) => handleDrop(e, products.length)}
          className={cn(
            'h-16 border-2 border-dashed border-muted-foreground/30 rounded-lg flex items-center justify-center transition-colors',
            dragOverIndex === products.length && 'border-primary bg-primary/10',
          )}
        >
          <span className="text-sm text-muted-foreground">Drop here to move to end</span>
        </div>
      )}
    </div>
  );
}
