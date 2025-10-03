import { cn } from '@/lib/cn';
import { Button } from '../ui';
import type { Product } from '@/api/stores';
import { COVER_IMAGE } from '@/features/public/checkout-product-page';
import { useSortable } from '@dnd-kit/sortable';
import { routes } from '@/config/routes';
import { useAppNavigation } from '@/hooks';
import { Grip, Move } from 'lucide-react';
import { CSS } from '@dnd-kit/utilities';

type DisplayMode = 'full' | 'compact';

export type ProductCardType = Pick<
  Product,
  'productSlug' | 'thumbnailPicture' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType'
>;

interface ProductCardProps {
  product: ProductCardType;
  onClick?: () => void;
  displayMode?: DisplayMode;
  className?: string;
  edit?: boolean;
  onActionClick?: () => void; // New prop for action button click
}

export function ProductCard({ product, onClick, className, displayMode = 'full', edit = false, onActionClick }: ProductCardProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: product.productSlug || '',
    disabled: !edit || !product.productSlug,
  });
  const navigate = useAppNavigation();

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1, // Optional: Visual feedback during drag
  };

  const editProduct = () => {
    navigate.goTo({ to: routes.to.store.productEdit({ productSlug: product.productSlug, type: product.productType }) });
  };

  return (
    <div
      className={cn(
        'bg-card border-border w-full rounded-2xl border p-4 shadow-sm',
        'transition-shadow hover:shadow-md',
        onClick && 'cursor-pointer',
        className,
        edit && 'relative',
      )}
      ref={setNodeRef}
      style={style}
      onClick={onClick}
    >
      {edit && (
        <div {...attributes} {...listeners} className="bg-secondary text-primary absolute mr-2 cursor-move rounded-3xl p-1 active:cursor-grabbing">
          <Move className="text-foreground" />
        </div>
      )}
      <div className="flex h-full w-full flex-col gap-4">
        {displayMode === 'full' && product.thumbnailPicture?.mainLink && (
          <div className={cn(`min-w-[${COVER_IMAGE.width}] min-h-[${COVER_IMAGE.height}]`)}>
            <img src={product.thumbnailPicture?.mainLink} alt={product.title} className="h-full w-full rounded-2xl object-cover" />
          </div>
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

        {
          <Button
            onClick={onActionClick}
            className="bg-primary text-primary-foreground w-full rounded-3xl px-3 py-1.5 text-sm font-semibold transition-opacity hover:opacity-90"
          >
            {edit ? 'Edit' : product.clickToPay ? product.clickToPay : 'Buy Now'}
          </Button>
        }
      </div>
    </div>
  );
}
