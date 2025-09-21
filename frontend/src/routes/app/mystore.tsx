import { EnhancedStorefrontDashboard } from '@/components/store';
import { mockProducts, mockStore } from '@/lib/mock-data';
import type { Product } from '@/types/product';
import { createFileRoute } from '@tanstack/react-router';
import { useState } from 'react';

export const Route = createFileRoute('/app/mystore')({
  component: RouteComponent,
});

function RouteComponent() {
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  const [products, setProducts] = useState<Product[]>(mockProducts);

  const handleProductClick = (product: Product) => {
    setSelectedProduct(product);
  };

  const handleProductEdit = (product: Product) => {
    setSelectedProduct(product);
  };

  const handleProductDelete = (product: Product) => {
    if (confirm(`Delete "${product.title}"?`)) {
      setProducts((prev) => prev.filter((p) => p.id !== product.id));
    }
  };

  const handleProductsReorder = (reorderedProducts: Product[]) => {
    setProducts(reorderedProducts);
  };

  const handleAddProduct = () => {};

  const handleProductCreated = (productData: any) => {
    const newProduct: Product = {
      ...productData,
      id: `new-${Date.now()}`,
      price: productData.price.startsWith('$') ? productData.price : `$${productData.price}`,
    };
    setProducts((prev) => [newProduct, ...prev]);
  };

  const handleProductUpdated = (updatedProduct: Product) => {
    setProducts((prev: Product[]) => prev.map((p) => (p.id === updatedProduct.id ? updatedProduct : p)));
  };

  const handleStoreSetup = (storeData: any) => {
    console.log('Store created:', storeData);
  };
  return (
    <div className="flex flex-col items-center">
      <EnhancedStorefrontDashboard
        products={products}
        onAddProduct={handleAddProduct}
        onProductClick={handleProductClick}
        onProductEdit={handleProductEdit}
        onProductDelete={handleProductDelete}
        onProductsReorder={handleProductsReorder}
        isOwner={true}
      />
    </div>
  );
}
