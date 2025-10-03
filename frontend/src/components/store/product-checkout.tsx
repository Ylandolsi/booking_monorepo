import { FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL } from '@/assets';
import { Link } from '@/components/ui';
import { routes } from '@/config';
import type { ProductCheckoutType } from '@/features';
import { cn } from '@/lib/cn';
import { Upload, X } from 'lucide-react';

export const COVER_IMAGE = {
  width: 100, // rem
  height: 80, // rem
};

// export function ProductCheckout({ product, children }: { product: Product; children?: React.ReactNode }) {
export function ProductCheckout({ product, children }: { product: ProductCheckoutType; children?: React.ReactNode }) {
  return (
    <div className="bg-background-light dark:bg-background-dark relative text-slate-800 dark:text-slate-200">
      <header className="absolute top-0 right-0 left-0 z-10 flex items-center justify-between p-4">
        {/* todo change this to home page for the user  */}
        <Link className="flex items-center gap-2" to={routes.to.store.index()}>
          <X />
          <span className="font-bold text-slate-900 dark:text-white">Back to Store</span>
        </Link>
      </header>
      <main className="w-full pt-16 pb-28 break-words">
        <div className={cn(`min-w-[${COVER_IMAGE.width}] min-h-[${COVER_IMAGE.height}]`)}>
          {/* <div className="bg-accent h-${COVER_IMAGE.height} w-${COVER_IMAGE.width} flex items-center justify-center"> */}
          {/* Change to lazy Image */}
          {/* <img
            alt="Product Image"
            className="h-full w-full object-cover"
             src={product.thumbnailPicture?.mainLink ?? FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL}

          />
            */}
          {product.thumbnailPicture?.mainLink ? (
            <img src={product.thumbnailPicture?.mainLink} alt="Cover preview" className="h-full w-full object-cover" />
          ) : (
            <div className="text-muted-foreground flex h-full w-full flex-col items-center justify-center">
              <Upload />
              <span className="text-sm">Upload your image to preview it</span>
            </div>
          )}
        </div>
        <div className="mx-auto flex flex-col justify-center overflow-x-hidden px-2 pt-4">
          <h1 className="line-clamp-2 text-3xl font-extrabold break-all text-slate-900 dark:text-white">{product.title}</h1>
          <p className="mt-2 line-clamp-3 text-lg break-all text-slate-600 dark:text-slate-400">{product.subtitle}</p>
          <div className="mt-4">
            <span className="text-primary text-4xl font-bold">{product.price?.toFixed(2) || 0}$</span>
          </div>
          <div className="mt-6 mb-10 max-h-40 space-y-4 overflow-y-auto text-slate-700 dark:text-slate-300">
            <h2 className="text-xl font-bold break-all text-slate-900 dark:text-white">Description</h2>
            <p className="break-all">{product.description}</p>
          </div>
          {children}
        </div>
      </main>
    </div>
  );
}
