import { useState } from 'react';
import { createProductSchema, ProductType, type CreateProductInput, type Picture } from '@/api/stores';
import { SelectProductType } from '@/features/app/store/products/select-product-type';
import { TabNavigation } from '@/components/store';
import { ResponsiveBuilderLayout } from '@/features/app/store';
import { useForm, type Resolver } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';
import { useAppNavigation } from '@/hooks';
import { FormScheduleComponent } from '@/features/app/store/products/components/form-schedule';
import { FormGeneral } from '@/features/app/store/products/components/form-general';

export type TabsType = 'general' | 'details';

export type ProductFormData = CreateProductInput & { ui?: { picture?: Picture } };

export function AddProductFlow() {
  const [activeTab, setActiveTab] = useState<TabsType>('general');
  // TODO : make this follow react tanstack router best practices
  const [type, setType] = useState<ProductType | undefined>((new URLSearchParams(location.search).get('type') as ProductType) || undefined);
  const navigate = useAppNavigation();

  const handleComplete = async (data: CreateProductInput) => {
    try {
      console.log('Form data:', data);
      // TODO: Implement actual API call to create product
      // await createProductMutation.mutateAsync(data);

      // Navigate to products list or success page
      navigate.goTo({ to: '/app/store', replace: true });
    } catch (error) {
      console.error('Failed to create product:', error);
      // Handle error - show toast, etc.
    }
  };
  const onCancel = () => {
    setType(undefined);
    navigate.goTo({ to: '/app/store/product/add', replace: true });
  };

  const form = useForm<ProductFormData>({
    resolver: zodResolver(createProductSchema) as Resolver<CreateProductInput>,
    defaultValues: {
      title: '',
      subtitle: '',
      price: 0,
      description: '',
      clickToPay: 'Buy Now',
      productType: type === 'Session' ? 'Session' : 'DigitalDownload',
      thumbnail: undefined,
      duration: type === 'Session' ? 30 : undefined,
      bufferTime: type === 'Session' ? 0 : undefined,
      meetingInstructions: '',
      timeZoneId: type === 'Session' ? 'UTC' : undefined,
      files: type === 'DigitalDownload' ? [] : undefined,
      deliveryUrl: '',
      previewImage: undefined,
      dailySchedule: [],
      ui: { picture: undefined },
    },
  });
  {
    /* Thumbnail Display Mode */
    // <ThumbnailModeSelector value={data.thumbnailMode || 'expanded'} onChange={(mode) => handleFieldChange('thumbnailMode', mode)} />;
  }
  const watchedValues = form.watch();

  // Type selection doesn't need preview
  if (type == null || type == undefined) {
    return (
      <div className="flex h-full items-center justify-center p-4">
        <SelectProductType setType={setType} />
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
            <p className="text-muted-foreground">Update your {type === 'Session' ? 'booking service' : 'digital product'}</p>
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
          {type && activeTab === 'general' && <FormGeneral form={form} type={type} setActiveTab={setActiveTab} />}
          {type && activeTab === 'details' && (
            <div className="space-y-6 px-6">
              <div className="mb-6 text-center">
                <h2 className="text-foreground mb-2 text-xl font-semibold">{type === 'Session' ? 'Session Settings' : 'Digital Product Settings'}</h2>
                <p className="text-muted-foreground">
                  Configure specific settings for your {type === 'Session' ? 'booking service' : 'digital product'}
                </p>
              </div>

              {type === 'Session' ? (
                <div className="space-y-4">
                  {/* Duration */}
                  <FormField
                    control={form.control}
                    name="duration"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="text-foreground">Duration (minutes) *</FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            placeholder="30"
                            className="py-3"
                            min="15"
                            step="15"
                            {...field}
                            onChange={(e) => field.onChange(parseInt(e.target.value) || 30)}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />

                  {/* Buffer Time */}
                  <FormField
                    control={form.control}
                    name="bufferTime"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="text-foreground">Buffer Time (minutes)</FormLabel>
                        <FormControl>
                          <Input
                            type="number"
                            placeholder="0"
                            className="py-3"
                            min="0"
                            {...field}
                            onChange={(e) => field.onChange(parseInt(e.target.value) || 0)}
                          />
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
                  <div className="bg-muted text-muted-foreground rounded-lg p-6 text-center">
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
                <Button type="button" onClick={form.handleSubmit(handleComplete)} className="flex-1 py-3">
                  Create Product
                </Button>
              </div>
            </div>
          )}
        </Form>

        {/* Cancel Button */}
        <div className="mt-6 w-full text-center">
          <Button variant="ghost" onClick={onCancel} className="w-full text-sm">
            Cancel
          </Button>
        </div>
      </div>
    </ResponsiveBuilderLayout>
  );
}
