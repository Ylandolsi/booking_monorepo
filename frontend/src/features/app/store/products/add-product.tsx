import { useState } from 'react';
import { createProductSchema, ProductType, type CreateProductInput } from '@/api/stores';
import { SelectProductType } from '@/features/app/store/products/select-product-type';
import { TabNavigation } from '@/components/store';
import { ResponsiveBuilderLayout } from '@/features/app/store';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useDialogUploadPicture } from '@/hooks/use-upload-picture';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';

export function AddProductFlow() {
  const [activeTab, setActiveTab] = useState<'general' | 'details' | 'integrations'>('general');
  const [type, setType] = useState<ProductType | undefined>((new URLSearchParams(location.search).get('type') as ProductType) || undefined);

  const { croppedImageUrl, fileInputRef, handleFileSelect } = useDialogUploadPicture({
    onUpload: (file) => form.setValue('thumbnail', file),
  });

  const handleComplete = () => {};
  const onCancel = () => {};

  const form = useForm<CreateProductInput>({
    resolver: zodResolver(createProductSchema),
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
    },
  });

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
          />
        </div>

        {/* Content */}
        <Form {...form}>
          {type && activeTab === 'general' && (
            <div className="space-y-6 px-6">
              <div className="mb-6 text-center">
                <h2 className="text-foreground mb-2 text-xl font-semibold">{type === 'Session' ? 'Session Details' : 'Product Details'}</h2>
                <p className="text-muted-foreground">Fill in the details for your {type === 'Session' ? 'booking service' : 'digital product'}</p>
              </div>

              {/* Title */}
              <FormField
                control={form.control}
                name="title"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground">Title *</FormLabel>
                    <FormControl>
                      <Input
                        placeholder={type === 'Session' ? '30-minute Strategy Call' : 'Ultimate Course Bundle'}
                        className="py-3 text-lg"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              {/* Subtitle */}
              <FormField
                control={form.control}
                name="subtitle"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground">Subtitle</FormLabel>
                    <FormControl>
                      <Input placeholder="A brief description of what you offer" className="py-3" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              {/* Price */}
              <FormField
                control={form.control}
                name="price"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground">Price *</FormLabel>
                    <div className="flex">
                      <span className="bg-muted text-muted-foreground inline-flex items-center rounded-l-lg border border-r-0 px-3 py-2">$</span>
                      <FormControl>
                        <Input
                          type="number"
                          placeholder="99"
                          className="rounded-l-none py-3"
                          min="0"
                          step="0.01"
                          {...field}
                          onChange={(e) => field.onChange(parseFloat(e.target.value) || 0)}
                        />
                      </FormControl>
                    </div>
                    <FormMessage />
                  </FormItem>
                )}
              />

              {/* Thumbnail Image */}
              <div className="space-y-2">
                <FormLabel className="text-foreground">Cover Image</FormLabel>
                <div className="flex items-start space-x-4">
                  <div className="bg-muted border-border flex h-24 w-24 items-center justify-center overflow-hidden rounded-lg border">
                    {croppedImageUrl ? (
                      <img src={croppedImageUrl} alt="Cover preview" className="h-full w-full object-cover" />
                    ) : (
                      <span className="text-muted-foreground text-center text-xs">No image</span>
                    )}
                  </div>
                  <div className="flex-1">
                    <button
                      type="button"
                      onClick={() => fileInputRef.current?.click()}
                      className="bg-secondary text-secondary-foreground hover:bg-secondary/80 inline-flex items-center rounded-lg px-4 py-2 transition-colors"
                    >
                      Choose Image
                    </button>
                    <input ref={fileInputRef} type="file" accept="image/*" onChange={handleFileSelect} className="hidden" />
                    <p className="text-muted-foreground mt-1 text-xs">Recommended: 400×400px or square format</p>
                  </div>
                </div>
              </div>

              {/* Description */}
              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground">Description *</FormLabel>
                    <FormControl>
                      <Textarea placeholder="Describe what customers will get..." rows={4} className="resize-none" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              {/* CTA Text */}
              <FormField
                control={form.control}
                name="clickToPay"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground">Button Text</FormLabel>
                    <FormControl>
                      <Input placeholder={type === 'Session' ? 'Book Now' : 'Buy Now'} className="py-3" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              {/* Navigation */}
              <div className="flex space-x-3 pt-4">
                <Button type="button" onClick={() => setActiveTab('details')} className="flex-1 py-3">
                  Next: Details
                </Button>
              </div>
            </div>
          )}

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

                  {/* Time Zone */}
                  <FormField
                    control={form.control}
                    name="timeZoneId"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="text-foreground">Time Zone</FormLabel>
                        <FormControl>
                          <select
                            className="bg-input border-border text-foreground focus:ring-ring w-full rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
                            {...field}
                          >
                            <option value="UTC">UTC</option>
                            <option value="America/New_York">Eastern Time</option>
                            <option value="America/Chicago">Central Time</option>
                            <option value="America/Denver">Mountain Time</option>
                            <option value="America/Los_Angeles">Pacific Time</option>
                          </select>
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
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

          {type && activeTab === 'integrations' && (
            <div className="space-y-6 px-6">
              <div className="mb-6 text-center">
                <h2 className="text-foreground mb-2 text-xl font-semibold">Integrations</h2>
                <p className="text-muted-foreground">Connect your product with external services</p>
              </div>

              {/* Placeholder for integrations */}
              <div className="bg-muted text-muted-foreground rounded-lg p-6 text-center">
                <p className="mb-4">Integration settings coming soon</p>
                <p className="text-xs">Connect with payment processors, calendars, and more</p>
              </div>

              {/* Navigation */}
              <div className="flex space-x-3 pt-4">
                <Button type="button" variant="outline" onClick={() => setActiveTab('details')} className="flex-1 py-3">
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
        <div className="mt-6 text-center">
          <Button variant="ghost" onClick={onCancel} className="text-sm">
            Cancel
          </Button>
        </div>
      </div>
    </ResponsiveBuilderLayout>
  );
}
