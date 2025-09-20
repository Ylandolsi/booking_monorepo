import { cn } from '@/lib/cn';
import type { Product } from '@/types/product';

interface ProductCardProps {
  product: Product;
  onClick?: () => void;
  showActions?: boolean;
  className?: string;
}

export function ProductCard({ product, onClick, showActions = false, className }: ProductCardProps) {
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
      {/* Cover Image */}
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

      {/* Content */}
      <div className="space-y-2">
        <h3 className="font-semibold text-foreground mb-1 line-clamp-2">{product.title}</h3>

        {product.subtitle && <p className="text-sm text-muted-foreground mb-2 line-clamp-2">{product.subtitle}</p>}

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
          <button className="text-xs text-muted-foreground hover:text-foreground px-2 py-1 rounded">Edit</button>
          <button className="text-xs text-muted-foreground hover:text-destructive px-2 py-1 rounded">Delete</button>
        </div>
      )}
    </div>
  );
}
