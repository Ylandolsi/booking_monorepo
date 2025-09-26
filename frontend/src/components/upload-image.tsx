import { Label } from '@/components/ui/label';
import { useUploadPicture } from '@/hooks';
import { Camera, Upload } from 'lucide-react';

export const UploadImage = ({ description }: { description: string }) => {
  const { openDialog } = useUploadPicture();

  return (
    <div className="space-y-2">
      <Label className="text-foreground flex items-center gap-2">
        <Upload className="h-4 w-4" />
        {description}
      </Label>
      <div className="flex items-center gap-4" onClick={openDialog}>
        <Label
          htmlFor="profile-picture-input"
          className="flex h-12 w-full cursor-pointer items-center justify-center rounded-lg border-2 border-dashed border-gray-300 transition-colors hover:bg-gray-50"
        >
          <Camera className="mr-2 h-6 w-6 text-gray-400" />
          <span className="font-medium text-gray-600">Choose a photo</span>
        </Label>
      </div>
      <p className="text-muted-foreground text-xs">PNG, JPG up to 10MB</p>
    </div>
  );
};
