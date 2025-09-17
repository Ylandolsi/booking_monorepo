import type { Store } from '../types';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Button } from '@/components/ui';

interface StorefrontPreviewProps {
  store: Store;
}

export const StorefrontPreview = ({ store }: StorefrontPreviewProps) => {
  return (
    <div className="w-full max-w-md mx-auto bg-white rounded-xl shadow-lg overflow-hidden md:max-w-2xl">
      <div className="p-8">
        <div className="flex flex-col items-center">
          <Avatar className="w-24 h-24">
            <AvatarImage src={store.picture?.url} alt={store.picture?.altText || store.title} />
            <AvatarFallback>{store.title.charAt(0)}</AvatarFallback>
          </Avatar>
          <h1 className="mt-4 text-2xl font-bold">{store.title}</h1>
          <p className="text-gray-600">@{store.slug}</p>
          <p className="mt-2 text-center">{store.description}</p>
        </div>

        <div className="mt-8">
          <Button className="w-full">+ Add Product</Button>
        </div>

        <div className="mt-4 space-y-4">
          {/* Product list will go here */}
          {store.products.length === 0 && (
            <div className="text-center text-gray-500 py-8">
              <p>Your store is empty!</p>
              <p>Click "+ Add Product" to get started.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
