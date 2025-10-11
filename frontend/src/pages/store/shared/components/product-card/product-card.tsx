import { cn } from '@/lib/cn';
import { routes } from '@/config/routes';
import { useAppNavigation } from '@/hooks';
import { BrushCleaning, EllipsisVertical, Globe, Move } from 'lucide-react';
import { Label, Button, Popover, PopoverContent, PopoverTrigger, Switch } from '@/components/ui';
import { ProductCardProvider, useProductCardContext } from './context';
import { useProductCard } from './use-product-card';
import type { ProductCardProps } from './types';

export function ProductCard({ product, onClick, className, edit = false, onActionClick, setProducts }: ProductCardProps) {
  const cardHook = useProductCard({ product, edit, setProducts });

  return (
    <ProductCardProvider product={product} edit={edit} onActionClick={onActionClick}>
      <div
        className={cn(
          'group/card bg-card/50 w-full rounded-xl border p-5 shadow-sm backdrop-blur-sm',
          'transition-all duration-300 ease-in-out',
          'hover:border-border hover:bg-card hover:shadow-lg',
          onClick && 'cursor-pointer',
          className,
          edit && 'relative',
          product.productStyle === 'Minimal' && 'p-3',
        )}
        ref={cardHook.setNodeRef}
        style={cardHook.style}
        onClick={onClick}
      >
        {edit && <ProductCard.DragHandle dragHandleProps={cardHook.dragHandleProps} />}

        <div className="flex h-full w-full flex-col gap-4">
          {product.productStyle === 'Compact' && (
            <>
              <div className="flex w-full gap-4">
                <ProductCard.Image />
                <ProductCard.Content
                  isPopoverOpen={cardHook.isPopoverOpen}
                  setIsPopoverOpen={cardHook.setIsPopoverOpen}
                  handleTogglePublished={cardHook.handleTogglePublished}
                  handleDeleteProduct={cardHook.handleDeleteProduct}
                />
              </div>
              <ProductCard.Footer />
            </>
          )}
          {product.productStyle === 'Full' && (
            <>
              <ProductCard.Image />
              <ProductCard.Content
                isPopoverOpen={cardHook.isPopoverOpen}
                setIsPopoverOpen={cardHook.setIsPopoverOpen}
                handleTogglePublished={cardHook.handleTogglePublished}
                handleDeleteProduct={cardHook.handleDeleteProduct}
              />
              <ProductCard.Footer />
            </>
          )}
          {product.productStyle === 'Minimal' && (
            <div className="flex w-full items-center gap-4">
              <ProductCard.Image />
              <ProductCard.Content
                isPopoverOpen={cardHook.isPopoverOpen}
                setIsPopoverOpen={cardHook.setIsPopoverOpen}
                handleTogglePublished={cardHook.handleTogglePublished}
                handleDeleteProduct={cardHook.handleDeleteProduct}
              />
            </div>
          )}
        </div>
      </div>
    </ProductCardProvider>
  );
}

// Drag Handle
ProductCard.DragHandle = function DragHandle({ dragHandleProps }: { dragHandleProps: any }) {
  return (
    <div
      {...dragHandleProps}
      className="bg-muted/80 hover:bg-muted text-muted-foreground hover:text-foreground absolute -top-2 left-[200px] -z-2 cursor-grab rounded-lg p-1 backdrop-blur-sm transition-all duration-200 active:scale-95 active:cursor-grabbing"
    >
      <Move className="h-4 w-4" />
    </div>
  );
};

// Image Section
ProductCard.Image = function Image() {
  const { product } = useProductCardContext();

  if (!product.thumbnailPicture?.mainLink) {
    return null;
  }

  return (
    <div
      className={cn(
        'mx-auto rounded-lg transition-all',
        product.productStyle === 'Compact' || product.productStyle === 'Minimal' ? 'flex h-14 w-14 items-center justify-center' : 'h-full w-full',
      )}
    >
      <img
        src={product.thumbnailPicture.mainLink}
        alt={product.title}
        className={cn(
          'transition-transform duration-300 group-hover/card:scale-105',
          product.productStyle === 'Compact' || product.productStyle === 'Minimal'
            ? 'min-h-full min-w-full rounded-2xl object-cover object-center'
            : 'h-full w-full rounded-lg',
        )}
      />
    </div>
  );
};

// Content Section (Title, Subtitle, Edit Menu)
interface ContentProps {
  isPopoverOpen: boolean;
  setIsPopoverOpen: (open: boolean) => void;
  handleTogglePublished: (checked: boolean) => void;
  handleDeleteProduct: () => void;
}

