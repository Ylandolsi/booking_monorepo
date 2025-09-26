import { usePublicStore } from '@/api/stores';
import { ErrorComponenet, LoadingState, MobileContainer, ProductCard, StoreHeader } from '@/components';
import { GenerateIdCrypto } from '@/lib';
import { useParams } from '@tanstack/react-router';

export const PublicStorePreview = () => {
  const { storeSlug } = useParams({ strict: false }) as Record<string, string | undefined>;
  const { data: store, isLoading, isError } = usePublicStore(storeSlug!, { enabled: !!storeSlug });
  if (!storeSlug) return null;
  if (isLoading) return <LoadingState type="spinner" />;
  if (isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;
  if (!store) return <ErrorComponenet message="Store not found." title="Store Error" />;

  const handleProductClick = (product: any) => {
    // Implement navigation to product detail page
    console.log('Product clicked:', product);
  };

  console.log(store.products);
  return (
    <div className="mx-auto flex min-h-screen max-w-md flex-col items-center justify-center gap-10">
      <MobileContainer>
        <StoreHeader store={store} />
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
      </MobileContainer>
    </div>
  );
};
