import { useState } from 'react';
import {
  EnhancedStorefrontDashboard,
  StoreSetupForm,
  AddProductFlow,
  EditProductFlow,
  ProductDetailPage,
  FocusedProductPreview,
} from '@/components/store';
import { mockStore, mockProducts } from '@/lib/mock-data';
import type { Product } from '@/types/product';

type View = 'store-setup' | 'dashboard' | 'product-creation' | 'product-edit' | 'product-detail' | 'preview-demo';

interface PreviewData {
  title?: string;
  subtitle?: string;
  price?: string;
  description?: string;
  type?: 'booking' | 'digital';
}

export function EnhancedStoreBuilderDemo() {
  const [currentView, setCurrentView] = useState<View>('dashboard');
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [previewData, setPreviewData] = useState<PreviewData>({
    title: 'Sample Product',
    subtitle: 'This is a sample product description',
    price: '99',
    description: 'This is where the product description would go...',
    type: 'digital',
  });
  const [products, setProducts] = useState<Product[]>(mockProducts);

  const handleProductClick = (product: Product) => {
    setSelectedProduct(product);
    setCurrentView('product-detail');
  };

  const handleProductEdit = (product: Product) => {
    setSelectedProduct(product);
    setCurrentView('product-edit');
  };

  const handleProductDelete = (product: Product) => {
    if (confirm(`Delete "${product.title}"?`)) {
      setProducts((prev) => prev.filter((p) => p.id !== product.id));
    }
  };

  const handleProductsReorder = (reorderedProducts: Product[]) => {
    setProducts(reorderedProducts);
  };

  const handleAddProduct = () => {
    setCurrentView('product-creation');
  };

  const handleProductCreated = (productData: any) => {
    const newProduct: Product = {
      ...productData,
      id: `new-${Date.now()}`,
      price: productData.price.startsWith('$') ? productData.price : `$${productData.price}`,
    };
    setProducts((prev) => [newProduct, ...prev]);
    setCurrentView('dashboard');
  };

  const handleProductUpdated = (updatedProduct: Product) => {
    setProducts((prev) => prev.map((p) => (p.id === updatedProduct.id ? updatedProduct : p)));
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
        <h1 className="text-2xl font-bold text-foreground mb-4">Enhanced Store Builder Demo</h1>

        <div className="bg-card p-4 rounded-lg border border-border mb-4">
          <h2 className="font-semibold text-foreground mb-2">✨ New Features</h2>
          <ul className="text-sm text-muted-foreground space-y-1">
            <li>
              • <strong>Product Display Modes:</strong> Switch between full and compact views
            </li>
            <li>
              • <strong>Focused Live Preview:</strong> See individual products during creation
            </li>
            <li>
              • <strong>Responsive Builder:</strong> Desktop layout with side-by-side editing
            </li>
            <li>
              • <strong>Edit Products:</strong> Click "Edit" on any product to modify it
            </li>
            <li>
              • <strong>Drag & Drop:</strong> Reorder products by dragging (when logged in as owner)
            </li>
          </ul>
        </div>

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
            Enhanced Dashboard
          </button>

          <button
            onClick={() => setCurrentView('product-creation')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              currentView === 'product-creation' ? 'bg-primary text-primary-foreground' : 'bg-card text-foreground hover:bg-accent'
            }`}
          >
            Add Product (Responsive)
          </button>

          <button
            onClick={() => setCurrentView('preview-demo')}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              currentView === 'preview-demo' ? 'bg-primary text-primary-foreground' : 'bg-card text-foreground hover:bg-accent'
            }`}
          >
            Focused Preview Demo
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
          <EnhancedStorefrontDashboard
            store={mockStore}
            products={products}
            onAddProduct={handleAddProduct}
            onProductClick={handleProductClick}
            onProductEdit={handleProductEdit}
            onProductDelete={handleProductDelete}
            onProductsReorder={handleProductsReorder}
            isOwner={true}
          />
        )}

        {currentView === 'product-creation' && (
          <AddProductFlow onComplete={handleProductCreated} onCancel={() => setCurrentView('dashboard')} storeData={mockStore} />
        )}

        {currentView === 'product-edit' && selectedProduct && (
          <EditProductFlow
            product={selectedProduct}
            onComplete={handleProductUpdated}
            onCancel={() => setCurrentView('dashboard')}
            storeData={mockStore}
          />
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
                <select
                  value={previewData.type || 'digital'}
                  onChange={(e) => setPreviewData((prev) => ({ ...prev, type: e.target.value as 'booking' | 'digital' }))}
                  className="w-full px-3 py-2 bg-input border border-border rounded-lg"
                >
                  <option value="digital">Digital Download</option>
                  <option value="booking">1:1 Booking</option>
                </select>
                <p className="text-sm text-muted-foreground">Changes will appear in the focused preview →</p>
              </div>
            </div>

            {/* Preview Side */}
            <div>
              <FocusedProductPreview productData={previewData} storeData={mockStore} />
            </div>
          </div>
        )}
      </div>

      {/* Back Button for Product Detail/Edit */}
      {(currentView === 'product-detail' || currentView === 'product-edit') && (
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
