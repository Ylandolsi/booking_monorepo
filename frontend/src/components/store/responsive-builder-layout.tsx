import { cn } from '@/lib/cn';
import { FocusedProductPreview } from './focused-product-preview';
import type { Product } from '@/types/product';

interface ResponsiveBuilderLayoutProps {
  children: React.ReactNode;
  previewData: Partial<Product>;
  storeData: {
    name: string;
    description?: string;
    profilePicture?: string;
  };
  showPreview?: boolean;
  className?: string;
}

export function ResponsiveBuilderLayout({ children, previewData, storeData, showPreview = true, className }: ResponsiveBuilderLayoutProps) {
  return (
    <div className={cn('min-h-screen bg-muted/30', className)}>
      <div className="max-w-7xl mx-auto p-4">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Form Side */}
          <div className="order-2 lg:order-1">
            <div className="bg-card rounded-xl shadow-lg border border-border">{children}</div>
          </div>

          {/* Preview Side */}
          {showPreview && (
            <div className="order-1 lg:order-2">
              <div className="lg:sticky lg:top-4">
                <FocusedProductPreview productData={previewData} storeData={storeData} />
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
