import { ProductStyle, type CreateSessionProductRequest, type ProductType } from '@/api/stores/produtcs';
import type { UseFormReturn } from 'react-hook-form';
import { FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Input, UploadImage } from '@/components';
import type { ProductFormData, TabsType } from '@/pages/store/private/products/add-product-page';
import { useEffect, useRef, useState } from 'react';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { useUploadPicture } from '@/hooks';
import 'react-image-crop/dist/ReactCrop.css';
import { cn } from '@/lib/cn';
import { Upload } from 'lucide-react';
import Quill from 'quill';
import 'quill/dist/quill.snow.css';
import { COVER_IMAGE } from '@/pages/store/shared';

// ThumbnailImage : file uploaded
// ThumbnailPicture : object with mainLink and thumbnailLink

export function FormGeneral({
  form,
  type,
  setActiveTab,
  editProductData,
}: {
  form: UseFormReturn<ProductFormData>;
  type?: ProductType;
  setActiveTab: (tab: TabsType) => void;
  editProductData?: CreateSessionProductRequest;
}) {
  const { croppedImageUrl, setAspectRatio } = useUploadPicture();

  const editorRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (editorRef.current) {
      const quill = new Quill(editorRef.current, {
        theme: 'snow',
        modules: {
          toolbar: [
            // [{ header: [1, 2, false] }],
            ['bold', 'italic', 'underline'],
            // [{ list: 'ordered' }, { list: 'bullet' }],
            ['link', 'image'],
            [
              { color: ['var(--foreground)', 'var(--primary)', 'var(--secondary)', 'var(--accent)', 'var(--destructive)'] },
              { background: ['var(--background)', 'var(--muted)', 'var(--secondary)', 'var(--accent)'] },
            ],
          ],
        },
      });
      // Optional: Handle content changes
      quill.on('text-change', () => {
        form.setValue('description', quill.root.innerHTML); // Sync with form
      });
    }
  }, []);

  useEffect(() => {
    if (croppedImageUrl) {
      form.setValue('thumbnailPicture', { mainLink: croppedImageUrl || '', thumbnailLink: croppedImageUrl || '' });
    } else {
      // restore to default
      // form.setValue(
      //   'thumbnailPicture',
      //   editProductData?  || {
      //     mainLink: '',
      //     thumbnailLink: '',
      //   },
      // );
    }
  }, [croppedImageUrl, form]);

  const handleAspectRatioChange = (newRatio: number) => {
    setAspectRatio(newRatio);
  };

  useEffect(() => {
    handleAspectRatioChange(16 / 9);
  }, []);

  return (
    <>
      <div className="mb-6 text-center">
        <h2 className="text-foreground mb-2 text-xl font-semibold">{type === 'Session' ? 'Session Details' : 'Product Details'}</h2>
        <p className="text-muted-foreground">Fill in the details for your {type === 'Session' ? 'booking service' : 'digital product'}</p>
      </div>
      <UploadPictureDialog onUpload={(file) => form.setValue('thumbnailImage', file)} />

      <div className="flex w-full flex-wrap gap-4">
        {/* Title */}
        <FormField
          control={form.control}
          name="title"
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel className="text-foreground">Title *</FormLabel>
              <FormControl>
                <Input placeholder={type === 'Session' ? '30-minute Strategy Call' : 'Ultimate Course Bundle'} className="py-3" {...field} />
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
            <FormItem className="flex-1">
              <FormLabel className="text-foreground">Subtitle</FormLabel>
              <FormControl>
                <Input placeholder="A brief description of what you offer" className="py-3" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
      </div>

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
      {/* Product Style */}
      <ProductStyleField form={form} />
      {/* Thumbnail Image */}
      <UploadImage description="Thumbnail Image (Optional)" />
      <div className="flex items-center justify-center">
        <div className={cn(`w-${COVER_IMAGE.width} h-${COVER_IMAGE.height}`)}>
          {croppedImageUrl ? (
            <img src={croppedImageUrl} alt="Cover preview" className="h-full w-full object-cover" />
          ) : (
            <div className="text-muted-foreground flex h-full w-full flex-col items-center justify-center">
              <Upload />
              <span className="text-sm">Upload your image to preview it</span>
            </div>
          )}
          {/* <img src={croppedImageUrl ?? FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL} alt="Cover preview" className="h-full w-full object-cover" /> */}
        </div>
      </div>

      {/* Description */}
      <FormField
        control={form.control}
        name="description"
        render={({ field }) => (
          <FormItem className="gap-0">
            <FormLabel className="text-foreground mb-2">Description *</FormLabel>
            <FormControl>
              <div {...field} ref={editorRef} />
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
    </>
  );
}

function ProductStyleField({ form }: { form: UseFormReturn<ProductFormData> }) {
  const [productStyle, setProductStyle] = useState<ProductStyle>(form.getValues('productStyle') || ProductStyle.Full);
  return (
    <div className="space-y-4">
      <h2 className="text-foreground mb-2 text-xl font-semibold">Pick a style </h2>
      <p className="text-muted-foreground">Choose the style for your product</p>
      <div className="flex flex-wrap gap-2">
        {Object.values(ProductStyle).map((style) => (
          <div
            key={style}
            className={cn('cursor-pointer rounded border p-4', productStyle === style ? 'border-primary' : 'border-muted hover:border-foreground')}
            onClick={() => {
              setProductStyle(style);
              form.setValue('productStyle', style);
            }}
          >
            {/* <img
              src={`/assets/product-styles/${style.toLowerCase()}.png`}
              alt={style}
              className={cn('h-40 w-60 object-cover', productStyle === style ? '' : 'opacity-50')}
            /> */}
            <div className="mt-2 text-center">
              <span className="text-foreground">{style}</span>
            </div>
          </div>
        ))}
      </div>
      <FormField
        control={form.control}
        name="productStyle"
        render={({ field }) => (
          <FormItem>
            <FormMessage />
          </FormItem>
        )}
      />
    </div>
  );
}
