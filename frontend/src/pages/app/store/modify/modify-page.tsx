import { useEffect, useState, type Dispatch, type SetStateAction } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { User, CheckCircle, Plus, Grip, Store as StoreIcon } from 'lucide-react';
import routes from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import {
  patchPostStoreSchema,
  useDeleteProduct,
  useMyStore,
  useRearrangeProducts,
  useToggleProduct,
  useUpdateStore,
  type PatchPostStoreRequest,
  type Picture,
  type Product,
  type Store,
} from '@/api/stores';
import { useUploadPicture } from '@/hooks/use-upload-picture';
import { MobilePreview, SocialLinksForm } from '@/pages/app/store';
import { ErrorComponenet, LoadingState, ProductCard, UploadImage } from '@/components';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { useAppNavigation, useDeepCompareEffect } from '@/hooks';
import { SortableContext, rectSortingStrategy, sortableKeyboardCoordinates, arrayMove } from '@dnd-kit/sortable';
import { DndContext, PointerSensor, KeyboardSensor, useSensor, useSensors } from '@dnd-kit/core';
import { GenerateIdCrypto } from '@/lib';
import { InputToCopy } from '@/components/input-to-copy';

export type StoreFormData = PatchPostStoreRequest & { picture?: Picture };

export function ModifyStore() {
  const { data: store, isLoading, isError } = useMyStore();
  const { croppedImageUrl, setAspectRatio, handleCloseDialog } = useUploadPicture();
  const [products, setProducts] = useState<Product[]>(store?.products || []);

  const form = useForm<StoreFormData>({
    resolver: zodResolver(patchPostStoreSchema),
    defaultValues: {
      slug: store?.slug || '',
      title: store?.title || '',
      description: store?.description || '',
      socialLinks: store?.socialLinks || [],
      file: undefined,

      // ui :
      picture: store?.picture,
    },
  });

  useEffect(() => {
    setAspectRatio(1 / 1); // Set aspect ratio to 1:1 for store profile picuture
  }, []);

  useEffect(() => {
    if (croppedImageUrl) {
      form.setValue('picture', { mainLink: croppedImageUrl || '', thumbnailLink: croppedImageUrl || '' });
    } else {
      // restore to default
      form.setValue('picture', {
        mainLink: store?.picture?.mainLink || '  ',
        thumbnailLink: store?.picture?.thumbnailLink || store?.picture?.mainLink || '',
      });
    }
  }, [croppedImageUrl, form]);

  if (isLoading) return <LoadingState type="spinner" />;

  if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  const watchedValues = form.watch();

  return (
    <div className="mx-auto flex min-h-screen max-w-7xl flex-col items-center justify-around gap-12 px-4 py-8 lg:flex-row lg:items-start lg:px-6">
      <style>{`.material-symbols-outlined { font-variation-settings: "FILL" 0, "wght" 400, "GRAD" 0, "opsz" 24; }`}</style>
      <UploadPictureDialog onUpload={(file) => form.setValue('file', file)} />
      <aside className="flex w-full max-w-lg flex-col gap-6">
        <div className="flex-1 space-y-2">
          {/* Store Details Section */}
          <StoreSection form={form} handleCloseDialog={handleCloseDialog} />
          {/* Products Section */}
          <ProductSection products={products} setProducts={setProducts} store={store} />
        </div>
      </aside>
      <div>
        <PreviewUrl store={store} />
        <MobilePreview storeForm={watchedValues} productsRearranged={products} />
      </div>
    </div>
  );
}

const PreviewUrl = ({ store }: { store: Store }) => {
  const link = window.location.origin + '/store/' + store.slug;
  return <InputToCopy input={link || ''} className="mb-4" label={'Store Public Link'} />;
};

const ProductSection = ({
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
                      <ProductCard key={GenerateIdCrypto()} product={item} edit={true} onActionClick={() => handleProductEdit(item)} />
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

const StoreSection = ({ form, handleCloseDialog }: { form: ReturnType<typeof useForm<StoreFormData>>; handleCloseDialog: () => void }) => {
  const updateStoreMutation = useUpdateStore();
  const navigate = useAppNavigation();
  const onSubmit = async (data: PatchPostStoreRequest) => {
    try {
      console.log('Submitting store data:', data);
      // todo : handle this api
      await updateStoreMutation.mutateAsync(data);

      // after upadte clean the cropped image
      handleCloseDialog();

      navigate.goTo({ to: routes.to.store.index() + '/' });
    } catch (error) {
      console.error('Failed to update store:', error);
    }
  };

  return (
    <div className="bg-card/50 border-border/50 rounded-xl border shadow-sm backdrop-blur-sm">
      <Accordion type="single" collapsible className="w-full">
        <AccordionItem value="store-details" className="border-0">
          <AccordionTrigger className="hover:bg-accent/50 rounded-t-xl px-6 py-5 transition-colors hover:no-underline">
            <div className="flex items-center gap-3">
              <div className="bg-primary/10 text-primary rounded-lg p-2">
                <StoreIcon className="h-5 w-5" />
              </div>
              <h2 className="text-foreground text-xl font-semibold">Store Details</h2>
            </div>
          </AccordionTrigger>
          <AccordionContent className="px-6 pt-2 pb-6">
            <div className="space-y-5">
              <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">
                  <FormField
                    control={form.control}
                    name="title"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="text-foreground flex items-center gap-2 text-sm font-medium">
                          <User className="h-4 w-4" />
                          Store Name *
                        </FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Your Amazing Store"
                            className="border-border text-foreground h-11 rounded-lg"
                            {...field}
                            onChange={(e) => {
                              field.onChange(e);
                            }}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  <FormField
                    control={form.control}
                    name="description"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="text-foreground text-sm font-medium">Store Description</FormLabel>
                        <FormControl>
                          <Textarea
                            placeholder="Tell your customers what you offer..."
                            rows={3}
                            className="border-border text-foreground resize-none rounded-lg"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <UploadImage description="Profile Picture (Optional)" />

                  <SocialLinksForm form={form} />

                  <Button
                    type="submit"
                    size="lg"
                    className="h-11 w-full rounded-lg font-medium transition-all duration-200"
                    disabled={updateStoreMutation.isPending}
                  >
                    {updateStoreMutation.isPending ? (
                      'Updating Store...'
                    ) : (
                      <>
                        <CheckCircle className="h-4 w-4" />
                        Update Store Details
                      </>
                    )}
                  </Button>
                </form>
              </Form>
            </div>{' '}
          </AccordionContent>
        </AccordionItem>
      </Accordion>
    </div>
  );
};
