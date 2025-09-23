import type { CreateProductInput, ProductType } from '@/api/stores/produtcs/sessions';
import type { UseFormReturn } from 'react-hook-form';
import { FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Input, Textarea } from '@/components';
import type { ProductFormData, TabsType } from '@/features/app/store/products/add-product';
import { useEffect } from 'react';
import { LazyImage } from '@/utils';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { useUploadPicture } from '@/hooks';

export function FormGeneral({
  form,
  type,
  setActiveTab,
}: {
  form: UseFormReturn<ProductFormData>;
  type?: ProductType;
  setActiveTab: (tab: TabsType) => void;
}) {
  const uiPicture = form.getValues('ui')?.picture;

  const { handleFileSelect, fileInputRef, croppedImageUrl } = useUploadPicture();

  useEffect(() => {
    if (croppedImageUrl) {
      form.setValue('ui', { ...form.getValues('ui'), picture: { mainLink: croppedImageUrl || '', thumbnailLink: croppedImageUrl || '' } });
    }
  }, [croppedImageUrl, form]);
  return (
    <div className="space-y-6 px-6">
      <div className="mb-6 text-center">
        <h2 className="text-foreground mb-2 text-xl font-semibold">{type === 'Session' ? 'Session Details' : 'Product Details'}</h2>
        <p className="text-muted-foreground">Fill in the details for your {type === 'Session' ? 'booking service' : 'digital product'}</p>
      </div>
      <UploadPictureDialog onUpload={(file) => form.setValue('thumbnail', file)} />

      {/* Title */}
      <FormField
        control={form.control}
        name="title"
        render={({ field }) => (
          <FormItem>
            <FormLabel className="text-foreground">Title *</FormLabel>
            <FormControl>
              <Input placeholder={type === 'Session' ? '30-minute Strategy Call' : 'Ultimate Course Bundle'} className="py-3 text-lg" {...field} />
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
            <div className="flex items-center">
              <span className="bg-muted text-muted-foreground w-it inline-flex h-full items-center rounded-l-lg border border-r-0 px-3 py-2">$</span>
              <FormControl>
                <Input
                  type="number"
                  placeholder="99"
                  className="h-full rounded-l-none py-3"
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
            {uiPicture ? (
              <LazyImage
                src={uiPicture.mainLink || ''}
                alt="Cover preview"
                className="h-full w-full object-cover"
                placeholder={uiPicture.thumbnailLink || ''}
              />
            ) : (
              <span className="text-muted-foreground text-center text-xs">No image</span>
            )}
          </div>
          <div className="flex-1">
            <Button
              onClick={() => fileInputRef.current?.click()}
              className="bg-secondary text-secondary-foreground hover:bg-secondary/80 inline-flex items-center rounded-lg px-4 py-2 transition-colors"
            >
              Choose Image
            </Button>
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
  );
}
