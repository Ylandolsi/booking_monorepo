import { useState } from 'react';
import { cn } from '@/lib/cn';

interface ProductData {
  title: string;
  subtitle: string;
  price: string;
  description: string;
  coverImage: File | null;
  ctaText: string;
  type: 'booking' | 'digital';
}

interface ProductDetailsFormProps {
  productType: 'booking' | 'digital';
  data: Partial<ProductData>;
  onChange: (data: Partial<ProductData>) => void;
  onNext?: () => void;
  onBack?: () => void;
  className?: string;
}

export function ProductDetailsForm({ productType, data, onChange, onNext, onBack, className }: ProductDetailsFormProps) {
  const [imagePreview, setImagePreview] = useState<string | null>(null);

  const handleFieldChange = (field: keyof ProductData, value: any) => {
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
      <div className="text-center mb-6">
        <h2 className="text-xl font-semibold text-foreground mb-2">{productType === 'booking' ? 'Booking Details' : 'Product Details'}</h2>
        <p className="text-muted-foreground">Fill in the details for your {productType === 'booking' ? 'booking service' : 'digital product'}</p>
      </div>

      {/* Title */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Title *</label>
        <input
          type="text"
          value={data.title || ''}
          onChange={(e) => handleFieldChange('title', e.target.value)}
          placeholder={productType === 'booking' ? '30-minute Strategy Call' : 'Ultimate Course Bundle'}
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
        />
      </div>

      {/* Subtitle */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Subtitle</label>
        <input
          type="text"
          value={data.subtitle || ''}
          onChange={(e) => handleFieldChange('subtitle', e.target.value)}
          placeholder="A brief description of what you offer"
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
        />
      </div>

      {/* Price */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Price *</label>
        <div className="flex">
          <span className="inline-flex items-center px-3 py-2 bg-muted border border-r-0 border-border rounded-l-lg text-muted-foreground">$</span>
          <input
            type="number"
            value={data.price || ''}
            onChange={(e) => handleFieldChange('price', e.target.value)}
            placeholder="99"
            className="flex-1 px-3 py-2 bg-input border border-border rounded-r-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
            min="0"
            step="0.01"
          />
        </div>
      </div>

      {/* Cover Image */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Cover Image</label>
        <div className="flex items-start space-x-4">
          <div className="w-24 h-24 rounded-lg overflow-hidden bg-muted flex items-center justify-center border border-border">
            {imagePreview ? (
              <img src={imagePreview} alt="Cover preview" className="w-full h-full object-cover" />
            ) : (
              <span className="text-muted-foreground text-xs text-center">No image</span>
            )}
          </div>
          <div className="flex-1">
            <label className="cursor-pointer">
              <input type="file" accept="image/*" onChange={handleImageChange} className="hidden" />
              <span className="inline-flex items-center px-4 py-2 bg-secondary text-secondary-foreground rounded-lg hover:bg-secondary/80 transition-colors">
                Choose Image
              </span>
            </label>
            <p className="text-xs text-muted-foreground mt-1">Recommended: 400×400px or square format</p>
          </div>
        </div>
      </div>

      {/* Description */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Description *</label>
        <textarea
          value={data.description || ''}
          onChange={(e) => handleFieldChange('description', e.target.value)}
          placeholder="Describe what customers will get..."
          rows={4}
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring resize-none"
        />
      </div>

      {/* CTA Text */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Button Text</label>
        <input
          type="text"
          value={data.ctaText || defaultCtaText}
          onChange={(e) => handleFieldChange('ctaText', e.target.value)}
          placeholder={defaultCtaText}
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
        />
      </div>

      {/* Navigation */}
      <div className="flex space-x-3 pt-4">
        {onBack && (
          <button onClick={onBack} className="flex-1 py-3 px-4 border border-border rounded-lg text-foreground hover:bg-muted transition-colors">
            Back
          </button>
        )}

        {onNext && (
          <button
            onClick={onNext}
            disabled={!isValid}
            className={cn(
              'flex-1 py-3 px-4 rounded-lg font-semibold transition-all',
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
