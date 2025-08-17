import { DialogFooter, Button, Label, DrawerDialog } from '@/components/ui';
import { Camera, Check, X } from 'lucide-react';
import ReactCrop from 'react-image-crop';

type UploadPictureDialogProps = {
  isOpen: boolean;
  setIsOpen: (open: boolean) => void;
  step: 'select' | 'crop';
  fileInputRef: React.RefObject<HTMLInputElement>;
  handleFileSelect: (e: React.ChangeEvent<HTMLInputElement>) => void;
  croppedImageUrl: string | null;
  selectedImage: string | null;
  handleCropComplete: (crop: any) => void;
  handleImageLoad: (e: React.SyntheticEvent<HTMLImageElement>) => void;
  handleBackToSelect: () => void;
  updateProfilePictureMutation: { isPending: boolean };
  handleCloseDialog: () => void;
  handleUpload: () => void;
  imgRef: React.RefObject<HTMLImageElement>;
  crop: any;
  setCrop: (crop: any) => void;
};

export function UploadPictureDialog({
  isOpen,
  setIsOpen,
  step,
  fileInputRef,
  handleFileSelect,
  croppedImageUrl,
  selectedImage,
  handleCropComplete,
  handleImageLoad,
  handleBackToSelect,
  updateProfilePictureMutation,
  handleCloseDialog,
  handleUpload,
  imgRef,
  crop,
  setCrop,
}: UploadPictureDialogProps) {
  return (
    <DrawerDialog
      open={isOpen}
      onOpenChange={setIsOpen}
      trigger={null}
      title="Update Profile Picture"
      description=""
    >
      {/* <Dialog open={isOpen} onOpenChange={setIsOpen}>
       <DialogContent className="sm:max-w-md">
         <DialogHeader>
           <DialogTitle>Update Profile Picture</DialogTitle>
         </DialogHeader> */}

      <div className="space-y-4">
        {step === 'select' && (
          <div className="space-y-4">
            <input
              ref={fileInputRef}
              type="file"
              accept="image/*"
              onChange={handleFileSelect}
              className="hidden"
              id="profile-picture-input"
            />

            <Label
              htmlFor="profile-picture-input"
              className="flex flex-col items-center justify-center w-full h-48 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer hover:bg-gray-50 transition-colors"
            >
              <Camera className="w-12 h-12 text-gray-400 mb-4" />
              <p className="text-gray-600 font-medium">Choose a photo</p>
              <p className="text-sm text-gray-400">PNG, JPG up to 10MB</p>
            </Label>
          </div>
        )}

        {step === 'crop' && selectedImage && (
          <div className="space-y-4">
            {/* Cropping Interface */}
            <div className="flex justify-center">
              <div className="w-full max-w-sm">
                <ReactCrop
                  crop={crop}
                  aspect={1}
                  onChange={(c) => setCrop(c)}
                  onComplete={handleCropComplete}
                  className="rounded-lg overflow-hidden"
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

            {/* Preview */}
            {croppedImageUrl && (
              <div className="text-center">
                <p className="mb-2 text-sm text-gray-600">Preview:</p>
                <div className="flex justify-center">
                  <img
                    src={croppedImageUrl}
                    alt="Cropped preview"
                    className="w-20 h-20 rounded-full object-cover border-2 border-gray-200"
                  />
                </div>
              </div>
            )}
          </div>
        )}
      </div>

      <DialogFooter className="flex justify-between">
        {step === 'select' ? (
          <Button
            onClick={handleCloseDialog}
            variant="outline"
            className="w-full"
          >
            Cancel
          </Button>
        ) : (
          <div className="flex gap-2 w-full mt-2">
            <Button
              onClick={handleBackToSelect}
              variant="outline"
              disabled={updateProfilePictureMutation.isPending}
            >
              <X className="w-4 h-4 mr-2" />
              Back
            </Button>

            <Button
              onClick={handleUpload}
              disabled={
                !croppedImageUrl || updateProfilePictureMutation.isPending
              }
              className="flex-1 "
            >
              {updateProfilePictureMutation.isPending ? (
                <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin mr-2" />
              ) : (
                <Check className="w-4 h-4 mr-2" />
              )}
              {updateProfilePictureMutation.isPending ? 'Uploading...' : 'Save'}
            </Button>
          </div>
        )}
      </DialogFooter>
      {/* </DialogContent>
    </Dialog> */}
    </DrawerDialog>
  );
}
