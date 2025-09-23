import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog';
import { Check, Camera, X } from 'lucide-react';
import ReactCrop, { centerCrop, makeAspectCrop, type PixelCrop } from 'react-image-crop';
import 'react-image-crop/dist/ReactCrop.css';
import { useUploadImageStore } from '@/stores/upload-image-store';

export const DialogUploadPicture = ({ onUpload }: { onUpload: (file: File | undefined) => void }) => {
  const { setIsUploadDialogOpen, setStep, step, crop, fileInputRef, isUploadDialogOpen, selectedImage, croppedImageUrl, imgRef, setCrop } =
    useUploadImageStore();

  const { handleImageLoad, handleBackToSelect, handleCropComplete, handleFileSelect } = useUploadPicture();
  const getUploadedFile = async (): Promise<File | undefined> => {
    if (!croppedImageUrl) {
      console.error('No cropped image to upload');
      return;
    }

    // Convert URL to File
    const response = await fetch(croppedImageUrl);
    const blob = await response.blob();
    const file = new File([blob], 'profile-picture.jpg', { type: blob.type });
    return file;
  };
  const handleUpload = async () => {
    const file = await getUploadedFile();
    if (file) {
      onUpload(file);
    }
    handleCloseDialog();
  };

  const handleCloseDialog = () => {
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    // setSelectedImage(null);
    // setCroppedImageUrl(null);
    setCrop(undefined);
    setStep('select');
    setIsUploadDialogOpen(false);

    if (imgRef.current) {
      imgRef.current.value = '';
    }
  };

  const DialogComponent = () => {
    return (
      <Dialog open={isUploadDialogOpen} onOpenChange={setIsUploadDialogOpen}>
        <DialogContent className="sm:max-w-md">
          <DialogHeader>
            <DialogTitle>Update Profile Picture</DialogTitle>
          </DialogHeader>

          <div className="space-y-4">
            {step === 'select' && (
              <div className="space-y-4">
                <Label
                  htmlFor="profile-picture-input-dialog"
                  className="flex h-48 w-full cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-gray-300 transition-colors hover:bg-gray-50"
                >
                  <Camera className="mb-4 h-12 w-12 text-gray-400" />
                  <p className="font-medium text-gray-600">Choose a photo</p>
                  <p className="text-sm text-gray-400">PNG, JPG up to 10MB</p>
                </Label>
                <input
                  ref={fileInputRef}
                  type="file"
                  accept="image/*"
                  onChange={handleFileSelect}
                  className="hidden"
                  id="profile-picture-input-dialog"
                />
              </div>
            )}

            {step === 'crop' && selectedImage && (
              <div className="space-y-4">
                <div className="flex justify-center">
                  <div className="w-full max-w-sm">
                    <ReactCrop
                      crop={crop}
                      aspect={1}
                      onChange={(c) => setCrop(c)}
                      onComplete={handleCropComplete}
                      className="overflow-hidden rounded-lg"
                    >
                      <img
                        ref={imgRef}
                        src={selectedImage}
                        onLoad={handleImageLoad}
                        alt="Profile picture"
                        className="max-h-64 w-full object-contain"
                      />
                    </ReactCrop>
                  </div>
                </div>

                {croppedImageUrl && (
                  <div className="text-center">
                    <p className="mb-2 text-sm text-gray-600">Preview:</p>
                    <div className="flex justify-center">
                      <img src={croppedImageUrl} alt="Cropped preview" className="h-20 w-20 rounded-full border-2 border-gray-200 object-cover" />
                    </div>
                  </div>
                )}
              </div>
            )}
          </div>

          <DialogFooter className="flex justify-between">
            {step === 'select' ? (
              <Button onClick={handleCloseDialog} variant="outline" className="w-full">
                Cancel
              </Button>
            ) : (
              <div className="flex w-full gap-2">
                <Button onClick={handleBackToSelect} variant="outline">
                  <X className="mr-2 h-4 w-4" />
                  Back
                </Button>

                <Button onClick={handleUpload} disabled={!croppedImageUrl} className="flex-1">
                  <Check className="mr-2 h-4 w-4" />
                  Save
                </Button>
              </div>
            )}
          </DialogFooter>
        </DialogContent>
      </Dialog>
    );
  };

  return DialogComponent();
};

export interface uploadPictureState {
  handleImageLoad: (e: React.SyntheticEvent<HTMLImageElement>) => void;
  handleBackToSelect: () => void;
  handleCropComplete: (cropData: PixelCrop) => void;
  handleFileSelect: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const useUploadPicture = (): uploadPictureState => {
  const { setIsUploadDialogOpen, setSelectedImage, setCroppedImageUrl, setStep, selectedImage, croppedImageUrl, imgRef, setCrop } =
    useUploadImageStore();

  // Image upload functions
  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        setSelectedImage(reader.result as string);
        setCroppedImageUrl(reader.result as string);
        setStep('crop');
        setIsUploadDialogOpen(true);
      };
      reader.readAsDataURL(file);
    }
  };

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

  const handleImageLoad = (e: React.SyntheticEvent<HTMLImageElement>) => {
    const { width, height } = e.currentTarget;
    //imgRef.current = e.currentTarget;
    const newCrop = centerAspectCrop(width, height, 1);
    setCrop(newCrop);
  };

  const handleCropComplete = async (cropData: PixelCrop) => {
    if (imgRef.current && cropData.width && cropData.height) {
      try {
        const croppedImage = await getCroppedImg(imgRef.current, cropData);
        setCroppedImageUrl(croppedImage);
      } catch (error) {
        console.error('Error cropping image:', error);
      }
    }
  };

  const handleBackToSelect = () => {
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    setSelectedImage(null);
    setCroppedImageUrl(null);
    setCrop(undefined);
    setStep('select');
  };

  return {
    handleImageLoad,
    handleBackToSelect,
    handleCropComplete,
    handleFileSelect,
  };
};
