import { useState } from 'react';
import { StorefrontDashboard, StoreSetupForm, AddProductFlow, ProductDetailPage, LivePreview } from '@/components/store';
import { mockStore, mockProducts } from '@/lib/mock-data';
import type { Product } from '@/types/product';

type View = 'store-setup' | 'dashboard' | 'product-creation' | 'product-detail' | 'preview-demo';

interface PreviewData {
  title?: string;
  subtitle?: string;
  price?: string;
  description?: string;
  type?: 'booking' | 'digital';
}

export function StoreBuilderDemo() {
  const [currentView, setCurrentView] = useState<View>('dashboard');
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [previewData, setPreviewData] = useState<PreviewData>({});

  const handleProductClick = (product: Product) => {
    setSelectedProduct(product);
    setCurrentView('product-detail');
  };

  const handleAddProduct = () => {
    setCurrentView('product-creation');
  };

  const handleProductCreated = (productData: any) => {
    console.log('Product created:', productData);
    setCurrentView('dashboard');
  };

  const handleStoreSetup = (storeData: any) => {
    console.log('Store created:', storeData);
    setCurrentView('dashboard');
  };

  return (
    <div className="min-h-screen bg-muted/30 p-4">
      {/* Navigation */}
      <div className="max-w-4xl mx-auto mb-6">
        <h1 className="text-2xl font-bold text-foreground mb-4">Mobile-First Store Builder Demo</h1>

        <div className="flex flex-wrap gap-2 mb-4">
          <button
            onClick={() => setCurrentView('store-setup')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              currentView === 'store-setup' ? 'bg-primary text-primary-foreground' : 'bg-card text-foreground hover:bg-accent'
            }`}
          >
            Store Setup
          </button>

          <button
            onClick={() => setCurrentView('dashboard')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              currentView === 'dashboard' ? 'bg-primary text-primary-foreground' : 'bg-card text-foreground hover:bg-accent'
            }`}
          >
            Dashboard (with products)
          </button>

          <button
            onClick={() => setCurrentView('product-creation')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              currentView === 'product-creation' ? 'bg-primary text-primary-foreground' : 'bg-card text-foreground hover:bg-accent'
            }`}
          >
            Add Product Flow
          </button>

          <button
            onClick={() => setCurrentView('preview-demo')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              currentView === 'preview-demo' ? 'bg-primary text-primary-foreground' : 'bg-card text-foreground hover:bg-accent'
            }`}
          >
            Live Preview Demo
          </button>
        </div>
      </div>

      {/* Content */}
      <div className="max-w-4xl mx-auto">
        {currentView === 'store-setup' && (
          <div className="max-w-md mx-auto bg-card p-6 rounded-xl shadow-lg">
            <StoreSetupForm onSubmit={handleStoreSetup} isLoading={false} />
          </div>
        )}

        {currentView === 'dashboard' && (
          <StorefrontDashboard
            store={mockStore}
            products={mockProducts}
            onAddProduct={handleAddProduct}
            onProductClick={handleProductClick}
            isOwner={true}
          />
        )}

        {currentView === 'product-creation' && (
          <div className="bg-card rounded-xl shadow-lg">
            <AddProductFlow onComplete={handleProductCreated} onCancel={() => setCurrentView('dashboard')} />
          </div>
        )}

        {currentView === 'product-detail' && selectedProduct && (
          <ProductDetailPage product={selectedProduct} store={mockStore} onPurchase={() => alert('Purchase flow would start here!')} />
        )}

        {currentView === 'preview-demo' && (
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* Form Side */}
            <div className="bg-card p-6 rounded-xl shadow-lg">
              <h3 className="text-lg font-semibold mb-4">Edit Product Details</h3>
              <div className="space-y-4">
                <input
                  type="text"
                  placeholder="Product title"
                  value={previewData.title || ''}
                  onChange={(e) => setPreviewData((prev) => ({ ...prev, title: e.target.value }))}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg"
                />
                <input
                  type="text"
                  placeholder="Subtitle"
                  value={previewData.subtitle || ''}
                  onChange={(e) => setPreviewData((prev) => ({ ...prev, subtitle: e.target.value }))}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg"
                />
                <input
                  type="number"
                  placeholder="Price"
                  value={previewData.price || ''}
                  onChange={(e) => setPreviewData((prev) => ({ ...prev, price: e.target.value }))}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg"
                />
                <textarea
                  placeholder="Description"
                  value={previewData.description || ''}
                  onChange={(e) => setPreviewData((prev) => ({ ...prev, description: e.target.value }))}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg"
                  rows={3}
                />
                <p className="text-sm text-muted-foreground">Changes will appear in the live preview →</p>
              </div>
            </div>

            {/* Preview Side */}
            <div>
              <LivePreview productData={previewData} storeData={mockStore} />
            </div>
          </div>
        )}
      </div>

      {/* Back Button for Product Detail */}
      {currentView === 'product-detail' && (
        <div className="fixed top-4 left-4 z-50">
          <button
            onClick={() => setCurrentView('dashboard')}
            className="bg-card text-foreground px-4 py-2 rounded-lg shadow-lg border border-border hover:bg-accent transition-colors"
          >
            ← Back to Dashboard
          </button>
        </div>
      )}
    </div>
  );
}
