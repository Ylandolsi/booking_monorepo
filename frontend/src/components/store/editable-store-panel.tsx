import { useState, useEffect } from 'react';
import { DndContext, closestCorners, DragOverlay } from '@dnd-kit/core';
import type { DragEndEvent, DragStartEvent } from '@dnd-kit/core';
import { arrayMove, SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable';
import { Button } from '@/components/ui';
import { SortableProductItem } from './sortable-product-item';
import { LazyImage } from '@/utils';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import { socialPlatforms } from '@/features/app/store/create/setup-store';
import { Plus, Calendar, Download, Link, Mail, Tag } from 'lucide-react';
import type { Product, Store } from '@/api/stores';

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

interface EditableStorePanelProps {
  store: Store;
  products: Product[];
  onProductsReorder: (products: Product[]) => void;
  onProductEdit?: (product: Product) => void;
  onProductDelete?: (product: Product) => void;
  onProductView?: (product: Product) => void;
  onAddProduct?: () => void;
}

export function EditableStorePanel({
  store,
  products,
  onProductsReorder,
  onProductEdit,
  onProductDelete,
  onProductView,
  onAddProduct,
}: EditableStorePanelProps) {
  const [localProducts, setLocalProducts] = useState(products);
  const [activeProduct, setActiveProduct] = useState<Product | null>(null);

  const handleDragStart = (event: DragStartEvent) => {
    const { active } = event;
    const product = localProducts.find((p) => p.productSlug === active.id);
    setActiveProduct(product || null);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;

    setActiveProduct(null);

    if (over && active.id !== over.id) {
      const oldIndex = localProducts.findIndex((item) => item.productSlug === active.id);
      const newIndex = localProducts.findIndex((item) => item.productSlug === over.id);

      const reorderedProducts = arrayMove(localProducts, oldIndex, newIndex);
      setLocalProducts(reorderedProducts);
      onProductsReorder(reorderedProducts);
    }
  };

  // Update local products when prop changes
  useEffect(() => {
    setLocalProducts(products);
  }, [products]);

  return (
    <div className="w-full max-w-md space-y-4">
      {/* Store Header */}
      <div className="bg-card rounded-xl border p-4 shadow-sm">
        <div className="flex gap-3">
          <LazyImage
            src={store?.picture?.mainLink ?? FALLBACK_PROFILE_PICTURE}
            placeholder={store?.picture?.thumbnailLink ?? FALLBACK_PROFILE_PICTURE}
            alt="Store Profile Picture"
            className="h-16 w-16 rounded-xl border object-cover"
          />
          <div className="min-w-0 flex-1">
            <h2 className="text-primary truncate text-lg font-bold">{store.title || 'Your Store Name'}</h2>
            <p className="text-primary/80 text-sm">@{store.slug}</p>
            {store.description && <p className="text-muted-foreground mt-1 line-clamp-2 text-sm">{store.description}</p>}
            {store.socialLinks && store.socialLinks.length > 0 && (
              <div className="mt-2 flex gap-2">
                {socialPlatforms.slice(0, 4).map(
                  ({ key, icon: Icon }) =>
                    store.socialLinks?.find((link: any) => link.platform === key)?.url && (
                      <a
                        key={key}
                        href={store.socialLinks?.find((link: any) => link.platform === key)?.url}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="text-primary hover:text-foreground transition-colors"
                      >
                        <div className="bg-primary/10 flex h-6 w-6 items-center justify-center rounded-lg">
                          <Icon className="h-3 w-3" />
                        </div>
                      </a>
                    ),
                )}
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Products List */}
      <div className="space-y-3">
        <div className="flex items-center justify-between">
          <h3 className="text-foreground font-semibold">Products</h3>
          <span className="text-muted-foreground text-sm">
            {localProducts.length} {localProducts.length === 1 ? 'item' : 'items'}
          </span>
        </div>

        {localProducts.length > 0 ? (
          <DndContext
            collisionDetection={closestCorners}
            onDragStart={handleDragStart}
            onDragEnd={handleDragEnd}
          >
            <SortableContext items={localProducts.map((p) => p.productSlug)} strategy={verticalListSortingStrategy}>
              <div className="space-y-2">
                {localProducts.map((product) => (
                  <SortableProductItem
                    key={product.productSlug}
                    product={product}
                    onEdit={onProductEdit}
                    onDelete={onProductDelete}
                    onView={onProductView}
                  />
                ))}
              </div>
            </SortableContext>
            <DragOverlay>
              {activeProduct ? (
                <div className="bg-card border shadow-lg rounded-xl p-3 opacity-90 rotate-3">
                  <div className="flex items-center gap-3">
                    <div className="flex items-center justify-center">
                      {getProductIcon(activeProduct.productType)}
                    </div>
                    <div className="flex-1 min-w-0">
                      <h3 className="font-semibold truncate">{activeProduct.title}</h3>
                      <span className="text-sm font-medium text-primary">${activeProduct.price}</span>
                    </div>
                  </div>
                </div>
              ) : null}
            </DragOverlay>
          </DndContext>
        ) : (
          <div className="border-muted-foreground/20 rounded-xl border-2 border-dashed p-8 text-center">
            <div className="text-muted-foreground">
              <p className="text-sm">No products yet</p>
              <p className="mt-1 text-xs">Add your first product to get started</p>
            </div>
          </div>
        )}

        {/* Add Product Button */}
        <Button
          onClick={onAddProduct}
          variant="outline"
          className="border-primary/30 text-primary hover:bg-primary/5 w-full justify-center gap-2 border-dashed"
        >
          <Plus className="h-4 w-4" />
          Add Product
        </Button>
      </div>
    </div>
  );
}
