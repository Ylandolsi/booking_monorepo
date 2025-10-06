import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button';
import { Card } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { User, Link, CheckCircle, LogOut } from 'lucide-react';
import { ROUTE_PATHS } from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import { MobileContainer, StoreHeader } from '@/pages/store/shared/components';
import { patchPostStoreSchema, useCheckSlugAvailability, useCreateStore, type PatchPostStoreRequest, type Store } from '@/api/stores';
import useDebounce from '@/hooks/use-debounce';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { useUploadPicture } from '@/hooks/use-upload-picture';
import { useNavigate } from '@tanstack/react-router';
import { ThemeToggle, UploadImage } from '@/components';
import { useAuth } from '@/api/auth';
import { SocialLinksForm, type StoreFormData } from '@/pages/store';

// TODO : handle when the cropped image is not saved it should be showed on the phone mock
export const SetupStore = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  const createStoreMutation = useCreateStore();

  const form = useForm<StoreFormData>({
    resolver: zodResolver(patchPostStoreSchema),
    defaultValues: {
      title: '',
      slug: '',
      description: '',
      socialLinks: [],
      file: undefined,

      // ui :
      picture: undefined,
    },
  });

  const { croppedImageUrl, setAspectRatio } = useUploadPicture();

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

  useEffect(() => {
    if (croppedImageUrl) {
      form.setValue('picture', { mainLink: croppedImageUrl || '', thumbnailLink: croppedImageUrl || '' });
    } else {
      // restore to default
      form.setValue('picture', { mainLink: '', thumbnailLink: '' });
    }
  }, [croppedImageUrl, form]);

  const picture = form.getValues('picture');

  const onSubmit = async (data: PatchPostStoreRequest) => {
    try {
      await createStoreMutation.mutateAsync(data);
      navigate({ to: ROUTE_PATHS.APP.INDEX });
    } catch (error) {
      console.error('Failed to create store:', error);
    }
  };

  return (
    // <StoreGuard> // TODO uncomment this
    <div className="flex min-h-screen w-full flex-col items-center justify-center px-4 py-10 lg:px-8">
      <div className="mb-4 flex w-full max-w-7xl justify-start px-4 lg:px-0">
        <ThemeToggle />
        {/* Logout Button */}
        <Button variant="ghost" onClick={() => logout()} className={`text-destructive h-auto justify-start gap-3 px-3 py-2.5`} title="Logout">
          <div className="flex gap-2">
            <LogOut size={20} />
            <span className="font-medium">Logout</span>
          </div>
        </Button>
      </div>
      <div className="flex w-full max-w-7xl flex-col items-center justify-center gap-8 pb-5 lg:flex-row lg:items-start">
        <div className="max-w-lg flex-1 p-6 lg:mr-8">
          <div className="mb-8 text-center">
            <h1 className="from-primary to-chart-4 bg-gradient-to-r bg-clip-text text-3xl font-bold text-transparent">Create Your Linki Store</h1>
            <p className="text-muted-foreground mt-2">Set up your personal mobile store in seconds</p>
          </div>

          <Card className="bg-card/80 animate-in fade-in p-6 shadow-xl backdrop-blur-sm duration-500">
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
                <UploadImage description="Profile Picture (Optional)" />
                <SocialLinksForm form={form} />
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
            <StoreHeader store={{ ...watchedValues, picture } as Store} />
          </MobileContainer>
        </div>
      </div>
    </div>
    // </StoreGuard>
  );
};
