import { createFileRoute, useParams } from '@tanstack/react-router';
import { useStoreBySlug } from '@/features/store/hooks';
import { ProductDetailPage } from '@/features/store/components/product-detail';
import { Button } from '@/components/ui';

export const Route = createFileRoute('/store/$storeSlug/$productSlug')({
  component: StoreProductDetail,
});

function StoreProductDetail() {
  const { storeSlug, productSlug } = useParams({ from: '/store/$storeSlug/$productSlug' });
  const { data: store, isLoading, error } = useStoreBySlug(storeSlug);

  if (isLoading) {
    return <div className="p-8 text-center">Loading product...</div>;
  }

  if (error || !store) {
    return (
      <div className="p-8 text-center">
        <h1 className="text-2xl mb-4">Store not found</h1>
        <p className="mb-6 text-gray-600">Sorry, we couldn't find the requested store.</p>
        <Button onClick={() => (window.location.href = '/')}>Go Home</Button>
      </div>
    );
  }

  const product = store.products?.find((p) => p.productSlug === productSlug);

  if (!product) {
    return (
      <div className="p-8 text-center">
        <h1 className="text-2xl mb-4">Product not found</h1>
        <p className="mb-6 text-gray-600">Sorry, we couldn't find the requested product.</p>
        <Button onClick={() => (window.location.href = `/store/${storeSlug}`)}>Back to Store</Button>
      </div>
    );
  }

  return <ProductDetailPage product={product} />;
}
