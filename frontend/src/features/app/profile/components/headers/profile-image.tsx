import React, { useRef, useState } from 'react';
import { centerCrop, makeAspectCrop, type PixelCrop } from 'react-image-crop';
import { UploadPictureDialog, useProfileBySlug } from '@/features/app/profile';

import { Edit2 } from 'lucide-react';
import { useUpdateProfilePicture } from '@/features/app/profile/api';
import 'react-image-crop/dist/ReactCrop.css';
import { cn, LazyImage } from '@/utils';

interface ProfileImageProps extends React.HTMLAttributes<HTMLDivElement> {
  className?: string;
  size?: 'sm' | 'lg';
}

export const ProfileImage = ({
  className = '',
  size = 'lg',
  ...props
}: ProfileImageProps) => {
  const sizeClasses = {
    sm: 'w-24 h-24',
    lg: 'w-33 h-33',
  };

  const { user, isSlugCurrent } = useProfileBySlug();

  const [isOpen, setIsOpen] = useState(false);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);
  const [crop, setCrop] = useState<any>();
  const [croppedImageUrl, setCroppedImageUrl] = useState<string | null>(null);
  const [step, setStep] = useState<'select' | 'crop'>('select');

  const imgRef = useRef<HTMLImageElement>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const updateProfilePictureMutation = useUpdateProfilePicture();

  // Convert cropped image URL to File object
  const urlToFile = async (url: string, filename: string): Promise<File> => {
    const response = await fetch(url);
    const blob = await response.blob();
    return new File([blob], filename, { type: blob.type });
  };

  // Create FormData from cropped image
  const createFormData = async (croppedUrl: string): Promise<FormData> => {
    const file = await urlToFile(croppedUrl, 'profile-picture.jpg');
    const formData = new FormData();
    formData.append('file', file);
    return formData;
  };

  // Handle file selection
  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        setSelectedImage(reader.result as string);
        setCroppedImageUrl(null);
        setStep('crop');
      };
      reader.readAsDataURL(file);
    }
  };

  // Center aspect crop utility
  const centerAspectCrop = (
    mediaWidth: number,
    mediaHeight: number,
    aspect: number,
  ) => {
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

  // Generate cropped image
  const getCroppedImg = (
    image: HTMLImageElement,
    cropData: PixelCrop,
  ): Promise<string> => {
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

  // Handle image load
  const handleImageLoad = (e: React.SyntheticEvent<HTMLImageElement>) => {
    const { width, height } = e.currentTarget;
    imgRef.current = e.currentTarget;
    const newCrop = centerAspectCrop(width, height, 1);
    setCrop(newCrop);
  };

  // Handle crop complete
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

  // Upload cropped image
  const handleUpload = async () => {
    if (!croppedImageUrl) {
      console.error('No cropped image to upload');
      return;
    }

    try {
      const formData = await createFormData(croppedImageUrl);
      await updateProfilePictureMutation.mutateAsync({ data: formData });

      // Clean up and close dialog
      handleCloseDialog();
    } catch (error) {
      console.error('Error uploading profile picture:', error);
    }
  };

  // reset dialog state and clean up
  const handleCloseDialog = () => {
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    setSelectedImage(null);
    setCroppedImageUrl(null);
    setCrop(undefined);
    setStep('select');
    setIsOpen(false);

    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  // Go back to file selection
  const handleBackToSelect = () => {
    if (selectedImage) URL.revokeObjectURL(selectedImage);
    if (croppedImageUrl) URL.revokeObjectURL(croppedImageUrl);

    setSelectedImage(null);
    setCroppedImageUrl(null);
    setCrop(undefined);
    setStep('select');
  };

  return (
    <>
      {/* Profile Picture with Edit Icon */}
      <div className={`group relative inline-block ${className}`}>
        <div className="relative">
          <LazyImage
            src={
              user?.profilePicture.profilePictureLink ||
              'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
            }
            alt="profile-picture"
            placeholder={
              user?.profilePicture.thumbnailUrlPictureLink ||
              'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
            }
            className={cn(
              'rounded-full ring-4 ring-primary/20 object-cover transition-all group-hover:ring-primary/40 shadow-lg',
              sizeClasses[size],
            )}
          />
          {isSlugCurrent && (
            <button
              onClick={() => setIsOpen(true)}
              className="absolute -top-2 -right-2 bg-primary hover:bg-primary/90 text-white p-2 rounded-full shadow-lg transition-colors"
              title="Edit profile picture"
            >
              <Edit2 className="w-4 h-4" />
            </button>
          )}
        </div>
      </div>
      <UploadPictureDialog
        crop={crop}
        setCrop={setCrop}
        setIsOpen={setIsOpen}
        step={step}
        fileInputRef={fileInputRef}
        handleFileSelect={handleFileSelect}
        croppedImageUrl={croppedImageUrl}
        selectedImage={selectedImage}
        handleCropComplete={handleCropComplete}
        handleImageLoad={handleImageLoad}
        handleBackToSelect={handleBackToSelect}
        updateProfilePictureMutation={updateProfilePictureMutation}
        handleCloseDialog={handleCloseDialog}
        handleUpload={handleUpload}
        imgRef={imgRef}
        isOpen={isOpen}
      />
    </>
  );
};
