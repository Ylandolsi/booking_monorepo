import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { MoreVertical, GripVertical, Calendar, Download, Link, Mail, Tag } from 'lucide-react';
import { Button, DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui';
import { cn } from '@/lib/cn';
import { LazyImage } from '@/utils';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import type { Product } from '@/api/stores';

interface SortableProductItemProps {
  product: Product;
  onEdit?: (product: Product) => void;
  onDelete?: (product: Product) => void;
  onView?: (product: Product) => void;
}

const getProductIcon = (productType: string) => {
  switch (productType) {
    case 'Session':
      return <Calendar className="h-4 w-4 text-blue-600" />;
    case 'DigitalDownload':
      return <Download className="h-4 w-4 text-purple-600" />;
    case 'Link':
      return <Link className="h-4 w-4 text-green-600" />;
    case 'Email':
      return <Mail className="h-4 w-4 text-orange-600" />;
    default:
      return <Tag className="h-4 w-4 text-gray-600" />;
  }
};

export function SortableProductItem({ product, onEdit, onDelete, onView }: SortableProductItemProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({ id: product.productSlug });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      className={cn(
        'group bg-card relative flex items-center gap-3 rounded-xl border p-3 shadow-sm transition-all',
        'hover:bg-accent/5 hover:shadow-md',
        isDragging && 'opacity-50 shadow-lg',
      )}
    >
      {/* Drag Handle */}
      <div
        {...listeners}
        {...attributes}
        className="text-muted-foreground/60 hover:text-muted-foreground hover:bg-accent/50 flex cursor-grab items-center justify-center rounded-md p-2 transition-colors active:cursor-grabbing"
        title="Drag to reorder"
      >
        <GripVertical className="h-4 w-4" />
      </div>

      {/* Product Type Icon */}
      <div className="flex items-center justify-center">{getProductIcon(product.productType)}</div>

      {/* Product Image */}
      <LazyImage
        src={product?.thumbnail?.mainLink ?? FALLBACK_PROFILE_PICTURE}
        placeholder={product?.thumbnail?.thumbnailLink ?? FALLBACK_PROFILE_PICTURE}
        alt={product.title}
        className="h-12 w-12 rounded-lg border object-cover"
      />

      {/* Product Info */}
      <div className="min-w-0 flex-1">
        <div className="flex items-start justify-between">
          <div className="min-w-0 flex-1">
            <h3 className="text-foreground truncate font-semibold">{product.title}</h3>
            {product.subtitle && <p className="text-muted-foreground truncate text-sm">{product.subtitle}</p>}
            <div className="mt-1 flex items-center gap-2">
              <span className="text-primary text-sm font-medium">${product.price}</span>
              {!product.isPublished && <span className="bg-muted text-muted-foreground rounded-full px-2 py-0.5 text-xs">Draft</span>}
            </div>
          </div>
        </div>
      </div>

      {/* Action Menu */}
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button
            variant="ghost"
            size="sm"
            className="text-muted-foreground/60 hover:text-muted-foreground h-8 w-8 p-0 opacity-0 transition-opacity group-hover:opacity-100"
          >
            <MoreVertical className="h-4 w-4" />
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-32">
          {onView && <DropdownMenuItem onClick={() => onView(product)}>View</DropdownMenuItem>}
          {onEdit && <DropdownMenuItem onClick={() => onEdit(product)}>Edit</DropdownMenuItem>}
          {onDelete && (
            <DropdownMenuItem onClick={() => onDelete(product)} className="text-destructive focus:text-destructive">
              Delete
            </DropdownMenuItem>
          )}
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  );
}
