import { useState } from 'react';
import { EnhancedStorefrontDashboard, EditProductFlow, ProductDetailPage } from '@/components/store';
import { AddProductFlowV2 } from './add-product-flow-v2';
import { initialStore, initProducts, type Product } from '@/api/stores';

type View = 'store-setup' | 'dashboard' | 'product-creation' | 'product-edit' | 'product-detail' | 'preview-demo';

interface PreviewData {
  title?: string;
  subtitle?: string;
  price?: string;
  description?: string;
  type?: 'booking' | 'digital';
}

export function EnhancedStoreBuilderDemo() {
  const [currentView, setCurrentView] = useState<View>('product-edit');
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  const [products, setProducts] = useState<Product[]>(initProducts);

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
      setProducts((prev) => prev.filter((p) => p.productSlug !== product.productSlug));
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
    setCurrentView('product-creation');
  };

  const handleProductUpdated = (updatedProduct: Product) => {
    setProducts((prev) => prev.map((p) => (p.productSlug === updatedProduct.productSlug ? updatedProduct : p)));
    setCurrentView('product-creation');
  };

  return (
    <div className="bg-muted/30 min-h-screen p-4">
      {/* Content */}
      <div className="mx-auto max-w-4xl">
        {currentView === 'dashboard' && (
          <EnhancedStorefrontDashboard
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
          <AddProductFlowV2 onComplete={handleProductCreated} onCancel={() => setCurrentView('dashboard')} storeData={initialStore} />
        )}

        {currentView === 'product-edit' && selectedProduct && (
          <EditProductFlow
            product={selectedProduct}
            onComplete={handleProductUpdated}
            onCancel={() => setCurrentView('dashboard')}
            storeData={initialStore}
          />
        )}

        {currentView === 'product-detail' && selectedProduct && (
          <ProductDetailPage product={selectedProduct} store={initialStore} onPurchase={() => alert('Purchase flow would start here!')} />
        )}
      </div>

      {/* Back Button for Product Detail/Edit */}
      {(currentView === 'product-detail' || currentView === 'product-edit') && (
        <div className="fixed top-4 left-4 z-50">
          <button
            onClick={() => setCurrentView('dashboard')}
            className="bg-card text-foreground border-border hover:bg-accent rounded-lg border px-4 py-2 shadow-lg transition-colors"
          >
            ← Back to Dashboard
          </button>
        </div>
      )}
    </div>
  );
}
