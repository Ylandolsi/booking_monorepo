import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Card } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Label } from '@/components/ui/label';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Checkbox } from '@/components/ui/checkbox';
import { Upload, User, Link, CheckCircle, Globe, Plus, Check, Camera, Edit2Icon } from 'lucide-react';
import { ROUTE_PATHS } from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import { patchPostStoreSchema, useCheckSlugAvailability, useCreateStore, useMyStore, type PatchPostStoreRequest } from '@/api/stores';
import useDebounce from '@/hooks/use-debounce';
import { useUploadPicture } from '@/hooks/use-upload-picture';
import { useNavigate, createFileRoute } from '@tanstack/react-router';
import { socialPlatforms } from '@/features/app/store';
import { ErrorComponenet, LoadingState, MobileContainer, ProductCard, StoreHeader } from '@/components';
import { cn } from '@/lib';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';

export const Route = createFileRoute('/app/store/builder')({
  component: RouteComponent,
});

function RouteComponent() {
  const [open1, setOpen1] = useState(false);
  const [open2, setOpen2] = useState(false);
  const [open3, setOpen3] = useState(false);

  const navigate = useNavigate();
  let { data: store, isLoading, isError } = useMyStore();

  if (isLoading) return <LoadingState type="spinner" />;

  if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  const products = store.products || [];
  const createStoreMutation = useCreateStore();
  const [additionalPlatforms, setAdditionalPlatforms] = useState<string[]>([]);
  const [selectedPlatforms, setSelectedPlatforms] = useState<string[]>([]);
  const [isPopoverOpen, setIsPopoverOpen] = useState(false);

  const form = useForm<PatchPostStoreRequest>({
    resolver: zodResolver(patchPostStoreSchema),
    defaultValues: {
      title: '',
      slug: '',
      description: '',
      socialLinks: [],
      file: undefined,
    },
  });

  const { handleFileSelect, fileInputRef, openDialog, setAspectRatio } = useUploadPicture();

  const watchedValues = form.watch();
  const debouncedSlug = useDebounce(watchedValues.slug, 500);

  const { data: slugAvailabilityResponse } = useCheckSlugAvailability(debouncedSlug, debouncedSlug.length >= 3);
  const slugAvailable = slugAvailabilityResponse?.isAvailable;

  useEffect(() => {
    if (slugAvailable === false && debouncedSlug.length >= 3) {
      form.setError('slug', { type: 'manual', message: 'Slug is already taken' });
    } else {
      if (form.formState.errors.slug?.type === 'manual') {
        form.clearErrors('slug');
      }
    }
  }, [slugAvailable, form]);

  useEffect(() => {
    setAspectRatio(1 / 1); // Set aspect ratio to 1:1 for store profile picuture
  }, []);

  const generateSlug = (title: string) => {
    const slug = title
      .toLowerCase()
      .replace(/[^a-z0-9]/g, '-')
      .replace(/-+/g, '-')
      .replace(/^-|-$/g, '');
    form.setValue('slug', slug);
  };

  const onSubmit = async (data: PatchPostStoreRequest) => {
    try {
      await createStoreMutation.mutateAsync(data);
      navigate({ to: ROUTE_PATHS.APP.INDEX });
    } catch (error) {
      console.error('Failed to create store:', error);
    }
  };

  const availablePlatforms = socialPlatforms.filter((p) => !additionalPlatforms.includes(p.key) && !['instagram', 'twitter'].includes(p.key));

  const handleAddPlatforms = () => {
    setAdditionalPlatforms([...additionalPlatforms, ...selectedPlatforms]);
    setSelectedPlatforms([]);
    setIsPopoverOpen(false);
  };

  const handleCheckboxChange = (key: string, checked: boolean) => {
    if (checked) {
      setSelectedPlatforms([...selectedPlatforms, key]);
    } else {
      setSelectedPlatforms(selectedPlatforms.filter((p) => p !== key));
    }
  };

  return (
    <div className="mx-auto flex min-h-screen max-w-7xl flex-col items-center justify-around gap-10 lg:flex-row lg:items-start">
      <style>{`.material-symbols-outlined { font-variation-settings: "FILL" 0, "wght" 400, "GRAD" 0, "opsz" 24; }`}</style>
      <UploadPictureDialog onUpload={(file) => form.setValue('file', file)} />
      <aside className="border-primary/20 dark:border-primary/30 flex w-[460px] flex-col">
        <div className="flex-1">
          <div className="border-primary/20 dark:border-primary/30 border-b p-6">
            <Accordion type="single" collapsible className="w-full">
              <AccordionItem value="store-details">
                <AccordionTrigger className="text-foreground hover:text-primary">
                  <div className="flex items-center gap-2">
                    {/* TODO change this  */}
                    <Globe className="h-4 w-4" />
                    <h2 className="text-xl font-bold">Edit Store Details</h2>
                  </div>
                </AccordionTrigger>
                <AccordionContent className="mt-0 space-y-4">
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
                                    if (!watchedValues.slug) generateSlug(e.target.value);
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

                        {/* <div className="flex-1">
                          <Button
                            onClick={() => openDialog()}
                            className="bg-secondary text-secondary-foreground hover:bg-secondary/80 inline-flex items-center rounded-lg px-4 py-2 transition-colors"
                          >
                            Choose Image
                          </Button>
                        </div> */}
                        <div className="space-y-2">
                          <Label className="text-foreground flex items-center gap-2">
                            <Upload className="h-4 w-4" />
                            Profile Picture (Optional)
                          </Label>
                          <div className="flex items-center gap-4" onClick={openDialog}>
                            <Label
                              htmlFor="profile-picture-input"
                              className="flex h-12 w-full cursor-pointer items-center justify-center rounded-lg border-2 border-dashed border-gray-300 transition-colors hover:bg-gray-50"
                            >
                              <Camera className="mr-2 h-6 w-6 text-gray-400" />
                              <span className="font-medium text-gray-600">Choose a photo</span>
                            </Label>
                          </div>
                          <p className="text-muted-foreground text-xs">PNG, JPG up to 10MB</p>
                        </div>

                        {/* Collapsible Social Media Section */}
                        <Accordion type="single" collapsible className="w-full">
                          <AccordionItem value="social-links">
                            <AccordionTrigger className="text-foreground hover:text-primary">
                              <div className="flex items-center gap-2">
                                <Globe className="h-4 w-4" />
                                Add Social Links (Optional)
                              </div>
                            </AccordionTrigger>
                            <AccordionContent className="space-y-4">
                              {/* Default platforms */}
                              {socialPlatforms.slice(0, 2).map(({ key, label, icon: Icon }) => (
                                <FormField
                                  key={key}
                                  control={form.control}
                                  name="socialLinks"
                                  render={({ field }) => (
                                    <FormItem>
                                      <FormLabel className="text-foreground flex items-center gap-2">
                                        <Icon className="h-4 w-4" />
                                        {label}
                                      </FormLabel>
                                      <FormControl>
                                        <Input
                                          placeholder={`https://${key}.com/your-profile`}
                                          className="border-border text-foreground"
                                          value={field.value?.find((link: any) => link.platform === key)?.url || ''}
                                          onChange={(e) => {
                                            const currentLinks = field.value || [];
                                            const existingIndex = currentLinks.findIndex((link: any) => link.platform === key);
                                            if (existingIndex >= 0) {
                                              currentLinks[existingIndex].url = e.target.value;
                                            } else {
                                              currentLinks.push({ platform: key, url: e.target.value });
                                            }
                                            field.onChange(currentLinks.filter((link: any) => link.url));
                                          }}
                                        />
                                      </FormControl>
                                      <FormMessage />
                                    </FormItem>
                                  )}
                                />
                              ))}

                              {/* Dynamically added platforms */}
                              {additionalPlatforms.map((key) => {
                                const platform = socialPlatforms.find((p) => p.key === key);
                                if (!platform) return null;
                                const { label, icon: Icon } = platform;
                                return (
                                  <FormField
                                    key={key}
                                    control={form.control}
                                    name="socialLinks"
                                    render={({ field }) => (
                                      <FormItem>
                                        <FormLabel className="text-foreground flex items-center gap-2">
                                          <Icon className="h-4 w-4" />
                                          {label}
                                        </FormLabel>
                                        <FormControl>
                                          <Input
                                            placeholder={`https://${key}.com/your-profile`}
                                            className="border-border text-foreground"
                                            value={field.value?.find((link: any) => link.platform === key)?.url || ''}
                                            onChange={(e) => {
                                              const currentLinks = field.value || [];
                                              const existingIndex = currentLinks.findIndex((link: any) => link.platform === key);
                                              if (existingIndex >= 0) {
                                                currentLinks[existingIndex].url = e.target.value;
                                              } else {
                                                currentLinks.push({ platform: key, url: e.target.value });
                                              }
                                              field.onChange(currentLinks.filter((link: any) => link.url));
                                            }}
                                          />
                                        </FormControl>
                                        <FormMessage />
                                      </FormItem>
                                    )}
                                  />
                                );
                              })}
                              {/* Add more button with popover */}
                              {availablePlatforms.length > 0 && (
                                <Popover open={isPopoverOpen} onOpenChange={setIsPopoverOpen}>
                                  <PopoverTrigger asChild>
                                    <div
                                      className="from-primary to-primary/20 text-background border-border hover:bg-primary flex w-full items-center gap-2 rounded-md bg-gradient-to-br p-3 transition-all duration-200"
                                      onClick={() => {
                                        setIsPopoverOpen(true);
                                      }}
                                    >
                                      <Plus className="h-4 w-4" />
                                      Add Another Platform
                                    </div>
                                  </PopoverTrigger>
                                  <PopoverContent className="w-64 p-4" side="top" align="center">
                                    <div className="grid gap-4">
                                      <div className="space-y-2">
                                        <h4 className="leading-none font-medium">Select Platforms</h4>
                                        <p className="text-muted-foreground text-sm">Choose which platforms to add</p>
                                      </div>
                                      <div className="grid gap-2">
                                        {availablePlatforms.map(({ key, label, icon: Icon }) => (
                                          <div key={key} className="flex items-center space-x-2">
                                            <Checkbox
                                              id={key}
                                              checked={selectedPlatforms.includes(key)}
                                              onCheckedChange={(checked) => {
                                                handleCheckboxChange(key, checked as boolean);
                                              }}
                                            />
                                            <label
                                              htmlFor={key}
                                              className="flex cursor-pointer items-center gap-2 text-sm leading-none font-medium peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                                            >
                                              <Icon className="h-4 w-4" />
                                              {label}
                                            </label>
                                          </div>
                                        ))}
                                        <Button
                                          type="button"
                                          size="sm"
                                          onClick={handleAddPlatforms}
                                          disabled={selectedPlatforms.length === 0}
                                          className="mt-2 w-full"
                                        >
                                          <Check className="mr-2 h-4 w-4" />
                                          Add Selected ({selectedPlatforms.length})
                                        </Button>
                                      </div>
                                    </div>
                                  </PopoverContent>
                                </Popover>
                              )}
                            </AccordionContent>
                          </AccordionItem>
                        </Accordion>

                        <Button
                          type="submit"
                          size="lg"
                          className="hover:bg-primary hover:accent text-primary-foreground w-full transition-all duration-300"
                          disabled={createStoreMutation.isPending}
                        >
                          {createStoreMutation.isPending ? (
                            'Creating Store...'
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
          <div className="flex h-16 items-center justify-between px-6">
            <h2 className="text-xl font-bold">My Products</h2>
            <button
              className="bg-primary/10 hover:bg-primary/20 dark:bg-primary/20 dark:hover:bg-primary/30 text-primary flex items-center justify-center rounded-lg p-2 transition-colors"
              aria-label="Add new product"
            >
              <Plus className="h-5 w-5" />
            </button>
          </div>
          <div className="space-y-4 p-6 pt-0">
            <div className="group border-primary/20 dark:border-primary/30 bg-card-light dark:bg-card-dark hover:border-primary/40 relative rounded-xl border shadow-sm transition-all hover:shadow-lg">
              <div className="flex items-center p-4">
                <div
                  className="ml-4 h-16 w-16 flex-shrink-0 rounded-lg bg-cover bg-center"
                  style={{
                    backgroundImage:
                      "url('https://lh3.googleusercontent.com/aida-public/AB6AXuCb11m5DhgoWYEkTUpMJEVCL54e_altP1CmZlnIJXk-6LX_RLAlggJKLprrULvn_v9zvOtiAMACHFTYwZZUaoENiAkm3S-toDSBEU0Mc6q8RKoOUAYui6kWDCeU_BfQ1CPtqTNorHgMeWS1ABeanJ8JMe1tloXrygZT9hbBR8fTZjuhUC9l20RAUrQ-4a9gPPTC6m2cEexmZ6CWgHxsbvt4Z7pTRosOhKvxVRa-hOF3OaF8Li-cPV4pTkGBq_PvcI-4qgB5htJDCdwZ')",
                  }}
                />
                <div className="ml-4 flex-1">
                  <p className="text-lg font-semibold">Product 1</p>
                  <p className="text-primary text-sm font-medium">$10.00</p>
                </div>
                <button
                  onClick={() => setOpen1(!open1)}
                  className="flex size-8 items-center justify-center rounded-full bg-black/10 p-2 text-gray-600 transition-colors hover:bg-black/20 dark:bg-white/10 dark:text-gray-300 dark:hover:bg-white/20"
                  aria-label="Edit product"
                >
                  <Edit2Icon />
                </button>
              </div>
              {open1 && (
                <div className="bg-card-light/95 dark:bg-card-dark/95 animate-in fade-in absolute inset-0 z-20 space-y-4 rounded-xl p-4 backdrop-blur-sm duration-200">
                  <div>
                    <label className="mb-1 block text-sm font-medium text-[#101922]/70 dark:text-[#f6f7f8]/70" htmlFor="productName1">
                      Product Name
                    </label>
                    <input
                      className="border-primary/30 bg-background-light dark:bg-background-dark focus:border-primary focus:ring-primary/20 mt-1 block w-full rounded-md px-3 py-2 shadow-sm transition-all focus:ring-2 sm:text-sm"
                      id="productName1"
                      type="text"
                      defaultValue="Product 1"
                    />
                  </div>
                  <div>
                    <label className="mb-1 block text-sm font-medium text-[#101922]/70 dark:text-[#f6f7f8]/70" htmlFor="productPrice1">
                      Price
                    </label>
                    <input
                      className="border-primary/30 bg-background-light dark:bg-background-dark focus:border-primary focus:ring-primary/20 mt-1 block w-full rounded-md px-3 py-2 shadow-sm transition-all focus:ring-2 sm:text-sm"
                      id="productPrice1"
                      type="text"
                      defaultValue="$10.00"
                    />
                  </div>
                  <div className="flex items-center justify-between pt-4">
                    <button className="text-sm font-bold text-red-500 transition-colors hover:underline">Delete</button>
                    <button
                      onClick={() => setOpen1(false)}
                      className="bg-primary hover:bg-primary/90 rounded-md px-4 py-2 text-sm font-semibold text-white transition-colors"
                    >
                      Done
                    </button>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </aside>
      <main className="flex flex-1 items-center justify-center">
        <MobileContainer>
          <StoreHeader store={store} />
          {/* <div className="space-y-4 p-4">
            <div className="bg-background-light dark:bg-background-dark border-primary/20 dark:border-primary/30 flex items-center gap-4 rounded-lg border p-2">
              <div className="flex-1">
                <p className="font-bold">Product 1</p>
                <p className="text-sm text-[#101922]/70 dark:text-[#f6f7f8]/70">$10.00</p>
              </div>
              <div
                className="h-24 w-24 rounded-lg bg-cover bg-center"
                style={{
                  backgroundImage:
                    "url('https://lh3.googleusercontent.com/aida-public/AB6AXuCb11m5DhgoWYEkTUpMJEVCL54e_altP1CmZlnIJXk-6LX_RLAlggJKLprrULvn_v9zvOtiAMACHFTYwZZUaoENiAkm3S-toDSBEU0Mc6q8RKoOUAYui6kWDCeU_BfQ1CPtqTNorHgMeWS1ABeanJ8JMe1tloXrygZT9hbBR8fTZjuhUC9l20RAUrQ-4a9gPPTC6m2cEexmZ6CWgHxsbvt4Z7pTRosOhKvxVRa-hOF3OaF8Li-cPV4pTkGBq_PvcI-4qgB5htJDCdwZ')",
                }}
              />
            </div>
            <div className="bg-background-light dark:bg-background-dark border-primary/20 dark:border-primary/30 flex items-center gap-4 rounded-lg border p-2">
              <div className="flex-1">
                <p className="font-bold">Product 2</p>
                <p className="text-sm text-[#101922]/70 dark:text-[#f6f7f8]/70">$20.00</p>
              </div>
              <div
                className="h-24 w-24 rounded-lg bg-cover bg-center"
                style={{
                  backgroundImage:
                    "url('https://lh3.googleusercontent.com/aida-public/AB6AXuANMJj2KiYLdHV-Egcqe05bMkX8SBgQ0bIZ1IwpghFetfGZLdzRSObBpbALo_SrjyO8UVQg6QngCKy9zGhkJBmMwH4Dl3akYGK-aVx9w_48J47E-_6Ph0kNaOoKykfhUQhYUPl6WKQAStuAc39j5RufkaUufjgMwEEEEqsdSpux7qdlOSDFVyqYAC1KwCCYT1ggsxFEDT1JFdpYUzvjGMpmk6hXCWhzIhdJMPoN5hW-hJavjnsTwP7GLt5NpEEjyu95-dDmRXQUb_Jd')",
                }}
              />
            </div>
          </div> */}
          <div className={'w-full space-y-4'}>
            {products.map((product, index) => (
              <div
                key={product.productSlug}
                className={cn('relative transition-all duration-200', 'scale-95 opacity-50', 'translate-y-1 transform', 'cursor-move')}
              >
                <div className={'group'}>
                  <ProductCard product={product} />
                </div>
              </div>
            ))}
          </div>
        </MobileContainer>
      </main>
    </div>
  );
}
