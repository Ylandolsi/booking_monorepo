import { LazyImage } from '@/utils';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import { socialPlatforms } from '@/features/app/store/create/setup-store';
import { Button } from '@/components/ui';
import { cn } from '@/lib/cn';
import type { Product, Store } from '@/api/stores';

interface MobileStorePreviewProps {
  store: Store;
  products: Product[];
  className?: string;
}

export function MobileStorePreview({ store, products, className }: MobileStorePreviewProps) {
  return (
    <div className={cn('sticky top-4', className)}>
      {/* Phone Frame */}
      <div className="relative mx-auto w-[320px] rounded-[2.5rem] border-8 border-gray-800 bg-gray-800 shadow-xl">
        {/* Phone Screen */}
        <div className="relative h-[640px] w-full overflow-hidden rounded-[1.5rem] bg-white">
          {/* Status Bar */}
          <div className="flex h-6 items-center justify-between bg-gray-900 px-4 text-xs text-white">
            <span>9:41</span>
            <div className="flex items-center gap-1">
              <div className="h-1 w-4 rounded-full bg-white"></div>
              <div className="h-1 w-1 rounded-full bg-white"></div>
              <div className="h-1 w-6 rounded-sm bg-white"></div>
            </div>
          </div>

          {/* Store Header with Brand Color */}
          <div className="bg-gradient-to-b from-purple-600 to-purple-700 px-4 py-6 text-white">
            <div className="text-center">
              <div className="mb-3 flex justify-center">
                <LazyImage
                  src={store?.picture?.mainLink ?? FALLBACK_PROFILE_PICTURE}
                  placeholder={store?.picture?.thumbnailLink ?? FALLBACK_PROFILE_PICTURE}
                  alt="Store Profile"
                  className="h-16 w-16 rounded-full border-2 border-white object-cover shadow-lg"
                />
              </div>
              <h1 className="mb-1 text-lg font-bold">{store.title || 'My Store'}</h1>
              <p className="mb-2 text-sm opacity-90">@{store.slug}</p>
              {store.description && <p className="mb-3 text-sm leading-relaxed opacity-80">{store.description}</p>}
              {store.socialLinks && store.socialLinks.length > 0 && (
                <div className="flex justify-center gap-3">
                  {socialPlatforms.slice(0, 4).map(
                    ({ key, icon: Icon }) =>
                      store.socialLinks?.find((link: any) => link.platform === key)?.url && (
                        <div key={key} className="flex h-8 w-8 items-center justify-center rounded-full bg-white/20 backdrop-blur-sm">
                          <Icon className="h-4 w-4" />
                        </div>
                      ),
                  )}
                </div>
              )}
            </div>
          </div>

          {/* Products Section */}
          <div className="flex-1 overflow-y-auto bg-gray-50 px-4 py-4">
            <div className="space-y-3">
              {products.map((product) => (
                <ProductPreviewCard key={product.productSlug} product={product} />
              ))}

              {products.length === 0 && (
                <div className="py-12 text-center">
                  <div className="mx-auto mb-3 flex h-16 w-16 items-center justify-center rounded-full bg-gray-200">
                    <span className="text-2xl">📦</span>
                  </div>
                  <p className="text-sm text-gray-500">No products yet</p>
                  <p className="mt-1 text-xs text-gray-400">Products will appear here</p>
                </div>
              )}
            </div>
          </div>

          {/* Bottom Navigation Indicator */}
          <div className="absolute bottom-2 left-1/2 h-1 w-32 -translate-x-1/2 rounded-full bg-black"></div>
        </div>
      </div>
    </div>
  );
}

interface ProductPreviewCardProps {
  product: Product;
}

function ProductPreviewCard({ product }: ProductPreviewCardProps) {
  const getCardStyle = (productType: string) => {
    switch (productType) {
      case 'Session':
        return 'border-blue-200 bg-blue-50';
      case 'DigitalDownload':
        return 'border-purple-200 bg-purple-50';
      case 'Email':
        return 'border-orange-200 bg-orange-50';
      case 'Link':
        return 'border-green-200 bg-green-50';
      default:
        return 'border-gray-200 bg-white';
    }
  };

  const getButtonColor = (productType: string) => {
    switch (productType) {
      case 'Session':
        return 'bg-blue-600 hover:bg-blue-700';
      case 'DigitalDownload':
        return 'bg-purple-600 hover:bg-purple-700';
      case 'Email':
        return 'bg-orange-600 hover:bg-orange-700';
      case 'Link':
        return 'bg-green-600 hover:bg-green-700';
      default:
        return 'bg-purple-600 hover:bg-purple-700';
    }
  };

  return (
    <div className={cn('rounded-xl border-2 p-4 shadow-sm', getCardStyle(product.productType))}>
      <div className="flex items-start gap-3">
        {product.thumbnail && (
          <LazyImage
            src={product.thumbnail.mainLink}
            placeholder={product.thumbnail.thumbnailLink ?? product.thumbnail.mainLink}
            alt={product.title}
            className="h-12 w-12 flex-shrink-0 rounded-lg border object-cover"
          />
        )}
        <div className="min-w-0 flex-1">
          <h3 className="text-sm leading-tight font-semibold text-gray-900">{product.title}</h3>
          {product.subtitle && <p className="mt-1 text-xs leading-tight text-gray-600">{product.subtitle}</p>}

          {/* Price and CTA */}
          <div className="mt-3 flex items-center justify-between">
            <span className="font-bold text-gray-900">${product.price}</span>
            <Button size="sm" className={cn('h-8 px-3 text-xs text-white transition-colors', getButtonColor(product.productType))}>
              {product.clickToPay}
            </Button>
          </div>

          {/* Lead Magnet Form (for Email products) */}
          {false && ( // Disabled for now since 'Email' is not in ProductType
            <div className="mt-3 space-y-2">
              <input type="text" placeholder="Your name" className="w-full rounded border border-gray-300 px-2 py-1.5 text-xs" readOnly />
              <input type="email" placeholder="Your email" className="w-full rounded border border-gray-300 px-2 py-1.5 text-xs" readOnly />
              <Button size="sm" className={cn('h-8 w-full text-xs text-white', getButtonColor(product.productType))}>
                Sign Up
              </Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
