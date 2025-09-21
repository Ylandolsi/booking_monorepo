import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { useState, useRef, useEffect } from 'react';
import { useForm, type UseFormReturn } from 'react-hook-form';
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
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog';
import { Upload, User, Link, CheckCircle, Instagram, Twitter, Facebook, Youtube, Globe, Plus, Check, Camera, X } from 'lucide-react';
import { ROUTE_PATHS } from '@/config/routes';
import ReactCrop, { centerCrop, makeAspectCrop, type PixelCrop } from 'react-image-crop';
import 'react-image-crop/dist/ReactCrop.css';
import { MobileContainer, StoreHeader } from '@/components/store';
import { createStoreSchema, useCheckSlugAvailability, useCreateStore, type createStoreInput, type Store } from '@/api/stores';
import useDebounce from '@/hooks/use-debounce';

export const Route = createFileRoute('/app/setup')({
  component: RouteComponent,
});

export interface uploadPictureState {
  setIsUploadDialogOpen: React.Dispatch<React.SetStateAction<boolean>>;
  setCroppedImageUrl: React.Dispatch<React.SetStateAction<string | null>>;

  isUploadDialogOpen: boolean;
  selectedImage: string | null;

  croppedImageUrl: string | null;
  step: 'select' | 'crop';
  imgRef: React.MutableRefObject<HTMLImageElement | null>;
  fileInputRef: React.MutableRefObject<HTMLInputElement | null>;

  setCrop: React.Dispatch<React.SetStateAction<PixelCrop | undefined>>;
  crop: PixelCrop | undefined;

  handleCloseDialog: () => void;
  handleImageLoad: (e: React.SyntheticEvent<HTMLImageElement>) => void;
  handleUpload: () => void;
  handleBackToSelect: () => void;
  handleCropComplete: (cropData: PixelCrop) => void;
  handleFileSelect: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

const useUploadPicture = ({ form }: { form: UseFormReturn<createStoreInput> }): uploadPictureState => {
  const [isUploadDialogOpen, setIsUploadDialogOpen] = useState(false);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);
  const [crop, setCrop] = useState<any>();
  const [croppedImageUrl, setCroppedImageUrl] = useState<string | null>(null);
  const [step, setStep] = useState<'select' | 'crop'>('select');
  const imgRef = useRef<HTMLImageElement | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  // Image upload functions
  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        setSelectedImage(reader.result as string);
        setCroppedImageUrl(reader.result as string);
        setStep('crop');
        setIsUploadDialogOpen(true);
      };
      reader.readAsDataURL(file);
    }
  };

  const centerAspectCrop = (mediaWidth: number, mediaHeight: number, aspect: number) => {
    return centerCrop(
      makeAspectCrop(
        {
          unit: '%',
          width: 80,
        },
        aspect,
        mediaWidth,
        mediaHeight,
      ),
      mediaWidth,
      mediaHeight,
    );
  };

  const getCroppedImg = (image: HTMLImageElement, cropData: PixelCrop): Promise<string> => {
    const canvas = document.createElement('canvas');
    const scaleX = image.naturalWidth / image.width;
    const scaleY = image.naturalHeight / image.height;

    canvas.width = cropData.width;
    canvas.height = cropData.height;
    const ctx = canvas.getContext('2d');

    if (!ctx) {
      throw new Error('No 2d context');
    }

    ctx.drawImage(
      image,
      cropData.x * scaleX,
      cropData.y * scaleY,
      cropData.width * scaleX,
      cropData.height * scaleY,
      0,
      0,
      cropData.width,
      cropData.height,
    );

    return new Promise<string>((resolve) => {
      canvas.toBlob(
        (blob) => {
          if (!blob) {
            console.error('Canvas is empty');
            return;
          }
          resolve(URL.createObjectURL(blob));
        },
        'image/jpeg',
        0.9,
      );
    });
  };

  const handleImageLoad = (e: React.SyntheticEvent<HTMLImageElement>) => {
    const { width, height } = e.currentTarget;
    imgRef.current = e.currentTarget;
    const newCrop = centerAspectCrop(width, height, 1);
    setCrop(newCrop);
  };

  const handleCropComplete = async (cropData: PixelCrop) => {
    if (imgRef.current && cropData.width && cropData.height) {
      try {
        const croppedImage = await getCroppedImg(imgRef.current, cropData);
        setCroppedImageUrl(croppedImage);
      } catch (error) {
        console.error('Error cropping image:', error);
      }
    }
  };

  const handleUpload = async () => {
    if (!croppedImageUrl) {
      console.error('No cropped image to upload');
      return;
    }

    // Convert URL to File
    const response = await fetch(croppedImageUrl);
    const blob = await response.blob();
    const file = new File([blob], 'profile-picture.jpg', { type: blob.type });

    form.setValue('picture', file);
    handleCloseDialog();
  };

  const handleCloseDialog = () => {
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    // setSelectedImage(null);
    // setCroppedImageUrl(null);
    setCrop(undefined);
    setStep('select');
    setIsUploadDialogOpen(false);

    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleBackToSelect = () => {
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    setSelectedImage(null);
    setCroppedImageUrl(null);
    setCrop(undefined);
    setStep('select');
  };

  return {
    setIsUploadDialogOpen,
    setCroppedImageUrl,
    isUploadDialogOpen,
    selectedImage,
    croppedImageUrl,
    step,
    imgRef,
    fileInputRef,
    setCrop,
    crop,

    handleCloseDialog,
    handleImageLoad,
    handleUpload,
    handleBackToSelect,
    handleCropComplete,
    handleFileSelect,
  };
};

