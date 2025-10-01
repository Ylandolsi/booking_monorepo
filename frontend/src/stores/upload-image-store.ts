import { create } from 'zustand';
import { centerCrop, makeAspectCrop, type PixelCrop } from 'react-image-crop';

type UploadImageState = {
  // Dialog state
  isUploadDialogOpen: boolean;

  // Image processing state
  selectedImage: string | null;
  croppedImageUrl: string | null;
  crop: PixelCrop | undefined;
  step: 'select' | 'crop';
  aspectRatio: number; // Added aspect ratio to state
  isAspectRatioLocked: boolean; // Added to control if aspect ratio is locked

  // Refs
  imgRef: React.RefObject<HTMLImageElement>; // image displayed for cropping
  fileInputRef: React.RefObject<HTMLInputElement>; // file (image) uploaded by the user
};

type UploadImageActions = {
  // Dialog actions
  openDialog: () => void;
  closeDialog: (cancel: boolean) => void;

  // Image processing actions
  setSelectedImage: (image: string | null) => void;
  setCroppedImageUrl: (url: string | null) => void;
  setCrop: (crop: PixelCrop | undefined) => void;
  setStep: (step: 'select' | 'crop') => void;
  setAspectRatio: (ratio: number) => void; // Added action to change aspect ratio
  setAspectRatioLocked: (locked: boolean) => void; // Added action to lock/unlock aspect ratio

  // Business logic actions
  selectFile: (file: File) => void;
  cropImage: (cropData: PixelCrop) => Promise<void>;
  resetUpload: () => void;
  getCroppedFile: () => Promise<File | undefined>;
  initializeCrop: (width: number, height: number) => void;
  updateCropForNewAspectRatio: (width: number, height: number) => void; // New action for aspect ratio changes
};

type UploadImageStore = UploadImageState & UploadImageActions;

const initialState: UploadImageState = {
  isUploadDialogOpen: false,
  selectedImage: null,
  croppedImageUrl: null,
  crop: undefined,
  step: 'select',
  aspectRatio: 1, // Default to square (1:1)
  isAspectRatioLocked: false, // Default to unlocked
  imgRef: { current: null },
  fileInputRef: { current: null },
};

export const useUploadImageStore = create<UploadImageStore>((set, get) => ({
  ...initialState,

  // Dialog actions
  openDialog: () => set({ isUploadDialogOpen: true }),
  closeDialog: (cancel: boolean = true) => {
    const { selectedImage, croppedImageUrl } = get();

    // Clean up object URLs // TODO review this
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (cancel && croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    // Reset to initial state
    set({
      ...initialState,
      croppedImageUrl: cancel ? null : croppedImageUrl, // Keep cropped image if not cancelling
      selectedImage: null, // Always reset selected image
    });
  },

  // Image processing actions
  setSelectedImage: (image) => set({ selectedImage: image }),
  setCroppedImageUrl: (url) => set({ croppedImageUrl: url }),
  setCrop: (crop) => set({ crop }),
  setStep: (step) => set({ step }),
  setAspectRatio: (ratio) => set({ aspectRatio: ratio }),
  setAspectRatioLocked: (locked) => set({ isAspectRatioLocked: locked }),

  // Business logic actions
  selectFile: (file) => {
    const reader = new FileReader();
    reader.onload = () => {
      const result = reader.result as string;
      set({
        selectedImage: result,
        croppedImageUrl: result,
        step: 'crop',
        isUploadDialogOpen: true,
      });
    };
    reader.readAsDataURL(file);
  },

  cropImage: async (cropData) => {
    const { imgRef } = get();
    if (!imgRef.current || !cropData.width || !cropData.height) return;

    try {
      const croppedImageUrl = await getCroppedImg(imgRef.current, cropData);
      set({ croppedImageUrl });
    } catch (error) {
      console.error('Error cropping image:', error);
    }
  },

  resetUpload: () => {
    const { selectedImage, croppedImageUrl, aspectRatio } = get();

    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    set({
      selectedImage: null,
      croppedImageUrl: null,
      crop: undefined,
      step: 'select',
      // Don't reset aspectRatio here - preserve user's choice
    });
  },

  getCroppedFile: async () => {
    const { croppedImageUrl } = get();
    if (!croppedImageUrl) {
      console.error('No cropped image to upload');
      return;
    }

    const response = await fetch(croppedImageUrl);
    const blob = await response.blob();
    return new File([blob], 'profile-picture.jpg', { type: blob.type });
  },

  initializeCrop: (width: number, height: number) => {
    const { aspectRatio } = get(); // Use the current aspect ratio from state
    const percentCrop = centerAspectCrop(width, height, aspectRatio);
    // Convert percent crop to pixel crop
    const pixelCrop: PixelCrop = {
      unit: 'px',
      x: (percentCrop.x / 100) * width,
      y: (percentCrop.y / 100) * height,
      width: (percentCrop.width / 100) * width,
      height: (percentCrop.height / 100) * height,
    };
    set({ crop: pixelCrop });
  },

  // New action to update crop when aspect ratio changes
  updateCropForNewAspectRatio: (width: number, height: number) => {
    const { aspectRatio } = get();
    const percentCrop = centerAspectCrop(width, height, aspectRatio);
    const pixelCrop: PixelCrop = {
      unit: 'px',
      x: (percentCrop.x / 100) * width,
      y: (percentCrop.y / 100) * height,
      width: (percentCrop.width / 100) * width,
      height: (percentCrop.height / 100) * height,
    };
    set({ crop: pixelCrop });
  },
}));

// Helper function for cropping
const getCroppedImg = (image: HTMLImageElement, cropData: PixelCrop): Promise<string> => {
  const canvas = document.createElement('canvas');
  const scaleX = image.naturalWidth / image.width;
  const scaleY = image.naturalHeight / image.height;

  canvas.width = cropData.width;
  canvas.height = cropData.height;
  const ctx = canvas.getContext('2d');

  if (!ctx) {
    throw new Error('No 2d context');
  }

  ctx.drawImage(
    image,
    cropData.x * scaleX,
    cropData.y * scaleY,
    cropData.width * scaleX,
    cropData.height * scaleY,
    0,
    0,
    cropData.width,
    cropData.height,
  );

  return new Promise<string>((resolve) => {
    canvas.toBlob(
      (blob) => {
        if (!blob) {
          console.error('Canvas is empty');
          return;
        }
        resolve(URL.createObjectURL(blob));
      },
      'image/jpeg',
      0.9,
    );
  });
};

// Helper function for centering crop
const centerAspectCrop = (mediaWidth: number, mediaHeight: number, aspect: number) => {
  return centerCrop(
    makeAspectCrop(
      {
        unit: '%',
        width: 80,
      },
      aspect,
      mediaWidth,
      mediaHeight,
    ),
    mediaWidth,
    mediaHeight,
  );
};
