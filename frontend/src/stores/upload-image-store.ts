import { create } from 'zustand';

type UploadImageState = {
  isUploadDialogOpen: boolean;
  setIsUploadDialogOpen: (open: boolean) => void;

  selectedImage: string | null;
  setSelectedImage: (image: string | null) => void;

  crop: any;
  setCrop: (crop: any) => void;

  croppedImageUrl: string | null;
  setCroppedImageUrl: (url: string | null) => void;

  step: 'select' | 'crop';
  setStep: (step: 'select' | 'crop') => void;

  imgRef: React.RefObject<HTMLImageElement>;
  fileInputRef: React.RefObject<HTMLInputElement>;
};

export const useUploadImageStore = create<UploadImageState>((set) => ({
  isUploadDialogOpen: false,
  setIsUploadDialogOpen: (open: boolean) => set({ isUploadDialogOpen: open }),

  selectedImage: null,
  setSelectedImage: (image: string | null) => set({ selectedImage: image }),

  crop: null,
  setCrop: (crop: any) => set({ crop }),

  croppedImageUrl: null,
  setCroppedImageUrl: (url: string | null) => set({ croppedImageUrl: url }),

  step: 'select',
  setStep: (step: 'select' | 'crop') => set({ step }),

  imgRef: { current: null },
  fileInputRef: { current: null },
}));
