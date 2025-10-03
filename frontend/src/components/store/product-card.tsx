import { cn } from '@/lib/cn';
import { Button } from '../ui';
import type { Product } from '@/api/stores';
import type { ProductFormData } from '@/features';

type DisplayMode = 'full' | 'compact';

export type ProductCardType = Pick<Product, 'thumbnailPicture' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType'>;

interface ProductCardProps {
  product: ProductCardType | ProductFormData;
  onClick?: () => void;
  displayMode?: DisplayMode;
  className?: string;
}

export function ProductCard({ product, onClick, className, displayMode = 'full' }: ProductCardProps) {
  return (
    <div
      className={cn(
        'bg-card border-border w-full rounded-2xl border p-4 shadow-sm',
        'transition-shadow hover:shadow-md',
        onClick && 'cursor-pointer',
        className,
      )}
      onClick={onClick}
    >
      <div className="flex flex-col gap-4">
        {displayMode === 'full' && product.thumbnailPicture?.mainLink && (
          <img src={product.thumbnailPicture?.mainLink} alt={product.title} className="h-full w-full rounded-2xl object-cover" />
        )}
        {/* Top row: thumbnail | title/subtitle (flexible) | price (fixed) */}
        <div className="flex w-full items-start justify-between">
          <div className="flex min-w-0 items-start gap-3">
            {/* 
              <div className={cn('flex h-12 w-12 flex-shrink-0 items-center justify-center overflow-hidden rounded-lg')}>
                <span className="text-4xl">{product.productType === 'Session' ? '📅' : '📁'}</span>
              </div>
                 */}

            <div className="min-w-0 text-left">
              <h3 className="text-foreground line-clamp-2 font-semibold break-words">{product.title}</h3>
              {product.subtitle && <p className="text-muted-foreground line-clamp-2 text-sm break-words">{product.subtitle}</p>}
            </div>
          </div>

          <div className="ml-4 flex-none text-right">
            <span className="text-primary text-lg font-bold">${product.price}</span>
          </div>
        </div>

        <Button className="bg-primary text-primary-foreground w-full rounded-3xl px-3 py-1.5 text-sm font-semibold transition-opacity hover:opacity-90">
          {product.clickToPay ? product.clickToPay : 'Buy Now'}
        </Button>
      </div>
    </div>
  );
}