ProductCard.Content = function Content({ isPopoverOpen, setIsPopoverOpen, handleTogglePublished, handleDeleteProduct }: ContentProps) {
  const { product, edit } = useProductCardContext();

  return (
    <div className="flex flex-1 flex-col gap-3">
      <div className="flex w-full items-start justify-between gap-4">
        <ProductCard.Title />
        {product.productStyle !== 'Minimal' && <div className="text-primary text-xl font-bold tracking-tight">${product.price}</div>}

        {edit && product.productSlug && (
          <ProductCard.EditMenu
            isPopoverOpen={isPopoverOpen}
            setIsPopoverOpen={setIsPopoverOpen}
            handleTogglePublished={handleTogglePublished}
            handleDeleteProduct={handleDeleteProduct}
          />
        )}
      </div>
    </div>
  );
};

// Title and Subtitle
ProductCard.Title = function Title() {
  const { product } = useProductCardContext();

  return (
    <div className="min-w-0 flex-1 text-left">
      <h3 className="text-foreground mb-1 line-clamp-2 text-base leading-tight font-semibold break-words">{product.title}</h3>
      {product.productStyle !== 'Minimal' && (
        <>{product.subtitle && <p className="text-muted-foreground line-clamp-2 text-sm leading-relaxed break-all">{product.subtitle}</p>}</>
      )}
    </div>
  );
};

// Edit Menu (Price + Actions)
interface EditMenuProps {
  isPopoverOpen: boolean;
  setIsPopoverOpen: (open: boolean) => void;
  handleTogglePublished: (checked: boolean) => void;
  handleDeleteProduct: () => void;
}

ProductCard.EditMenu = function EditMenu({ isPopoverOpen, setIsPopoverOpen, handleTogglePublished, handleDeleteProduct }: EditMenuProps) {
  const { product } = useProductCardContext();

  return (
    <div className="flex items-center">
      <Popover open={isPopoverOpen} onOpenChange={setIsPopoverOpen}>
        <PopoverTrigger asChild>
          <div
            className="text-foreground hover:bg-accent/80 cursor-pointer rounded-lg p-1 transition-all duration-200"
            onClick={() => setIsPopoverOpen(true)}
          >
            <EllipsisVertical className="h-4 w-4" />
          </div>
        </PopoverTrigger>
        <PopoverContent className="w-72 p-4" side="top" align="center">
          <div className="grid gap-4">
            <div className="space-y-1">
              <h4 className="text-sm font-semibold">Product Options</h4>
              <p className="text-muted-foreground text-xs">Manage your product settings</p>
            </div>
            <div className="grid gap-3">
              <div className="hover:bg-accent/50 flex items-center space-x-3 rounded-lg p-2 transition-colors">
                <Globe className="text-muted-foreground h-4 w-4" />
                <Label htmlFor="featured-toggle" className="flex flex-1 cursor-pointer items-center gap-2 text-sm font-medium">
                  Published
                </Label>
                <Switch
                  id="featured-toggle"
                  className="data-[state=checked]:bg-green-600"
                  defaultChecked={product.isPublished}
                  onCheckedChange={(checked) => handleTogglePublished(checked as boolean)}
                />
              </div>
              <Button type="button" variant="destructive" size="sm" onClick={handleDeleteProduct} className="mt-1 h-9 w-full rounded-lg">
                <BrushCleaning className="h-4 w-4" />
                Delete Product
              </Button>
            </div>
          </div>
        </PopoverContent>
      </Popover>
    </div>
  );
};

// Footer (Action Button)
ProductCard.Footer = function Footer() {
  const { product, edit, onActionClick } = useProductCardContext();
  const navigate = useAppNavigation();

  const handleClick = () => {
    if (onActionClick) {
      onActionClick();
    } else if (edit && product.productSlug) {
      navigate.goTo({ to: routes.to.store.productEdit({ productSlug: product.productSlug, type: product.productType }) });
    }
  };

  return (
    <Button
      onClick={handleClick}
      variant={edit ? 'outline' : 'default'}
      className={cn(
        'h-10 w-full rounded-lg text-sm font-medium transition-all duration-200',
        edit ? 'hover:bg-accent hover:text-accent-foreground' : 'hover:scale-[1.02] hover:opacity-90',
      )}
    >
      {edit ? 'Edit Product' : product.clickToPay ? product.clickToPay : 'Buy Now'}
    </Button>
  );
};
