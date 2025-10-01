import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { User, CheckCircle, Globe, Plus, Edit2Icon, Store } from 'lucide-react';
import routes from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import {
  patchPostStoreSchema,
  useCreateStore,
  useMyStore,
  useUpdateStore,
  type PatchPostStoreRequest,
  type Picture,
  type Product,
} from '@/api/stores';
import { useUploadPicture } from '@/hooks/use-upload-picture';
import { MobilePreview, SocialLinksForm } from '@/features/app/store';
import { ErrorComponenet, LoadingState, UploadImage } from '@/components';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { useAppNavigation } from '@/hooks';

export type StoreFormData = PatchPostStoreRequest & { picture?: Picture };

export function ModifyStore() {
  const navigate = useAppNavigation();
  const { data: store, isLoading, isError } = useMyStore();
  const updateStoreMutation = useUpdateStore();

  const { croppedImageUrl, setAspectRatio } = useUploadPicture();

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
    }
  }, [croppedImageUrl, form]);

  if (isLoading) return <LoadingState type="spinner" />;

  if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  const products = store.products || [];

  const onSubmit = async (data: PatchPostStoreRequest) => {
    try {
      console.log('Submitting store data:', data);
      // todo : handle this api
      await updateStoreMutation.mutateAsync(data);

      navigate.goTo({ to: routes.to.store.index() + '/' });
    } catch (error) {
      console.error('Failed to update store:', error);
    }
  };

  function handleProductEdit(product: Product): void {
    navigate.goTo({ to: routes.to.store.productEdit({ productSlug: product.productSlug, type: product.productType }) });
  }

  const watchedValues = form.watch();

  return (
    <div className="mx-auto flex min-h-screen max-w-7xl flex-col items-center justify-around gap-10 lg:flex-row lg:items-start">
      <style>{`.material-symbols-outlined { font-variation-settings: "FILL" 0, "wght" 400, "GRAD" 0, "opsz" 24; }`}</style>
      <UploadPictureDialog onUpload={(file) => form.setValue('file', file)} />
      <aside className="flex w-[460px] flex-col px-6">
        <div className="flex-1">
          <div className="border-primary/20 dark:border-primary/30 border-b p-6">
            <Accordion type="single" collapsible className="w-full">
              <AccordionItem value="store-details">
                <AccordionTrigger className="text-foreground hover:text-primary">
                  <div className="flex items-center gap-2">
                    {/* TODO change this  */}
                    <Store className="h-8 w-8" />
                    <h2 className="text-3xl font-bold">Edit Store Details</h2>
                  </div>
                </AccordionTrigger>
                <AccordionContent className="mt-0 space-y-4 p-2">
                  <div className="max-w-lg flex-1">
                    <div className="mb-8 text-center">
                      {/* <h1 className="from-primary to-chart-4 bg-gradient-to-r bg-clip-text text-3xl font-bold text-transparent">Create Your Linki Store</h1> */}
                      {/* <p className="text-muted-foreground mt-2">Set up your personal mobile store in seconds</p> */}
                    </div>

                    <Form {...form}>
                      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
                        <FormField
                          control={form.control}
                          name="title"
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel className="text-foreground flex items-center gap-2">
                                <User className="h-4 w-4" />
                                Store Name *
                              </FormLabel>
                              <FormControl>
                                <Input
                                  placeholder="Your Amazing Store"
                                  className="border-border text-foreground py-3 text-lg"
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
                              <FormLabel className="text-foreground">Store Description</FormLabel>
                              <FormControl>
                                <Textarea
                                  placeholder="Tell your customers what you offer..."
                                  rows={3}
                                  className="border-border text-foreground"
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
                          className="hover:bg-primary hover:accent text-primary-foreground w-full transition-all duration-300"
                          disabled={updateStoreMutation.isPending}
                        >
                          {updateStoreMutation.isPending ? (
                            'Updating Store...'
                          ) : (
                            <>
                              <CheckCircle className="mr-2 h-4 w-4" />
                              Update store details
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
          <div className="flex h-16 items-center justify-between p-6">
            <h2 className="text-3xl font-bold">My Products</h2>
            <Button
              variant={'ghost'}
              className="bg-primary/10 hover:bg-primary/20 dark:bg-primary/20 dark:hover:bg-primary/30 text-primary flex items-center justify-center transition-colors"
              aria-label="Add new product"
              onClick={() => navigate.goTo({ to: routes.paths.APP.STORE.PRODUCT.INDEX + '/' })}
            >
              <Plus className="h-5 w-5" />
            </Button>
          </div>
          {products.length !== 0 &&
            products.map((product) => (
              <div className="space-y-4 px-6 py-2">
                <div className="group border-primary/20 dark:border-primary/30 bg-card-light dark:bg-card-dark hover:border-primary/40 relative rounded-xl border shadow-sm transition-all hover:shadow-lg">
                  <div className="flex items-center p-4">
                    <span className="text-4xl">{product.productType === 'Session' ? '📅' : '📁'}</span>
                    <div className="ml-4 flex-1">
                      <p className="text-lg font-semibold">{product.title}</p>
                      <p className="text-primary text-sm font-medium">${product.price}</p>
                    </div>
                    <Button
                      variant={'ghost'}
                      onClick={() => handleProductEdit(product)}
                      className="flex size-8 items-center justify-center rounded-full bg-black/10 p-2 text-gray-600 transition-colors hover:bg-black/20 dark:bg-white/10 dark:text-gray-300 dark:hover:bg-white/20"
                      aria-label="Edit product"
                    >
                      <Edit2Icon />
                    </Button>
                  </div>
                </div>
              </div>
            ))}
        </div>
      </aside>
      <MobilePreview storeForm={watchedValues} />
    </div>
  );
}
