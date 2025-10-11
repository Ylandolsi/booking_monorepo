import { useEffect, useState } from 'react';
import { createProductSchema, useCreateSession, useMyProductSession, useUpdateSession, type CreateProductInput, type Picture } from '@/api/stores';
import { SelectProductType } from '@/pages/store/private/products/select-product-type-page';
import { Card, CardContent, ErrorComponenet, LoadingState, Progress, TabNavigation } from '@/components';
import { motion, AnimatePresence } from 'framer-motion';
import { ResponsiveBuilderLayout } from '@/pages/store';
import { useForm, type Resolver } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { useAppNavigation, useUploadPicture } from '@/hooks';
import { FormSession } from '@/pages/store/private/products/components/forms/form-schedule';
import { FormGeneral } from '@/pages/store/private/products/components/forms/form-general';
import { useSearch } from '@tanstack/react-router';
import { routes } from '@/config';
import { useAuth } from '@/api/auth';
import { logger } from '@/lib';

export type TabsType = 'general' | 'details';

export type ProductFormData = CreateProductInput & { thumbnailPicture?: Picture };

export function AddProductFlow() {
  const [activeTab, setActiveTab] = useState<TabsType>('general');
  const navigate = useAppNavigation();
  const { type, productSlug } = useSearch({ strict: false });
  const createProductMutation = useCreateSession();
  const updateProductMutation = useUpdateSession();
  const { handleCloseDialog } = useUploadPicture();
  const { currentUser, isLoading: isAuthLoading } = useAuth();

  const { data: editProductData, isLoading: isEditLoading } = useMyProductSession(productSlug, { enabled: !!productSlug });

  const form = useForm<ProductFormData>({
    resolver: zodResolver(createProductSchema) as Resolver<CreateProductInput>,
    defaultValues: {
      title: '',
      subtitle: '',
      price: 0,
      description: '', // html content (from quill : should be sanitized on front and backend)
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
      logger.info('submitting', data);
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
        logger.error('Failed to update product:', error);
      } else {
        logger.error('Failed to create product:', error);
      }
    }
  };
  const onCancel = () => {
    navigate.goTo({ to: routes.to.store.index() + '/', replace: true });
  };

  if (isEditLoading || createProductMutation.isPending || updateProductMutation.isPending || isAuthLoading) {
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

  if (type == 'Session') {
    // we should check if the user is integrated with calendar or not
    if (!currentUser?.integratedWithGoogle) {
      return (
        <div className="flex h-full flex-col items-center justify-center p-4">
          <h2 className="text-foreground mb-2 text-xl font-semibold">Calendar Integration Required</h2>
          <p className="text-accent-foreground mb-4 text-center">
            To create a booking service, please integrate your calendar in your account settings.
          </p>
          <Button onClick={() => navigate.goTo({ to: routes.to.integrations() })} className="py-3">
            Go to Account Settings
          </Button>
        </div>
      );
    }
  }
  const currentStep = activeTab === 'general' ? 1 : 2;
  const totalSteps = 2;

  const progress = Math.round((currentStep / totalSteps) * 100);

  const variants = {
    hidden: { opacity: 0, x: 20 },
    enter: { opacity: 1, x: 0 },
    exit: { opacity: 0, x: -20 },
  };
  // Details and specific fields use responsive layout with preview
  return (
    <ResponsiveBuilderLayout previewData={watchedValues}>
      <Card>
        <div className="space-y-3 p-6">
          {/* Header */}
          <div className="px-6 pt-6">
            <div className="mb-4 text-center">
              <h2 className="text-foreground mb-2 text-xl font-semibold">Edit Product</h2>
              <p className="text-accent-foreground">Update your {type === 'Session' ? 'booking service' : 'digital product'}</p>
            </div>
          </div>
          <div className="flex items-center gap-4">
            <Progress value={progress} className="w-full" />
            <p className="text-muted-foreground text-sm whitespace-nowrap">
              {currentStep}/{totalSteps} completed
            </p>
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

        <CardContent>
          <AnimatePresence mode="wait">
            <motion.div key={currentStep} variants={variants} initial="hidden" animate="enter" exit="exit">
              <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="max-h-full max-w-full overflow-x-hidden overflow-y-auto pb-6">
                  <div className="space-y-6 px-6">
                    {type && activeTab === 'general' && (
                      <FormGeneral editProductData={editProductData} form={form} type={type} setActiveTab={setActiveTab} />
                    )}
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
                          <FormSession form={form} />
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
            </motion.div>
          </AnimatePresence>
        </CardContent>
        {/* Content */}
      </Card>
    </ResponsiveBuilderLayout>
  );
}
