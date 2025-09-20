import { useState } from 'react';
import { cn } from '@/lib/cn';
import { ResponsiveBuilderLayout } from './responsive-builder-layout';
import { ThumbnailModeSelector } from './thumbnail-mode-selector';
import { TabNavigation } from './tab-navigation';
import { ProductSpecificDetails } from './product-specific-details';
import { LinksIntegrations } from './links-integrations';
import type { Product, DigitalProductDetails, BookingDetails, Integration } from '@/types/product';

interface EditProductFlowProps {
  product: Product;
  onComplete: (productData: Product) => void;
  onCancel: () => void;
  storeData?: {
    name: string;
    description?: string;
    profilePicture?: string;
  };
  className?: string;
}

interface ProductFormData {
  title: string;
  subtitle: string;
  price: string;
  description: string;
  coverImage: File | string | null;
  ctaText: string;
  type: 'booking' | 'digital';
  thumbnailMode: 'compact' | 'expanded';
  digitalDetails: DigitalProductDetails;
  bookingDetails: BookingDetails;
  integrations: Integration[];
}

export function EditProductFlow({ product, onComplete, onCancel, storeData = { name: 'Your Store' }, className }: EditProductFlowProps) {
  const [activeTab, setActiveTab] = useState<'general' | 'details' | 'integrations'>('general');
  const [productData, setProductData] = useState<ProductFormData>({
    title: product.title,
    subtitle: product.subtitle || '',
    price: product.price.replace('$', ''),
    description: product.description || '',
    coverImage: product.coverImage,
    ctaText: product.ctaText,
    type: product.type,
    thumbnailMode: product.thumbnailMode || 'expanded',
    digitalDetails: product.digitalDetails || {
      downloadFile: null,
      downloadLink: '',
      previewMedia: null,
    },
    bookingDetails: product.bookingDetails || {
      duration: 30,
      bufferTime: 15,
      timezone: 'UTC',
      meetingLink: '',
      meetingPlatform: 'zoom',
      maxAttendees: 1,
      availableSlots: [],
    },
    integrations: product.integrations || [],
  });

  const handleComplete = () => {
    const updatedProduct: Product = {
      ...product,
      title: productData.title,
      subtitle: productData.subtitle,
      price: productData.price.startsWith('$') ? productData.price : `$${productData.price}`,
      description: productData.description,
      coverImage:
        typeof productData.coverImage === 'string'
          ? productData.coverImage
          : productData.coverImage
            ? URL.createObjectURL(productData.coverImage)
            : '',
      ctaText: productData.ctaText,
      type: productData.type,
      thumbnailMode: productData.thumbnailMode,
      digitalDetails: productData.digitalDetails,
      bookingDetails: productData.bookingDetails,
      integrations: productData.integrations,
    };

    onComplete(updatedProduct);
  };

  const isValid = productData.title && productData.price && (productData.description || productData.coverImage);

  return (
    <ResponsiveBuilderLayout
      previewData={{
        ...productData,
        id: product.id,
        price: productData.price.startsWith('$') ? productData.price : `$${productData.price}`,
        coverImage:
          typeof productData.coverImage === 'string'
            ? productData.coverImage
            : productData.coverImage
              ? URL.createObjectURL(productData.coverImage)
              : '',
      }}
      storeData={storeData}
      showPreview={true}
      className={className}
    >
      <div className="flex flex-col h-full">
        {/* Header */}
        <div className="px-6 pt-6 pb-4">
          <div className="text-center mb-4">
            <h2 className="text-xl font-semibold text-foreground mb-2">Edit Product</h2>
            <p className="text-muted-foreground">Update your {productData.type === 'booking' ? 'booking service' : 'digital product'}</p>
          </div>

          {/* Tab Navigation */}
          <TabNavigation
            tabs={[
              { id: 'general', label: 'General Info', description: 'Title, image, pricing', icon: '📝' },
              {
                id: 'details',
                label: 'Details',
                description: productData.type === 'booking' ? 'Scheduling & meetings' : 'Files & downloads',
                icon: productData.type === 'booking' ? '📅' : '📁',
              },
              { id: 'integrations', label: 'Links', description: 'Social & integrations', icon: '🔗' },
            ]}
            activeTab={activeTab}
            onTabChange={(tabId) => setActiveTab(tabId as 'general' | 'details' | 'integrations')}
          />
        </div>

        {/* Tab Content */}
        <div className="flex-1 px-6 pb-6 overflow-y-auto">
          {activeTab === 'general' && (
            <div className="space-y-6">
              {/* Title */}
              <div className="space-y-2">
                <label className="block text-sm font-medium text-foreground">Title *</label>
                <input
                  type="text"
                  value={productData.title}
                  onChange={(e) => setProductData((prev) => ({ ...prev, title: e.target.value }))}
                  placeholder={productData.type === 'booking' ? '30-minute Strategy Call' : 'Ultimate Course Bundle'}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
                />
              </div>

              {/* Subtitle */}
              <div className="space-y-2">
                <label className="block text-sm font-medium text-foreground">Subtitle</label>
                <input
                  type="text"
                  value={productData.subtitle}
                  onChange={(e) => setProductData((prev) => ({ ...prev, subtitle: e.target.value }))}
                  placeholder="A brief description of what you offer"
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
                />
              </div>

              {/* Price */}
              <div className="space-y-2">
                <label className="block text-sm font-medium text-foreground">Price *</label>
                <div className="flex">
                  <span className="inline-flex items-center px-3 py-2 bg-muted border border-r-0 border-border rounded-l-lg text-muted-foreground">
                    $
                  </span>
                  <input
                    type="number"
                    value={productData.price}
                    onChange={(e) => setProductData((prev) => ({ ...prev, price: e.target.value }))}
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
                    {productData.coverImage ? (
                      <img
                        src={typeof productData.coverImage === 'string' ? productData.coverImage : URL.createObjectURL(productData.coverImage)}
                        alt="Cover preview"
                        className="w-full h-full object-cover"
                      />
                    ) : (
                      <span className="text-muted-foreground text-xs text-center">No image</span>
                    )}
                  </div>
                  <div className="flex-1">
                    <label className="cursor-pointer">
                      <input
                        type="file"
                        accept="image/*"
                        onChange={(e) => {
                          const file = e.target.files?.[0] || null;
                          setProductData((prev) => ({ ...prev, coverImage: file }));
                        }}
                        className="hidden"
                      />
                      <span className="inline-flex items-center px-4 py-2 bg-secondary text-secondary-foreground rounded-lg hover:bg-secondary/80 transition-colors">
                        Change Image
                      </span>
                    </label>
                    <p className="text-xs text-muted-foreground mt-1">Recommended: 400×400px or square format</p>
                  </div>
                </div>
              </div>

              {/* Thumbnail Display Mode */}
              <ThumbnailModeSelector
                value={productData.thumbnailMode}
                onChange={(mode) => setProductData((prev) => ({ ...prev, thumbnailMode: mode }))}
              />

              {/* Description */}
              <div className="space-y-2">
                <label className="block text-sm font-medium text-foreground">Description *</label>
                <textarea
                  value={productData.description}
                  onChange={(e) => setProductData((prev) => ({ ...prev, description: e.target.value }))}
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
                  value={productData.ctaText}
                  onChange={(e) => setProductData((prev) => ({ ...prev, ctaText: e.target.value }))}
                  placeholder={productData.type === 'booking' ? 'Book Now' : 'Buy Now'}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
                />
              </div>
            </div>
          )}

          {activeTab === 'details' && (
            <ProductSpecificDetails
              productType={productData.type}
              digitalDetails={productData.digitalDetails}
              bookingDetails={productData.bookingDetails}
              onDigitalDetailsChange={(details) => setProductData((prev) => ({ ...prev, digitalDetails: details }))}
              onBookingDetailsChange={(details) => setProductData((prev) => ({ ...prev, bookingDetails: details }))}
            />
          )}

          {activeTab === 'integrations' && (
            <LinksIntegrations
              integrations={productData.integrations}
              onIntegrationsChange={(integrations) => setProductData((prev) => ({ ...prev, integrations }))}
            />
          )}
        </div>

        {/* Fixed Bottom Navigation */}
        <div className="border-t border-border bg-card px-6 py-4">
          <div className="flex space-x-3">
            <button onClick={onCancel} className="flex-1 py-3 px-4 border border-border rounded-lg text-foreground hover:bg-muted transition-colors">
              Cancel
            </button>

            <button
              onClick={handleComplete}
              disabled={!isValid}
              className={cn(
                'flex-1 py-3 px-4 rounded-lg font-semibold transition-all',
                isValid ? 'bg-primary text-primary-foreground hover:opacity-90' : 'bg-muted text-muted-foreground cursor-not-allowed',
              )}
            >
              Save Changes
            </button>
          </div>
        </div>
      </div>
    </ResponsiveBuilderLayout>
  );
}
