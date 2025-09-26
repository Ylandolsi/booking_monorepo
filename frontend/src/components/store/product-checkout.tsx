import type { Product } from '@/api/stores/produtcs';
import { FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL } from '@/assets';
import { Link } from '@/components/ui';
import { X } from 'lucide-react';

export const COVER_IMAGE = {
  width: 100, // rem
  height: 80, // rem
};

export function ProductCheckout({ product, children }: { product: Product; children?: React.ReactNode }) {
  return (
    <div className="bg-background-light dark:bg-background-dark relative text-slate-800 dark:text-slate-200">
      <header className="absolute top-0 right-0 left-0 z-10 flex items-center justify-between p-4">
        <Link className="flex items-center gap-2" to={'/app/store/builder'}>
          <X />
          <span className="font-bold text-slate-900 dark:text-white">My Store</span>
        </Link>
      </header>
      <main className="w-full pt-16 pb-28 break-words">
        <div className="bg-accent flex h-80 w-full items-center justify-center">
          {/* Change to lazy Image */}
          <img
            alt="Product Image"
            className="h-full w-full object-cover"
            src={product.thumbnailPicture?.mainLink ?? FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL}
          />
        </div>
        <div className="flex flex-col overflow-x-hidden p-6">
          <h1 className="line-clamp-2 text-3xl font-extrabold break-all text-slate-900 dark:text-white">{product.title}</h1>
          <p className="mt-2 line-clamp-3 text-lg break-all text-slate-600 dark:text-slate-400">{product.subtitle}</p>
          <div className="mt-4">
            <span className="text-primary text-4xl font-bold">{product.price.toFixed(2)}$</span>
          </div>
          <div className="mt-6 mb-10 max-h-40 space-y-4 overflow-y-auto text-slate-700 dark:text-slate-300">
            <h2 className="text-xl font-bold break-all text-slate-900 dark:text-white">Description</h2>
            <p className="break-all">{product.description}</p>
          </div>
          {/* <div className="bg-primary/10 dark:bg-primary/20 mt-6 rounded-lg p-4">
          <h3 className="font-bold text-slate-900 dark:text-white">Fulfillment Note</h3>
          <p className="mt-1 text-sm text-slate-700 dark:text-slate-300">{fulfillmentNote}</p>
        </div> */}

          {children}
          <button
            onClick={() => alert('Purchase completed!')}
            className="bg-primary shadow-primary/30 hover:bg-opacity-90 mt-10 h-14 w-full rounded-xl text-lg font-bold text-white shadow-lg transition-all duration-300"
          >
            Buy Now
          </button>
        </div>
      </main>
    </div>
  );
}
