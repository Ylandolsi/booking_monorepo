import { Card, CardContent } from '@/components';
import { EnhancedStorefrontDashboard } from '@/components/store';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import { mockProducts, mockStore } from '@/lib/mock-data';
import type { Product } from '@/types/product';
import { LazyImage } from '@/utils';
import { createFileRoute } from '@tanstack/react-router';
import { Calendar, Facebook, Globe, Instagram, Twitter, Youtube } from 'lucide-react';
import { useState } from 'react';
import { IPhoneMockup } from 'react-device-mockup';

export const Route = createFileRoute('/app/mystore')({
  component: RouteComponent,
});
const socialPlatforms = [
  { key: 'instagram', label: 'Instagram', icon: Instagram },
  { key: 'twitter', label: 'Twitter', icon: Twitter },
  { key: 'facebook', label: 'Facebook', icon: Facebook },
  { key: 'youtube', label: 'YouTube', icon: Youtube },
  { key: 'website', label: 'Website', icon: Globe },
];
function RouteComponent() {
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
    setProducts((prev: Product[]) => prev.map((p) => (p.id === updatedProduct.id ? updatedProduct : p)));
    setCurrentView('dashboard');
  };

  const handleStoreSetup = (storeData: any) => {
    console.log('Store created:', storeData);
    setCurrentView('dashboard');
  };
  return (
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
  );
}
