import { useUploadImageStore } from '@/stores/upload-image-store';
import { useLocation } from '@tanstack/react-router';
import { useEffect } from 'react';

export const useUploadPicture = () => {
  const store = useUploadImageStore();
  const location = useLocation();

  useEffect(() => {
    // when location changes :
    // reset the uploaded image
    store.resetUpload();
  }, [location]);

  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      store.selectFile(file);
    }
  };

  const handleImageLoad = (e: React.SyntheticEvent<HTMLImageElement>) => {
    const { width, height } = e.currentTarget;
    store.initializeCrop(width, height);
  };

  const handleCropComplete = (cropData: any) => {
    store.cropImage(cropData);
  };

  const handleBackToSelect = () => {
    store.resetUpload();
  };

  const handleUpload = async (onUpload: (file: File | undefined) => void) => {
    const file = await store.getCroppedFile();
    onUpload(file);
    //if (file) store.selectFile(file!);
    store.closeDialog(false);
  };

  const handleCloseDialog = () => {
    store.closeDialog(true);
  };

  return {
    // State
    ...store,

    // Event handlers
    handleFileSelect,
    handleImageLoad,
    handleCropComplete,
    handleBackToSelect,
    handleUpload,
    handleCloseDialog,
  };
};
