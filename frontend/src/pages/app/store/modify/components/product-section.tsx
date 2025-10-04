import { useState, type Dispatch, type SetStateAction } from 'react';
import { Button } from '@/components/ui/button';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { Plus, Grip } from 'lucide-react';
import routes from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import { useRearrangeProducts, type Product, type Store } from '@/api/stores';
import { ProductCard } from '@/components';
import { useAppNavigation, useDeepCompareEffect } from '@/hooks';
import { SortableContext, rectSortingStrategy, sortableKeyboardCoordinates, arrayMove } from '@dnd-kit/sortable';
import { DndContext, PointerSensor, KeyboardSensor, useSensor, useSensors } from '@dnd-kit/core';
import { GenerateIdCrypto } from '@/lib';

export const ProductSection = ({
  products,
  setProducts,
  store,
}: {
  products: Product[];
  setProducts: Dispatch<SetStateAction<Product[]>>;
  store: Store;
}) => {
  const rearrangeProductsMutation = useRearrangeProducts();

  const sensors = useSensors(
    useSensor(PointerSensor),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    }),
  );
  const [orderChanged, setOrderChanged] = useState<boolean>(false);

  useDeepCompareEffect(() => {
    for (let i = 0; i < products.length; i++) {
      if (products[i].productSlug != store.products[i].productSlug) {
        setOrderChanged(true);
        return;
      }
    }
    setOrderChanged(false);
  }, [products, store.products]);

  console.log('products :', products);

  const handleProductRearrange = async () => {
    if (!orderChanged) return;
    try {
      const productSlugs = products.map((p) => p.productSlug);
      const mappedSlugs = {} as Record<string, number>;
      productSlugs.forEach((slug, index) => {
        mappedSlugs[slug] = index + 1;
      });
      await rearrangeProductsMutation.mutateAsync({ orders: mappedSlugs });
      setOrderChanged(false);
    } catch (error) {
      console.error('Failed to rearrange products:', error);
    }
  };
  function handleProductEdit(product: Product): void {
    navigate.goTo({ to: routes.to.store.productEdit({ productSlug: product.productSlug, type: product.productType }) });
  }

  const handleDragEnd = ({ active, over }: any) => {
    if (!over) {
      return;
    }

    if (active.id === over.id) {
      return;
    }

    setProducts((items) => {
      return arrayMove(
        items,
        items.findIndex((it) => it.productSlug === active.id),
        items.findIndex((it) => it.productSlug === over.id),
      );
    });
  };
  const navigate = useAppNavigation();
  return (
    <div className="bg-card/50 border-border/50 rounded-xl border shadow-sm backdrop-blur-sm">
      <Accordion type="single" collapsible className="w-full">
        <AccordionItem value="products" className="border-0">
          <AccordionTrigger className="hover:bg-accent/50 rounded-t-xl px-6 py-5 transition-colors hover:no-underline">
            <div className="flex w-full items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="bg-primary/10 text-primary rounded-lg p-2">
                  <Grip className="h-5 w-5" />
                </div>
                <h2 className="text-foreground text-xl font-semibold">My Products</h2>
              </div>
              <Button
                variant="ghost"
                size="sm"
                className="bg-primary/10 hover:bg-primary/20 text-primary h-9 w-9 rounded-lg p-0"
                aria-label="Add new product"
                onClick={(e) => {
                  e.stopPropagation();
                  navigate.goTo({ to: routes.paths.APP.STORE.PRODUCT.INDEX + '/' });
                }}
              >
                <Plus className="h-5 w-5" />
              </Button>
            </div>
          </AccordionTrigger>
          <AccordionContent className="flex flex-col px-6 pt-2 pb-6">
            <div className="flex flex-col space-y-3 p-4">
              {products.length !== 0 && (
                <DndContext sensors={sensors} onDragEnd={handleDragEnd}>
                  <SortableContext items={products.map((p) => p.productSlug)} strategy={rectSortingStrategy}>
                    {products.map((item) => (
                      <ProductCard
                        key={GenerateIdCrypto()}
                        product={item}
                        edit={true}
                        onActionClick={() => handleProductEdit(item)}
                        setProducts={setProducts}
                      />
                    ))}
                  </SortableContext>
                </DndContext>
              )}
              {orderChanged && (
                <div className="flex justify-center">
                  <Button
                    onClick={handleProductRearrange}
                    type="button"
                    variant="outline"
                    size="sm"
                    className="rounded-lg"
                    disabled={rearrangeProductsMutation.isPending}
                  >
                    Confirm Rearrange
                  </Button>
                </div>
              )}
            </div>
          </AccordionContent>
        </AccordionItem>
      </Accordion>
    </div>
  );
};
