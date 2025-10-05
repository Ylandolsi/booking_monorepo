import { useState, type Dispatch, type SetStateAction } from 'react';
import { useMyStore, type Product } from '@/api/stores';
import { DrawerDialog, ErrorComponenet, LoadingState } from '@/components';
import { MobileContainer, ProductCard, ProductCheckout, StoreHeader } from '@/pages/app/store';
import { GenerateIdCrypto } from '@/lib';
import { BookingPage } from '@/pages/app/store/private/products/book';
import type { StoreFormData } from '@/pages/app';

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

export const MobilePreview = ({
  storeForm,
  productsRearranged,
  setProducts,
}: {
  storeForm: StoreFormData;
  productsRearranged: Product[];
  setProducts: Dispatch<SetStateAction<Product[]>>;
}) => {
  const { data: store, isLoading, isError } = useMyStore();
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  if (isLoading) return <LoadingState type="spinner" />;
  if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  const mergedStore = { ...store, ...storeForm, products: productsRearranged }; // Merge form data with existing store data

  const handleProductClick = (product: Product) => {
    setSelectedProduct(product);
  };

  const handleCloseModal = () => {
    setSelectedProduct(null);
  };

  return (
    // <main className="flex flex-1 items-center justify-center">
    <MobileContainer>
      <StoreHeader store={mergedStore} />
      <div className="w-full space-y-4 pb-10">
        {mergedStore.products.map((product, index) => {
          if (!product.isPublished) return null;
          return (
            <div
              key={GenerateIdCrypto()}
              onClick={() => handleProductClick(product)} // Add click handler
              className="cursor-pointer" // Indicate it's clickable
            >
              <div className="group px-6">
                <ProductCard product={product} setProducts={setProducts} />
              </div>
            </div>
          );
        })}
      </div>
      {/* Modal for product details */}
      <ProductDetailsModal product={selectedProduct} storeSlug={store.slug} isOpen={!!selectedProduct} onClose={handleCloseModal} />
    </MobileContainer>
    // </main>
  );
};
