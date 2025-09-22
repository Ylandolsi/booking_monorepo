import { cn } from '@/lib/cn';
import type { CreateProductInput, Product } from '@/api/stores';
import { MobileContainer } from '@/components/store/mobile-container';
import { ProductCard } from '@/components/store/product-card';

interface ResponsiveBuilderLayoutProps {
  children: React.ReactNode;
  previewData: CreateProductInput;
  className?: string;
}

export function ResponsiveBuilderLayout({ children, previewData, className }: ResponsiveBuilderLayoutProps) {
  return (
    <div className={cn(className)}>
      <div className="mx-auto flex max-w-7xl flex-col items-center justify-around lg:flex-row lg:items-start lg:gap-40">
        {/* Form Side */}
        <div className="bg-card border-border flex-1 rounded-xl border shadow-lg">{children}</div>

        {/* Preview Side */}
        <div>
          <div className="lg:sticky lg:top-4">
            <div className={cn('bg-muted/10 p-4')}>
              <MobileContainer>
                {/* TODO : add header ?  */}
                {/* TODO : make display mode dynamic ?  */}

                <ProductCard product={previewData} displayMode="full" />
              </MobileContainer>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
