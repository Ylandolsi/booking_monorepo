import { usePublicStore } from '@/api/stores';
import { ErrorComponenet, LoadingState, MobileContainer, ProductCard, StoreHeader } from '@/components';
import { CheckoutPageProduct } from '@/features/app';
import { GenerateIdCrypto } from '@/lib';
import { useParams } from '@tanstack/react-router';

const PublicStoreLayout = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="mx-auto flex min-h-screen max-w-md flex-col items-center justify-center gap-10">
      <MobileContainer>{children}</MobileContainer>
    </div>
  );
};

export const PublicStoreProductPreview = () => {
  const { storeSlug, productSlug } = useParams({ strict: false }) as Record<string, string | undefined>;
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
    <PublicStoreLayout>
      <CheckoutPageProduct productData={store.products.find((product) => product.productSlug === productSlug)} />
    </PublicStoreLayout>
  );
};
