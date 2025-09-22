import { cn } from '@/lib/cn';
import { FocusedProductPreview } from './focused-product-preview';
import type { Product } from '@/api/stores';

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
    <div className={cn('bg-muted/30 min-h-screen', className)}>
      <div className="mx-auto max-w-7xl p-4">
        <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
          {/* Form Side */}
          <div className="bg-card border-border rounded-xl border shadow-lg">{children}</div>

          {/* Preview Side */}
          <div>
            <div className="lg:sticky lg:top-4">
              <FocusedProductPreview productData={previewData} storeData={storeData} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
