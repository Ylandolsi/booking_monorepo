import { createFileRoute, useParams } from '@tanstack/react-router';
import { useStoreBySlug } from '@/features/store/hooks';
import { StorefrontPreview } from '@/features/store/components/storefront-preview';

export const Route = createFileRoute('/store/$storeSlug')({
  component: StoreSlugPage,
});

function StoreSlugPage() {
  const { storeSlug } = useParams({ from: '/store/$storeSlug' });
  const { data: store, isLoading, error } = useStoreBySlug(storeSlug);

  if (isLoading) {
    return <div className="p-8 text-center">Loading store...</div>;
  }

  if (error || !store) {
    return (
      <div className="p-8 text-center">
        <h1 className="text-2xl mb-4">Store not found</h1>
        <p className="text-gray-600">Sorry, we couldn't find the requested store.</p>
      </div>
    );
  }

  return (
    <div className="max-w-md mx-auto p-4">
      <StorefrontPreview store={store} />
    </div>
  );
}
