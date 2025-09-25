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
import { Upload, User, Link, CheckCircle, Instagram, Twitter, Facebook, Youtube, Globe, Plus, Check, Camera } from 'lucide-react';
import { ROUTE_PATHS } from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import { MobileContainer, StoreHeader } from '@/components/store';
import { patchPostStoreSchema, useCheckSlugAvailability, useCreateStore, type PatchPostStoreRequest, type Store } from '@/api/stores';
import useDebounce from '@/hooks/use-debounce';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { useUploadPicture } from '@/hooks/use-upload-picture';
import { useNavigate } from '@tanstack/react-router';
import { StoreGuard } from '@/components';

// TODO : handle when the cropped image is not saved it should be showed on the phone mock
export const SetupStore = () => {
  const navigate = useNavigate();

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

  const { openDialog, croppedImageUrl, setAspectRatio } = useUploadPicture();

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
    // <StoreGuard> // TODO uncomment this
    <div className="flex min-h-screen w-full items-center justify-center px-4 py-10 lg:px-8">
      <div className="flex w-full max-w-7xl flex-col items-center justify-center gap-8 pb-5 lg:flex-row lg:items-start">
        <div className="max-w-lg flex-1">
          <div className="mb-8 text-center">
            <h1 className="from-primary to-chart-4 bg-gradient-to-r bg-clip-text text-3xl font-bold text-transparent">Create Your Linki Store</h1>
            <p className="text-muted-foreground mt-2">Set up your personal mobile store in seconds</p>
          </div>

          <Card className="bg-card/80 animate-in fade-in border-0 p-6 shadow-xl backdrop-blur-sm duration-500">
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
                  name="slug"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-foreground flex items-center gap-2">
                        <Link className="h-4 w-4" />
                        Unique Slug *
                      </FormLabel>
                      <div className="flex">
                        <div className="bg-muted text-muted-foreground flex items-center justify-center rounded-l-md border border-r-0 px-2 text-center text-sm">
                          linki.store/
                        </div>
                        <FormControl>
                          <Input placeholder="your-store" className="border-border text-foreground rounded-l-none py-3 text-lg" {...field} />
                        </FormControl>
                      </div>
                      <p className="text-muted-foreground text-xs">This will be your store's URL: linki.store/{watchedValues.slug || 'your-store'}</p>
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
                        <Textarea placeholder="Tell your customers what you offer..." rows={3} className="border-border text-foreground" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                {/* <div className="space-y-2">
                  <Label className="text-foreground flex items-center gap-2">
                    <Upload className="h-4 w-4" />
                    Profile Picture (Optional)
                  </Label>
                  <Button onClick={() => openDialog()} className="flex items-center gap-4">
                    <Label
                      htmlFor="profile-picture-input"
                      className="flex h-12 w-full cursor-pointer items-center justify-center rounded-lg border-2 border-dashed border-gray-300 transition-colors hover:bg-gray-50"
                    >
                      <Camera className="mr-2 h-6 w-6 text-gray-400" />
                      <span className="font-medium text-gray-600">Choose a photo</span>
                    </Label>
                  </Button>
                  <p className="text-muted-foreground text-xs">PNG, JPG up to 10MB</p>
                </div> */}

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
                  className="from-primary to-scondary hover:from-primary hover:to-accent text-primary-foreground w-full bg-gradient-to-r transition-all duration-300"
                  disabled={createStoreMutation.isPending}
                >
                  {createStoreMutation.isPending ? (
                    'Creating Store...'
                  ) : (
                    <>
                      <CheckCircle className="mr-2 h-4 w-4" />
                      Create My Store
                    </>
                  )}
                </Button>
              </form>
            </Form>
          </Card>
        </div>

        {/* Upload Picture Dialog */}
        {/* <DialogComponent /> */}
        <UploadPictureDialog onUpload={(file) => form.setValue('file', file)} />

        {/* Live Preview - keeping the same */}
        <div className="sticky top-4">
          <MobileContainer>
            <StoreHeader store={{ ...watchedValues, picture: { mainLink: croppedImageUrl } } as Store} />
          </MobileContainer>
        </div>
      </div>
    </div>
    // </StoreGuard>
  );
};
export const socialPlatforms = [
  { key: 'instagram', label: 'Instagram', icon: Instagram },
  { key: 'twitter', label: 'Twitter', icon: Twitter },
  { key: 'facebook', label: 'Facebook', icon: Facebook },
  { key: 'youtube', label: 'YouTube', icon: Youtube },
  { key: 'website', label: 'Website', icon: Globe },
];
