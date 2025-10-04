import { usePublicStore, type Product } from '@/api/stores';
import { ErrorComponenet, LoadingState } from '@/components';
import { MobileContainer, ProductCard, StoreHeader } from '@/pages/app/store';
import { routes } from '@/config';
import { useAppNavigation } from '@/hooks';
import { GenerateIdCrypto } from '@/lib';
import { useParams } from '@tanstack/react-router';

export const PublicStorePreview = () => {
  const navigate = useAppNavigation();
  const { storeSlug } = useParams({ strict: false }) as Record<string, string | undefined>;
  const { data: store, isLoading, isError } = usePublicStore(storeSlug!, { enabled: !!storeSlug });
  if (!storeSlug) return null;
  if (isLoading) return <LoadingState type="spinner" />;
  if (isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;
  if (!store) return <ErrorComponenet message="Store not found." title="Store Error" />;

  const handleProductClick = (product: Product) => {
    console.log('Product clicked:', product);
    if (product.productType !== 'Session') return; // only session products for now
    navigate.goTo({ to: routes.to.store.publicSessionProduct({ storeSlug: store.slug, productSlug: product.productSlug }) });
  };

  return (
    <div className="mx-auto flex min-h-screen max-w-md flex-col items-center justify-center gap-10">
      <MobileContainer>
        <StoreHeader store={store} />
        <div className="w-full space-y-4">
          {store.products.map((product, index) => {
            if (!product.isPublished) return null;
            return (
              <div
                key={GenerateIdCrypto()}
                onClick={() => handleProductClick(product)} // Add click handler
                className="cursor-pointer" // Indicate it's clickable
              >
                <div className="group px-6">
                  <ProductCard product={product} />
                </div>
              </div>
            );
          })}
        </div>
      </MobileContainer>
    </div>
  );
};
