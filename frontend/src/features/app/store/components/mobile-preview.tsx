import { useState } from 'react';
import { useMyStore } from '@/api/stores';
import { DrawerDialog, ErrorComponenet, LoadingState, MobileContainer, ProductCard, ProductCheckout, StoreHeader } from '@/components';
import { GenerateIdCrypto } from '@/lib';
import { Button } from '@/components/ui/button'; // For close button
import { BookingPage } from '@/features/app/store/products/components/checkout/book';
import type { StoreFormData } from '@/features/app';

// New component for product details modal
const ProductDetailsModal = ({ product, storeSlug, isOpen, onClose }: { product: any; storeSlug: string; isOpen: boolean; onClose: () => void }) => {
  if (!product) return null;

  // const simulatedUrl = `appname/store/${storeSlug}/${product.productType}/${product.slug}`; // Simulate live URL

  return (
    // <DrawerDialog open={isOpen} onOpenChange={onClose} title={`Simulated URL: ${simulatedUrl}`}>
    <DrawerDialog open={isOpen} onOpenChange={onClose} title={`Product: ${product.title}`}>
      <div className="flex items-center justify-center py-10">
        <MobileContainer>
          <ProductCheckout product={product}>{product.productType == 'Session' && <BookingPage product={product} />}</ProductCheckout>
        </MobileContainer>
      </div>
    </DrawerDialog>
  );
};

export const MobilePreview = ({ storeForm }: { storeForm: StoreFormData }) => {
  const { data: store, isLoading, isError } = useMyStore();
  const [selectedProduct, setSelectedProduct] = useState<any>(null);

  if (isLoading) return <LoadingState type="spinner" />;
  if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  const mergedStore = { ...store, ...storeForm }; // Merge form data with existing store data
  console.log('Merged Store Data:', mergedStore); // Debugging line

  const handleProductClick = (product: any) => {
    setSelectedProduct(product);
  };

  const handleCloseModal = () => {
    setSelectedProduct(null);
  };

  return (
    // <main className="flex flex-1 items-center justify-center">
    <MobileContainer>
      <StoreHeader store={mergedStore} />
      <div className="w-full space-y-4">
        {store.products.map((product, index) => (
          <div
            key={GenerateIdCrypto()}
            onClick={() => handleProductClick(product)} // Add click handler
            className="cursor-pointer" // Indicate it's clickable
          >
            <div className="group px-6">
              <ProductCard product={product} />
            </div>
          </div>
        ))}
      </div>
      {/* Modal for product details */}
      <ProductDetailsModal product={selectedProduct} storeSlug={store.slug} isOpen={!!selectedProduct} onClose={handleCloseModal} />
    </MobileContainer>
    // </main>
  );
};
