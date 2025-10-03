import { useEffect, useState } from 'react';
import { createProductSchema, useCreateSession, useMyProductSession, useUpdateSession, type CreateProductInput, type Picture } from '@/api/stores';
import { SelectProductType } from '@/features/app/store/products/select-product-type';
import { ErrorComponenet, LoadingState, Select, SelectContent, SelectItem, SelectTrigger, SelectValue, TabNavigation } from '@/components';
import { ResponsiveBuilderLayout } from '@/features/app/store';
import { useForm, type Resolver } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';
import { useAppNavigation, useUploadPicture } from '@/hooks';
import { FormScheduleComponent } from '@/features/app/store/products/components/form-schedule';
import { FormGeneral } from '@/features/app/store/products/components/form-general';
import { useSearch } from '@tanstack/react-router';
import { routes } from '@/config';

export type TabsType = 'general' | 'details';

export type ProductFormData = CreateProductInput & { thumbnailPicture?: Picture };

export function AddProductFlow() {
  const [activeTab, setActiveTab] = useState<TabsType>('general');
  const navigate = useAppNavigation();
  const { type, productSlug } = useSearch({ strict: false });
  const createProductMutation = useCreateSession();
  const updateProductMutation = useUpdateSession();
  const { handleCloseDialog } = useUploadPicture();

  const { data: editProductData, isLoading: isEditLoading } = useMyProductSession(productSlug, { enabled: !!productSlug });

  const form = useForm<ProductFormData>({
    resolver: zodResolver(createProductSchema) as Resolver<CreateProductInput>,
    defaultValues: {
      title: '',
      subtitle: '',
      price: 0,
      description: '',
      clickToPay: 'Buy Now',
      productType: type === 'Session' ? 'Session' : 'DigitalDownload',
      thumbnailImage: undefined,
      durationMinutes: type == 'Session' ? 30 : undefined,
      bufferTimeMinutes: type == 'Session' ? 15 : undefined,
      meetingInstructions: '',
      files: type == 'DigitalDownload' ? [] : undefined,
      deliveryUrl: '',
      previewImage: undefined,
      dayAvailabilities: [],

      // UI
      thumbnailPicture: undefined,
    },
  });

  useEffect(() => {
    if (type != form.getValues('productType')) {
      form.setValue('productType', type === 'Session' ? 'Session' : 'DigitalDownload');
    }
  }, [type]);

  // Reset form with new values

  useEffect(() => {
    if (productSlug && editProductData) {
      // init form with product to update
      form.reset(editProductData);
    }
  }, [productSlug, editProductData]);

  const onSubmit = async (data: CreateProductInput) => {
    try {
      console.log('submitting', data);
      if (productSlug) {
        // update
        await updateProductMutation.mutateAsync({ productSlug: productSlug, data });
      } else {
        // create
        await createProductMutation.mutateAsync({ data });
      }
      // after update : delete cropped image
      handleCloseDialog();
      navigate.goTo({ to: routes.to.store.index() + '/', replace: true });
    } catch (error) {
      if (productSlug) {
        // specific error handling for update
        console.error('Failed to update product:', error);
      } else {
        console.error('Failed to create product:', error);
      }
    }
  };
  const onCancel = () => {
    navigate.goTo({ to: routes.to.store.index() + '/', replace: true });
  };

  if (isEditLoading) {
    return <LoadingState type="spinner" />;
  }
  if (productSlug && !editProductData && !isEditLoading) {
    return <ErrorComponenet message="Failed to load product for editing. It may not exist." />;
  }

  {
    /* Thumbnail Display Mode */
    // <ThumbnailModeSelector value={data.thumbnailMode || 'expanded'} onChange={(mode) => handleFieldChange('thumbnailMode', mode)} />;
  }
  const watchedValues = form.watch();
  // Type selection doesn't need preview
  if (type == null || type == undefined) {
    return (
      <div className="flex h-full items-center justify-center p-4">
        <SelectProductType />
      </div>
    );
  }

  // Details and specific fields use responsive layout with preview
  return (
    <ResponsiveBuilderLayout previewData={watchedValues}>
      <div className="p-6">
        {/* Header */}
        <div className="px-6 pt-6 pb-4">
          <div className="mb-4 text-center">
            <h2 className="text-foreground mb-2 text-xl font-semibold">Edit Product</h2>
            <p className="text-accent-foreground">Update your {type === 'Session' ? 'booking service' : 'digital product'}</p>
          </div>
        </div>

        {/* Tab Navigation */}
        <TabNavigation
          tabs={[
            { id: 'general', label: 'General Info', description: 'Title, image, pricing', icon: '📝' },
            {
              id: 'details',
              label: 'Details',
              description: type === 'Session' ? 'Scheduling & meetings' : 'Files & downloads',
              icon: type === 'Session' ? '📅' : '📁',
              // TODO : add type link as well
            },
          ]}
          activeTab={activeTab}
          onTabChange={(tabId) => setActiveTab(tabId as 'general' | 'details')}
          className=""
        />
      </div>

      {/* Content */}
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="max-h-full overflow-y-auto pb-6">
          <div className="space-y-6 px-6">
            {type && activeTab === 'general' && <FormGeneral editProductData={editProductData} form={form} type={type} setActiveTab={setActiveTab} />}
            {type && activeTab === 'details' && (
              <>
                <div className="mb-6 text-center">
                  <h2 className="text-foreground mb-2 text-xl font-semibold">
                    {type === 'Session' ? 'Session Settings' : 'Digital Product Settings'}
                  </h2>
                  <p className="text-accent-foreground">
                    Configure specific settings for your {type === 'Session' ? 'booking service' : 'digital product'}
                  </p>
                </div>

                {type === 'Session' ? (
                  <div className="space-y-4">
                    {/* Duration */}
                    <FormField
                      control={form.control}
                      name="durationMinutes"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-foreground">Duration (minutes) *</FormLabel>
                          <Select onValueChange={(value) => field.onChange(Number(value))} value={field.value?.toString()}>
                            <FormControl>
                              <SelectTrigger className="w-full">
                                <SelectValue placeholder="Select duration" />
                              </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                              <SelectItem value="30">30</SelectItem>
                              {/* only 30 minutes available for now */}
                            </SelectContent>
                          </Select>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    {/* Buffer Time */}
                    <FormField
                      control={form.control}
                      name="bufferTimeMinutes"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-foreground">Buffer Time (minutes)</FormLabel>
                          <FormControl>
                            <Select onValueChange={(value) => field.onChange(Number(value))} value={field.value?.toString()}>
                              <FormControl>
                                <SelectTrigger className="w-full">
                                  <SelectValue placeholder="Select duration" />
                                </SelectTrigger>
                              </FormControl>
                              <SelectContent>
                                <SelectItem value="15">15</SelectItem>
                                <SelectItem value="30">30</SelectItem>
                                <SelectItem value="45">45</SelectItem>
                                {/* only 30 minutes available for now */}
                              </SelectContent>
                            </Select>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    {/* Meeting Instructions */}
                    <FormField
                      control={form.control}
                      name="meetingInstructions"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-foreground">Meeting Instructions</FormLabel>
                          <FormControl>
                            <Textarea placeholder="What should customers know before the meeting?" rows={3} className="resize-none" {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    {/* Schedule Component */}
                    <FormScheduleComponent form={form} />
                  </div>
                ) : (
                  <div className="space-y-4">
                    {/* Delivery URL */}
                    <FormField
                      control={form.control}
                      name="deliveryUrl"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-foreground">Delivery URL</FormLabel>
                          <FormControl>
                            <Input type="url" placeholder="https://example.com/download" className="py-3" {...field} />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    {/* Placeholder for file uploads */}
                    <div className="bg-muted text-accent-foreground rounded-lg p-6 text-center">
                      <p className="mb-4">File upload functionality coming soon</p>
                      <p className="text-xs">You'll be able to upload files directly here</p>
                    </div>
                  </div>
                )}

                {/* Navigation */}
                <div className="flex space-x-3 pt-4">
                  <Button type="button" variant="outline" onClick={() => setActiveTab('general')} className="flex-1 py-3">
                    Back
                  </Button>
                  <Button type="submit" className="flex-1 py-3" disabled={createProductMutation.isPending || updateProductMutation.isPending}>
                    {productSlug
                      ? createProductMutation.isPending
                        ? 'Updating...'
                        : 'Update Product'
                      : createProductMutation.isPending
                        ? 'Creating...'
                        : 'Create Product'}
                  </Button>
                </div>
              </>
            )}
            <Button variant="ghost" onClick={onCancel} className="w-full text-sm">
              Cancel
            </Button>
          </div>
        </form>
      </Form>
    </ResponsiveBuilderLayout>
  );
}
