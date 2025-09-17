import { createFileRoute, Link } from '@tanstack/react-router';
import { useMyStore } from '@/features/store/hooks';
import { StorefrontPreview } from '@/features/store/components/storefront-preview';
import { Button } from '@/components/ui';

export const Route = createFileRoute('/store/')({
  component: StoreIndexPage,
});

function StoreIndexPage() {
  const { data: store, isLoading, error } = useMyStore();

  if (isLoading) {
    return <div className="p-8 text-center">Loading your store...</div>;
  }

  if (error || !store) {
    return (
      <div className="p-8 text-center">
        <h1 className="text-2xl mb-4">Store not found</h1>
        <p className="mb-6 text-gray-600">You don't have a store yet. Create one to get started!</p>
        <Button asChild>
          <Link to="/store/dashboard">Create Store</Link>
        </Button>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto py-8 px-4">
      <div className="flex flex-col md:flex-row items-start gap-8">
        <div className="w-full md:w-1/2">
          <div className="bg-white p-6 rounded-lg shadow-sm">
            <h1 className="text-2xl font-bold mb-6">My Store</h1>
            <dl className="space-y-4">
              <div>
                <dt className="text-sm font-medium text-gray-500">Store Name</dt>
                <dd className="mt-1 text-lg">{store.title}</dd>
              </div>
              <div>
                <dt className="text-sm font-medium text-gray-500">Store URL</dt>
                <dd className="mt-1">
                  <a href={`/store/${store.slug}`} className="text-blue-600 hover:underline">
                    linki.com/{store.slug}
                  </a>
                </dd>
              </div>
              {store.description && (
                <div>
                  <dt className="text-sm font-medium text-gray-500">Description</dt>
                  <dd className="mt-1">{store.description}</dd>
                </div>
              )}
            </dl>

            <div className="mt-8 flex gap-4">
              <Button asChild>
                <Link to="/store/dashboard">Manage Store</Link>
              </Button>
              <Button variant="outline" asChild>
                <Link to={`/store/${store.slug}`} target="_blank">
                  View Public Store
                </Link>
              </Button>
            </div>
          </div>
        </div>

        <div className="w-full md:w-1/2 mt-8 md:mt-0">
          <StorefrontPreview store={store} />
        </div>
      </div>
    </div>
  );
}
