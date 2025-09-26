import { useMyStore } from '@/api/stores';
import { ErrorComponenet, LoadingState, MobileContainer, ProductCard, StoreHeader } from '@/components';
import { GenerateIdCrypto } from '@/lib';

export const MobilePreview = () => {
  const { data: store, isLoading, isError } = useMyStore();
  if (isLoading) return <LoadingState type="spinner" />;
  if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  return (
    <main className="flex flex-1 items-center justify-center">
      <MobileContainer>
        <StoreHeader store={store} />
        <div className={'w-full space-y-4'}>
          {store.products.map((product, index) => (
            <div
              key={GenerateIdCrypto()}
              // for future drag and drop
              // className={cn('relative transition-all duration-200', 'scale-95 opacity-50', 'translate-y-1 transform', 'cursor-move')}
            >
              <div className={'group px-6'}>
                <ProductCard product={product} />
              </div>
            </div>
          ))}
        </div>
      </MobileContainer>
    </main>
  );
};
