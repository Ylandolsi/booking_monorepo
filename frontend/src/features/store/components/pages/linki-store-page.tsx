import { useState } from 'react';
import { MobilePreview } from '@/features/store/components/mobile-preview';
import { Button } from '@/components/ui';
import { Card, CardContent } from '@/components/ui/card';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { ProductType } from '@/features/store/types';
import type { Product } from '@/features/store/types';

// Product card component for the dashboard
const ProductCard = ({ product }: { product: Product }) => {
  return (
    <Card className="mb-4 cursor-pointer hover:shadow-md transition-shadow">
      <CardContent className="p-4">
        <div className="flex items-center gap-4">
          {product.thumbnailUrl && <img src={product.thumbnailUrl} alt={product.title} className="w-16 h-16 object-cover rounded" />}
          <div className="flex-1">
            <h3 className="font-medium">{product.title}</h3>
            {product.subtitle && <p className="text-sm text-gray-500">{product.subtitle}</p>}
            <div className="flex items-center justify-between mt-1">
              <span className="font-medium">${product.price.toFixed(2)}</span>
              <span className="text-xs px-2 py-1 bg-gray-100 rounded">{product.productType === ProductType.Session ? '1:1 Call' : 'Digital'}</span>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export const LinkiStorePage = () => {
  const [isAddProductOpen, setIsAddProductOpen] = useState(false);

  // Mock store data for development
  const mockStore = {
    id: 1,
    userId: 123,
    title: 'Linki Demo Store',
    slug: 'demo-store',
    description: 'This is a demo store for Linki',
    isPublished: true,
    step: 3,
    socialLinks: [
      { platform: 'twitter', url: 'https://twitter.com/demo' },
      { platform: 'instagram', url: 'https://instagram.com/demo' },
    ],
    products: [
      {
        id: 1,
        productSlug: 'coaching-call',
        storeId: 1,
        storeSlug: 'demo-store',
        title: 'Coaching Call',
        clickToPay: 'Book Now',
        subtitle: '1-on-1 Video Session',
        description: 'Get personalized coaching in a 30-minute video call.',
        thumbnailUrl: 'https://images.unsplash.com/photo-1596524430615-b46475ddff6e?w=300',
        productType: ProductType.Session,
        price: 49.99,
        displayOrder: 1,
        isPublished: true,
        createdAt: '2023-01-01',
        meetingInstructions: 'I will send you a Zoom link after booking.',
        duration: { minutes: 30 },
      },
      {
        id: 2,
        productSlug: 'digital-guide',
        storeId: 1,
        storeSlug: 'demo-store',
        title: 'Digital Growth Guide',
        clickToPay: 'Buy Now',
        subtitle: 'PDF Download',
        description: 'A comprehensive guide to personal growth and development.',
        thumbnailUrl: 'https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=300',
        productType: ProductType.DigitalDownload,
        price: 19.99,
        displayOrder: 2,
        isPublished: true,
        createdAt: '2023-01-15',
        files: [{ fileName: 'growth-guide.pdf', fileSize: '2.4MB' }],
      },
    ],
    createdAt: '2023-01-01',
  };

  return (
    <div className="max-w-6xl mx-auto p-4">
      <h1 className="text-2xl font-bold mb-8">Linki Store Builder</h1>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <div>
          {/* Store management section */}
          <div className="bg-white p-6 rounded-lg shadow-sm mb-8">
            <div className="flex justify-between items-center mb-6">
              <div>
                <h2 className="text-xl font-semibold">{mockStore.title}</h2>
                <p className="text-sm text-gray-500">linki.com/{mockStore.slug}</p>
              </div>
              <Button variant="outline" size="sm">
                Edit Store
              </Button>
            </div>

            <div className="space-y-4">
              <div>
                <h3 className="text-sm font-medium text-gray-500">Store Description</h3>
                <p>{mockStore.description}</p>
              </div>

              <div>
                <h3 className="text-sm font-medium text-gray-500">Social Links</h3>
                <div className="flex gap-2 mt-1">
                  {mockStore.socialLinks.map((link, i) => (
                    <Button key={i} variant="ghost" size="sm">
                      {link.platform}
                    </Button>
                  ))}
                </div>
              </div>
            </div>
          </div>

          {/* Products section */}
          <div className="bg-white p-6 rounded-lg shadow-sm">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-lg font-medium">Products</h2>
              <Button onClick={() => setIsAddProductOpen(true)}>Add Product</Button>
            </div>

            <div className="space-y-4">
              <div className="flex justify-between items-center">
                <span className="text-sm text-gray-500">{mockStore.products.length} products</span>
                <Button variant="outline" size="sm">
                  Rearrange
                </Button>
              </div>

              <div className="space-y-2">
                {mockStore.products.map((product) => (
                  <ProductCard key={product.id} product={product} />
                ))}
              </div>
            </div>
          </div>
        </div>

        {/* Mobile preview section */}
        <div className="sticky top-8 h-fit">
          <div className="bg-white p-4 rounded-lg shadow-sm">
            <h2 className="text-lg font-medium mb-4">Mobile Preview</h2>
            <MobilePreview
              formData={{
                title: mockStore.title,
                description: mockStore.description || '',
              }}
              storeInfo={{
                name: mockStore.title,
                slug: mockStore.slug,
                products: mockStore.products,
              }}
            />
          </div>
        </div>
      </div>

      {/* Simplified product dialog for demo purposes */}
      {isAddProductOpen && (
        <Dialog open={isAddProductOpen} onOpenChange={setIsAddProductOpen}>
          <DialogContent className="sm:max-w-lg">
            <DialogHeader>
              <DialogTitle>Add New Product</DialogTitle>
            </DialogHeader>
            <div className="space-y-4 py-4">
              <p>This is a simplified demo. In a real implementation, you would add product details here.</p>
              <Button onClick={() => setIsAddProductOpen(false)}>Close</Button>
            </div>
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
};