// TODO : handle when the cropped image is not saved it should be showed on the phone mock
function RouteComponent() {
  const navigate = useNavigate();

  const createStoreMutation = useCreateStore();
  const [additionalPlatforms, setAdditionalPlatforms] = useState<string[]>([]);
  const [selectedPlatforms, setSelectedPlatforms] = useState<string[]>([]);
  const [isPopoverOpen, setIsPopoverOpen] = useState(false);

  const form = useForm<createStoreInput>({
    resolver: zodResolver(createStoreSchema),
    defaultValues: {
      title: '',
      slug: '',
      description: '',
      socialLinks: [],
      picture: undefined,
    },
  });

  const {
    setIsUploadDialogOpen,
    isUploadDialogOpen,
    selectedImage,
    croppedImageUrl,
    setCrop,
    crop,
    step,
    imgRef,
    handleImageLoad,
    fileInputRef,
    handleUpload,
    handleBackToSelect,
    handleCropComplete,
    handleFileSelect,
    handleCloseDialog,
  } = useUploadPicture({ form });

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

  const generateSlug = (title: string) => {
    const slug = title
      .toLowerCase()
      .replace(/[^a-z0-9]/g, '-')
      .replace(/-+/g, '-')
      .replace(/^-|-$/g, '');
    form.setValue('slug', slug);
  };

  const onSubmit = async (data: createStoreInput) => {
    try {
      await createStoreMutation.mutateAsync(data);
      navigate({ to: ROUTE_PATHS.APP.STORE });
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
    <div className="flex w-full flex-col items-center justify-around gap-8 pb-5 lg:flex-row lg:items-start">
      {/* Setup Form */}
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

              <div className="space-y-2">
                <Label className="text-foreground flex items-center gap-2">
                  <Upload className="h-4 w-4" />
                  Profile Picture (Optional)
                </Label>
                <div className="flex items-center gap-4">
                  <input ref={fileInputRef} type="file" accept="image/*" onChange={handleFileSelect} className="hidden" id="profile-picture-input" />
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
      <Dialog open={isUploadDialogOpen} onOpenChange={setIsUploadDialogOpen}>
        <DialogContent className="sm:max-w-md">
          <DialogHeader>
            <DialogTitle>Update Profile Picture</DialogTitle>
          </DialogHeader>

          <div className="space-y-4">
            {step === 'select' && (
              <div className="space-y-4">
                <Label
                  htmlFor="profile-picture-input-dialog"
                  className="flex h-48 w-full cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-gray-300 transition-colors hover:bg-gray-50"
                >
                  <Camera className="mb-4 h-12 w-12 text-gray-400" />
                  <p className="font-medium text-gray-600">Choose a photo</p>
                  <p className="text-sm text-gray-400">PNG, JPG up to 10MB</p>
                </Label>
                <input
                  ref={fileInputRef}
                  type="file"
                  accept="image/*"
                  onChange={handleFileSelect}
                  className="hidden"
                  id="profile-picture-input-dialog"
                />
              </div>
            )}

            {step === 'crop' && selectedImage && (
              <div className="space-y-4">
                <div className="flex justify-center">
                  <div className="w-full max-w-sm">
                    <ReactCrop
                      crop={crop}
                      aspect={1}
                      onChange={(c) => setCrop(c)}
                      onComplete={handleCropComplete}
                      className="overflow-hidden rounded-lg"
                    >
                      <img
                        ref={imgRef}
                        src={selectedImage}
                        onLoad={handleImageLoad}
                        alt="Profile picture"
                        className="max-h-64 w-full object-contain"
                      />
                    </ReactCrop>
                  </div>
                </div>

                {croppedImageUrl && (
                  <div className="text-center">
                    <p className="mb-2 text-sm text-gray-600">Preview:</p>
                    <div className="flex justify-center">
                      <img src={croppedImageUrl} alt="Cropped preview" className="h-20 w-20 rounded-full border-2 border-gray-200 object-cover" />
                    </div>
                  </div>
                )}
              </div>
            )}
          </div>

          <DialogFooter className="flex justify-between">
            {step === 'select' ? (
              <Button onClick={handleCloseDialog} variant="outline" className="w-full">
                Cancel
              </Button>
            ) : (
              <div className="flex w-full gap-2">
                <Button onClick={handleBackToSelect} variant="outline">
                  <X className="mr-2 h-4 w-4" />
                  Back
                </Button>

                <Button onClick={handleUpload} disabled={!croppedImageUrl} className="flex-1">
                  <Check className="mr-2 h-4 w-4" />
                  Save
                </Button>
              </div>
            )}
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Live Preview - keeping the same */}
      <div className="sticky top-4">
        <MobileContainer>
          <StoreHeader store={{ ...watchedValues, picture: { mainLink: croppedImageUrl } } as Store} />
        </MobileContainer>
      </div>
    </div>
  );
}
export const socialPlatforms = [
  { key: 'instagram', label: 'Instagram', icon: Instagram },
  { key: 'twitter', label: 'Twitter', icon: Twitter },
  { key: 'facebook', label: 'Facebook', icon: Facebook },
  { key: 'youtube', label: 'YouTube', icon: Youtube },
  { key: 'website', label: 'Website', icon: Globe },
];
