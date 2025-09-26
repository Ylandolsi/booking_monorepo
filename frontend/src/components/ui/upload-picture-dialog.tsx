import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog';
import { Check, Camera, X } from 'lucide-react';
import ReactCrop from 'react-image-crop';
import 'react-image-crop/dist/ReactCrop.css';
import { useUploadPicture } from '@/hooks/use-upload-picture';

interface UploadPictureDialogProps {
  onUpload: (file: File | undefined) => void;
}

export const UploadPictureDialog = ({ onUpload }: UploadPictureDialogProps) => {
  const {
    isUploadDialogOpen,
    step,
    crop,
    fileInputRef,
    selectedImage,
    croppedImageUrl,
    imgRef,
    setCrop,
    handleFileSelect,
    handleImageLoad,
    handleCropComplete,
    handleBackToSelect,
    handleUpload,
    handleCloseDialog,
    isAspectRatioLocked,
    aspectRatio,
  } = useUploadPicture();

  return (
    <Dialog open={isUploadDialogOpen} onOpenChange={handleCloseDialog}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>{step === 'select' ? 'Upload Cover Image' : 'Crop Your Image'}</DialogTitle>
          {step === 'crop' && <p className="text-sm text-gray-600">Adjust the selection to choose which part of your image to use</p>}
        </DialogHeader>

        <div className="space-y-4">
          {step === 'select' && (
            <div className="space-y-4">
              <Label
                htmlFor="profile-picture-input-dialog"
                className="flex h-48 w-full cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-gray-300 transition-colors hover:bg-gray-50"
              >
                <Camera className="mb-4 h-12 w-12 text-gray-400" />
                <div className="space-y-1 text-center">
                  <p className="font-medium text-gray-700">Choose your cover image</p>
                  {/* <p className="text-sm text-gray-500">Best results with 1600×900 pixels</p> */}
                  <p className="text-xs text-gray-400">PNG, JPG up to 5MB</p>
                </div>
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
                    onChange={setCrop}
                    onComplete={handleCropComplete}
                    className="overflow-hidden rounded-lg"
                    aspect={isAspectRatioLocked ? aspectRatio : undefined} // Lock the ratio
                    locked={isAspectRatioLocked} // Prevent manual ratio changes
                  >
                    <img ref={imgRef} src={selectedImage} alt="Crop preview" onLoad={handleImageLoad} />
                  </ReactCrop>
                </div>
              </div>

              {/* {croppedImageUrl && (
                <div className="text-center">
                  <p className="mb-2 text-sm text-gray-600">Preview:</p>
                  <div className="flex justify-center">
                    <img src={croppedImageUrl} alt="Cropped preview" className="h-20 w-20 rounded-full border-2 border-gray-200 object-cover" />
                  </div>
                </div>
              )} */}
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

              <Button onClick={() => handleUpload(onUpload)} disabled={!croppedImageUrl} className="flex-1">
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
