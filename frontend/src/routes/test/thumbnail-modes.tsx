import { MobileContainer, ProductCard } from '@/components/store';
import { mockProducts } from '@/lib/mock-data';
import { createFileRoute } from '@tanstack/react-router';
export const Route = createFileRoute('/test/thumbnail-modes')({
  component: ThumbnailModesDemo,
});
function ThumbnailModesDemo() {
  // Create products with different thumbnail modes for demonstration
  const expandedProduct = { ...mockProducts[0], thumbnailMode: 'expanded' as const };
  const compactProduct = { ...mockProducts[1], thumbnailMode: 'compact' as const };

  return (
    <div className="bg-background min-h-screen">
      <MobileContainer className="px-4 py-8">
        <div className="space-y-8">
          {/* Header */}
          <div className="text-center">
            <h1 className="text-foreground mb-2 text-2xl font-bold">Thumbnail Display Modes</h1>
            <p className="text-muted-foreground">Comparing expanded vs compact thumbnail layouts</p>
          </div>

          {/* Full Display Mode Examples */}
          <div className="space-y-6">
            <div>
              <h2 className="text-foreground mb-4 text-lg font-semibold">Full Display Mode</h2>

              <div className="space-y-4">
                <div>
                  <h3 className="text-muted-foreground mb-2 text-sm font-medium">Expanded Thumbnail (Default)</h3>
                  <ProductCard product={expandedProduct} displayMode="full" />
                </div>

                <div>
                  <h3 className="text-muted-foreground mb-2 text-sm font-medium">Compact Thumbnail</h3>
                  <ProductCard product={compactProduct} displayMode="full" />
                </div>
              </div>
            </div>

            {/* Compact Display Mode Examples */}
            <div>
              <h2 className="text-foreground mb-4 text-lg font-semibold">Compact Display Mode</h2>

              <div className="space-y-4">
                <div>
                  <h3 className="text-muted-foreground mb-2 text-sm font-medium">Expanded (12x12 icon)</h3>
                  <ProductCard product={expandedProduct} displayMode="compact" />
                </div>

                <div>
                  <h3 className="text-muted-foreground mb-2 text-sm font-medium">Compact (8x8 icon)</h3>
                  <ProductCard product={compactProduct} displayMode="compact" />
                </div>
              </div>
            </div>

            {/* Side by Side Comparison */}
            <div>
              <h2 className="text-foreground mb-4 text-lg font-semibold">Side by Side Comparison</h2>

              <div className="grid grid-cols-1 gap-4">
                <div className="space-y-2">
                  <h3 className="text-muted-foreground text-sm font-medium">Same product with different thumbnail modes:</h3>
                  <ProductCard product={mockProducts[0]} displayMode="full" />
                  <ProductCard product={mockProducts[1]} displayMode="full" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </MobileContainer>
    </div>
  );
}
