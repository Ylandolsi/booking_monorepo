import { useState } from 'react';
import { cn } from '@/lib/cn';
import { ProductTypeSelector } from './product-type-selector';
import { ProductDetailsForm } from './product-details-form';
import { ResponsiveBuilderLayout } from './responsive-builder-layout';

interface ProductData {
  title: string;
  subtitle: string;
  price: string;
  description: string;
  coverImage: File | null;
  ctaText: string;
  type: 'booking' | 'digital';
}

interface AddProductFlowProps {
  onComplete: (productData: ProductData) => void;
  onCancel: () => void;
  storeData?: {
    name: string;
    description?: string;
    profilePicture?: string;
  };
  className?: string;
}

export function AddProductFlow({ onComplete, onCancel, storeData = { name: 'Your Store' }, className }: AddProductFlowProps) {
  const [step, setStep] = useState<'type-selection' | 'details' | 'specific-fields'>('type-selection');
  const [productData, setProductData] = useState<Partial<ProductData>>({});

  const handleTypeSelect = (type: 'booking' | 'digital') => {
    setProductData((prev) => ({ ...prev, type }));
    setStep('details');
  };

  const handleDetailsNext = () => {
    setStep('specific-fields');
  };

  const handleBack = () => {
    if (step === 'details') {
      setStep('type-selection');
    } else if (step === 'specific-fields') {
      setStep('details');
    }
  };

  const handleComplete = () => {
    if (productData.type && productData.title && productData.price) {
      onComplete(productData as ProductData);
    }
  };

  // Type selection doesn't need preview
  if (step === 'type-selection') {
    return (
      <div className="min-h-screen bg-muted/30 p-4">
        <div className="max-w-md mx-auto">
          <div className="bg-card rounded-xl shadow-lg border border-border p-6">
            <ProductTypeSelector onSelect={handleTypeSelect} />

            {/* Cancel Button */}
            <div className="mt-6 text-center">
              <button onClick={onCancel} className="text-sm text-muted-foreground hover:text-foreground transition-colors">
                Cancel
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Details and specific fields use responsive layout with preview
  return (
    <ResponsiveBuilderLayout previewData={productData} storeData={storeData} showPreview={true} className={className}>
      <div className="p-6">
        {/* Progress Indicator */}
        <div className="mb-8">
          <div className="flex items-center space-x-2 mb-2">
            <div className={cn('w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold', 'bg-muted text-muted-foreground')}>
              1
            </div>
            <div className="flex-1 h-px bg-border"></div>
            <div
              className={cn(
                'w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold',
                step === 'details' ? 'bg-primary text-primary-foreground' : 'bg-muted text-muted-foreground',
              )}
            >
              2
            </div>
            <div className="flex-1 h-px bg-border"></div>
            <div
              className={cn(
                'w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold',
                step === 'specific-fields' ? 'bg-primary text-primary-foreground' : 'bg-muted text-muted-foreground',
              )}
            >
              3
            </div>
          </div>
          <div className="flex justify-between text-xs text-muted-foreground">
            <span>Type</span>
            <span>Details</span>
            <span>Settings</span>
          </div>
        </div>

        {/* Content */}
        {step === 'details' && productData.type && (
          <ProductDetailsForm
            productType={productData.type}
            data={productData}
            onChange={setProductData}
            onNext={handleDetailsNext}
            onBack={handleBack}
          />
        )}

        {step === 'specific-fields' && (
          <div className="space-y-6">
            <div className="text-center mb-6">
              <h2 className="text-xl font-semibold text-foreground mb-2">
                {productData.type === 'booking' ? 'Booking Settings' : 'Digital Product Settings'}
              </h2>
              <p className="text-muted-foreground">Configure specific settings for your {productData.type}</p>
            </div>

            {/* Placeholder for specific fields */}
            <div className="p-6 bg-muted rounded-lg text-center text-muted-foreground">
              <p className="mb-4">
                {productData.type === 'booking' ? 'Booking-specific settings' : 'Digital download settings'}
                <br />
                <span className="text-xs">(Coming soon)</span>
              </p>
            </div>

            {/* Navigation */}
            <div className="flex space-x-3 pt-4">
              <button
                onClick={handleBack}
                className="flex-1 py-3 px-4 border border-border rounded-lg text-foreground hover:bg-muted transition-colors"
              >
                Back
              </button>

              <button
                onClick={handleComplete}
                className="flex-1 py-3 px-4 bg-primary text-primary-foreground rounded-lg font-semibold hover:opacity-90 transition-opacity"
              >
                Create Product
              </button>
            </div>
          </div>
        )}

        {/* Cancel Button */}
        <div className="mt-6 text-center">
          <button onClick={onCancel} className="text-sm text-muted-foreground hover:text-foreground transition-colors">
            Cancel
          </button>
        </div>
      </div>
    </ResponsiveBuilderLayout>
  );
}
