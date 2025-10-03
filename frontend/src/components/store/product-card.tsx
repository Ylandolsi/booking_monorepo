import { cn } from '@/lib/cn';
import { Button } from '../ui';
import type { Product } from '@/api/stores';
import { COVER_IMAGE } from '@/features/public/checkout-product-page';
import { useSortable } from '@dnd-kit/sortable';
import { routes } from '@/config/routes';
import { useAppNavigation } from '@/hooks';
import { Grip } from 'lucide-react';
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
        'group/card bg-card/50 border-border/50 w-full rounded-xl border p-5 shadow-sm backdrop-blur-sm',
        'transition-all duration-300 ease-in-out',
        'hover:border-border hover:bg-card hover:shadow-lg',
        onClick && 'cursor-pointer',
        className,
        edit && 'relative',
      )}
      ref={setNodeRef}
      style={style}
      onClick={onClick}
    >
      {edit && (
        <div
          {...attributes}
          {...listeners}
          className="bg-muted/80 hover:bg-muted text-muted-foreground hover:text-foreground absolute top-3 right-3 z-10 cursor-grab rounded-lg p-2 backdrop-blur-sm transition-all duration-200 active:scale-95 active:cursor-grabbing"
        >
          <Grip className="h-4 w-4" />
        </div>
      )}
      <div className="flex h-full w-full flex-col gap-4">
        {displayMode === 'full' && product.thumbnailPicture?.mainLink && (
          <div className="overflow-hidden rounded-lg">
            <img
              src={product.thumbnailPicture?.mainLink}
              alt={product.title}
              className="h-48 w-full object-cover transition-transform duration-300 group-hover/card:scale-105"
            />
          </div>
        )}

        {/* Content Section */}
        <div className="flex flex-1 flex-col gap-3">
          <div className="flex w-full items-start justify-between gap-4">
            <div className="min-w-0 flex-1 text-left">
              <h3 className="text-foreground mb-1 line-clamp-2 text-base leading-tight font-semibold break-words">{product.title}</h3>
              {product.subtitle && <p className="text-muted-foreground line-clamp-2 text-sm leading-relaxed break-words">{product.subtitle}</p>}
            </div>

            <div className="flex-none">
              <span className="text-primary text-xl font-bold tracking-tight">${product.price}</span>
            </div>
          </div>

          <Button
            onClick={onActionClick}
            variant={edit ? 'outline' : 'default'}
            className={cn(
              'h-10 w-full rounded-lg text-sm font-medium transition-all duration-200',
              edit ? 'hover:bg-accent hover:text-accent-foreground' : 'hover:scale-[1.02] hover:opacity-90',
            )}
          >
            {edit ? 'Edit Product' : product.clickToPay ? product.clickToPay : 'Buy Now'}
          </Button>
        </div>
      </div>
    </div>
  );
}
