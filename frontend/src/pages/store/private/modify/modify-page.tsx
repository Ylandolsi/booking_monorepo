import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import 'react-image-crop/dist/ReactCrop.css';
import { patchPostStoreSchema, storeKeys, useMyStore, type PatchPostStoreRequest, type Picture, type Product, type Store } from '@/api/stores';
import { useUploadPicture } from '@/hooks/use-upload-picture';
import { MobilePreview, ProductSection, StoreSection } from '@/pages/store';
import { UploadPictureDialog } from '@/components/ui/upload-picture-dialog';
import { ErrorComponenet, InputToCopy, LoadingState } from '@/components';

export type StoreFormData = PatchPostStoreRequest & { picture?: Picture };

export function ModifyStore() {
  const { data: store, isLoading, error } = useMyStore();
  const { croppedImageUrl, setAspectRatio, handleCloseDialog } = useUploadPicture();
  const [products, setProducts] = useState<Product[]>(store?.products || []);

  const form = useForm<StoreFormData>({
    resolver: zodResolver(patchPostStoreSchema),
    defaultValues: {
      slug: store?.slug || '',
      title: store?.title || '',
      description: store?.description || '',
      socialLinks: store?.socialLinks || [],
      file: undefined,

      // ui :
      picture: store?.picture,
    },
  });

  useEffect(() => {
    setAspectRatio(1 / 1); // Set aspect ratio to 1:1 for store profile picuture
  }, []);

  useEffect(() => {
    if (croppedImageUrl) {
      form.setValue('picture', { mainLink: croppedImageUrl || '', thumbnailLink: croppedImageUrl || '' });
    } else {
      // restore to default
      form.setValue('picture', {
        mainLink: store?.picture?.mainLink || '  ',
        thumbnailLink: store?.picture?.thumbnailLink || store?.picture?.mainLink || '',
      });
    }
  }, [croppedImageUrl, form]);

  if (isLoading) return <LoadingState type="dots" />;
  if (error || !store) return <ErrorComponenet message="Failed to load store data." title="Store Error" />;
  // const watchedValues = form.watch();

  return (
    <div className="mx-auto flex min-h-screen max-w-7xl flex-col items-center justify-around gap-12 px-4 py-8 lg:flex-row lg:items-start lg:px-6">
      <style>{`.material-symbols-outlined { font-variation-settings: "FILL" 0, "wght" 400, "GRAD" 0, "opsz" 24; }`}</style>
      <UploadPictureDialog onUpload={(file) => form.setValue('file', file)} />
      <aside className="flex w-full max-w-lg flex-col gap-6">
        <div className="flex-1 space-y-2">
          {/* Store Details Section */}
          <StoreSection form={form} handleCloseDialog={handleCloseDialog} />
          {/* Products Section */}
          <ProductSection products={products} setProducts={setProducts} store={store} />
        </div>
      </aside>
      <div className="sticky top-2 h-full w-fit">
        <PreviewUrl store={store} />
        <MobilePreview storeForm={watchedValues} productsRearranged={products} setProducts={setProducts} />
      </div>
    </div>
  );
}

const PreviewUrl = ({ store }: { store: Store }) => {
  const link = window.location.origin + '/store/' + store.slug;
  return <InputToCopy input={link || ''} className="mb-4" label={'Store Public Link'} />;
};
