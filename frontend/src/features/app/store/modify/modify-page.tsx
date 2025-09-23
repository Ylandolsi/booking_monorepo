import { initialStore, initProducts, useMyStore, type Product } from '@/api/stores';
import { Button, ErrorComponenet, LoadingState } from '@/components';
import { EnhancedStorefrontDashboard } from '@/components/store';
import { socialPlatforms } from '@/features/app/store/create';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import { LazyImage } from '@/utils';
import { useState } from 'react';

export function ModifyStore() {
  let { data: store, isLoading, isError } = useMyStore();
  store = initialStore; // for testing
  isError = false;
  isLoading = false;
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  const [products, setProducts] = useState<Product[]>(initProducts);

  if (isLoading) return <LoadingState type="spinner" />;
  // TODO : uncomment it if (!store || isError) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;

  const handleProductClick = (product: Product) => {
    setSelectedProduct(product);
  };

  const handleProductEdit = (product: Product) => {
    setSelectedProduct(product);
  };

  // const handleProductDelete = (product: Product) => {
  //   if (confirm(`Delete "${product.title}"?`)) {
  //     setProducts((prev) => prev.filter((p) => p.id !== product.id));
  //   }
  // };

  const handleProductsReorder = (reorderedProducts: Product[]) => {
    setProducts(reorderedProducts);
  };

  const handleAddProduct = () => {
    // setCurrentView('product-creation');
  };

  const handleProductCreated = (productData: any) => {
    const newProduct: Product = {
      ...productData,
      id: `new-${Date.now()}`,
      price: productData.price.startsWith('$') ? productData.price : `$${productData.price}`,
    };
    setProducts((prev) => [newProduct, ...prev]);
  };

  const handleProductUpdated = (updatedProduct: Product) => {
    // setProducts((prev: Product[]) => prev.map((p) => (p.id === updatedProduct.id ? updatedProduct : p)));
  };

  const handleStoreSetup = (storeData: any) => {
    console.log('Store created:', storeData);
  };
  return (
    <div className="flex min-h-screen flex-col items-center justify-around gap-10 lg:flex-row lg:items-start">
      <div className="flex flex-col gap-5">
        <div className="text-card-foreground rounded-xl border px-3 py-3 shadow-sm">
          <div className="flex gap-3">
            {/* // <img src={store?.picture?.mainLink ?? FALLBACK_PROFILE_PICTURE} alt="Store Profile Picture" /> */}
            <LazyImage
              src={store?.picture?.mainLink ?? FALLBACK_PROFILE_PICTURE}
              placeholder={store?.picture?.thumbnailLink ?? FALLBACK_PROFILE_PICTURE}
              alt="Store Profile Picture"
              className="border-border h-24 w-24 rounded-2xl border object-cover"
            />
            <div className="flex flex-1 flex-col items-start justify-center gap-2">
              <div>
                <h2 className="text-primary text-xl font-bold tracking-wider" style={{ letterSpacing: '0.0.3em' }}>
                  {store.title || 'Your Store Name'}
                </h2>
                <p className="text-primary/80">@{store.slug}</p>
              </div>
              <p className="text-muted-foreground line-clamp-3 text-sm leading-relaxed text-wrap break-words">
                {store.description || 'Your store description will appear here...'}
              </p>
              {store.socialLinks && store.socialLinks.length > 0 && (
                <div className="flex justify-center gap-4">
                  {socialPlatforms.map(
                    ({ key, icon: Icon }) =>
                      store.socialLinks?.find((link: any) => link.platform === key)?.url && (
                        <a
                          key={key}
                          href={store.socialLinks?.find((link: any) => link.platform === key)?.url}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-primary hover:text-foreground transition-colors"
                        >
                          <div className="bg-primary/10 flex h-9 w-9 items-center justify-center rounded-xl">
                            <Icon className="h-5 w-5" />
                          </div>
                        </a>
                      ),
                  )}
                </div>
              )}
            </div>
          </div>
        </div>
        <div className="space-y-3">
          {products.map((product) => {
            return (
              <div key={product.productSlug} className="bg-card text-card-foreground flex rounded-xl border px-3 py-3 shadow-sm">
                <LazyImage
                  src={product?.thumbnail?.mainLink ?? FALLBACK_PROFILE_PICTURE}
                  placeholder={product?.thumbnail?.thumbnailLink ?? FALLBACK_PROFILE_PICTURE}
                  alt="Store Profile Picture"
                  className="border-border h-15 w-15 rounded-2xl border object-cover"
                />
                <div className="ml-4 flex flex-1 flex-col justify-center">
                  <h3 className="text-foreground text-lg font-stretch-90%">{product.title}</h3>
                  {product.subtitle && <p className="text-muted-foreground text-sm">{product.subtitle}</p>}
                  <p className="text-primary mt-2 text-xl font-bold">{product.price}</p>
                </div>
              </div>
            );
          })}
        </div>
        <Button
          className="bg-primary text-primary-foreground mt-4 rounded-lg px-6 py-3 font-semibold transition-opacity hover:opacity-90"
          onClick={handleAddProduct}
        >
          <span className="text-xl">+</span>
          <span className="font-medium">Add Product</span>{' '}
        </Button>
      </div>
      <div className="sticky top-4">
        <EnhancedStorefrontDashboard
          products={products}
          onAddProduct={handleAddProduct}
          onProductClick={handleProductClick}
          onProductEdit={handleProductEdit}
          // onProductDelete={handleProductDelete}
          onProductsReorder={handleProductsReorder}
          isOwner={true}
        />
      </div>
    </div>
  );
}
