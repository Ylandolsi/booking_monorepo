import { usePublicStore } from '@/api/stores';
import { ErrorComponenet, LoadingState } from '@/components';
import { CheckoutPageProduct, MobileContainer } from '@/pages/app/store';
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

  const productData = store.products.find((product) => product.productSlug === productSlug);

  if (!productData) {
    return (
      <PublicStoreLayout>
        <ErrorComponenet message="Product not found." title="Product Error" />
      </PublicStoreLayout>
    );
  }
  return (
    <PublicStoreLayout>
      <CheckoutPageProduct productData={productData} />
    </PublicStoreLayout>
  );
};
