import { useState } from 'react';
import { cn } from '@/lib/cn';
import type { Product, ProductType } from '@/api/stores';

export function ProductDetailsForm() {
  const [imagePreview, setImagePreview] = useState<string | null>(null);

  const handleFieldChange = (field: keyof Product, value: any) => {
    onChange({ ...data, [field]: value });
  };

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0] || null;
    handleFieldChange('coverImage', file);

    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => setImagePreview(e.target?.result as string);
      reader.readAsDataURL(file);
    } else {
      setImagePreview(null);
    }
  };

  const defaultCtaText = productType === 'booking' ? 'Book Now' : 'Buy Now';

  const isValid = data.title && data.price && (data.description || data.coverImage);

  return (
    <div className={cn('space-y-6', className)}>
      <div className="mb-6 text-center">
        <h2 className="text-foreground mb-2 text-xl font-semibold">{productType === 'booking' ? 'Booking Details' : 'Product Details'}</h2>
        <p className="text-muted-foreground">Fill in the details for your {productType === 'booking' ? 'booking service' : 'digital product'}</p>
      </div>

      {/* Title */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Title *</label>
        <input
          type="text"
          value={data.title || ''}
          onChange={(e) => handleFieldChange('title', e.target.value)}
          placeholder={productType === 'booking' ? '30-minute Strategy Call' : 'Ultimate Course Bundle'}
          className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring w-full rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
        />
      </div>

      {/* Subtitle */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Subtitle</label>
        <input
          type="text"
          value={data.subtitle || ''}
          onChange={(e) => handleFieldChange('subtitle', e.target.value)}
          placeholder="A brief description of what you offer"
          className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring w-full rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
        />
      </div>

      {/* Price */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Price *</label>
        <div className="flex">
          <span className="bg-muted border-border text-muted-foreground inline-flex items-center rounded-l-lg border border-r-0 px-3 py-2">$</span>
          <input
            type="number"
            value={data.price || ''}
            onChange={(e) => handleFieldChange('price', e.target.value)}
            placeholder="99"
            className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring flex-1 rounded-r-lg border px-3 py-2 focus:ring-2 focus:outline-none"
            min="0"
            step="0.01"
          />
        </div>
      </div>

      {/* Cover Image */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Cover Image</label>
        <div className="flex items-start space-x-4">
          <div className="bg-muted border-border flex h-24 w-24 items-center justify-center overflow-hidden rounded-lg border">
            {imagePreview ? (
              <img src={imagePreview} alt="Cover preview" className="h-full w-full object-cover" />
            ) : (
              <span className="text-muted-foreground text-center text-xs">No image</span>
            )}
          </div>
          <div className="flex-1">
            <label className="cursor-pointer">
              <input type="file" accept="image/*" onChange={handleImageChange} className="hidden" />
              <span className="bg-secondary text-secondary-foreground hover:bg-secondary/80 inline-flex items-center rounded-lg px-4 py-2 transition-colors">
                Choose Image
              </span>
            </label>
            <p className="text-muted-foreground mt-1 text-xs">Recommended: 400×400px or square format</p>
          </div>
        </div>
      </div>

      {/* Thumbnail Display Mode */}
      <ThumbnailModeSelector value={data.thumbnailMode || 'expanded'} onChange={(mode) => handleFieldChange('thumbnailMode', mode)} />

      {/* Description */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Description *</label>
        <textarea
          value={data.description || ''}
          onChange={(e) => handleFieldChange('description', e.target.value)}
          placeholder="Describe what customers will get..."
          rows={4}
          className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring w-full resize-none rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
        />
      </div>

      {/* CTA Text */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Button Text</label>
        <input
          type="text"
          value={data.ctaText || defaultCtaText}
          onChange={(e) => handleFieldChange('ctaText', e.target.value)}
          placeholder={defaultCtaText}
          className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring w-full rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
        />
      </div>

      {/* Navigation */}
      <div className="flex space-x-3 pt-4">
        {onBack && (
          <button onClick={onBack} className="border-border text-foreground hover:bg-muted flex-1 rounded-lg border px-4 py-3 transition-colors">
            Back
          </button>
        )}

        {onNext && (
          <button
            onClick={onNext}
            disabled={!isValid}
            className={cn(
              'flex-1 rounded-lg px-4 py-3 font-semibold transition-all',
              isValid ? 'bg-primary text-primary-foreground hover:opacity-90' : 'bg-muted text-muted-foreground cursor-not-allowed',
            )}
          >
            Next
          </button>
        )}
      </div>
    </div>
  );
}
