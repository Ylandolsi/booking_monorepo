import { useState, useEffect } from 'react';
import { Avatar } from '@/components/ui/avatar';
import { cn } from '@/lib/cn';

interface MobilePreviewProps {
  formData: {
    title?: string;
    subtitle?: string;
    price?: number;
    thumbnailUrl?: string;
    description?: string;
    productType?: string;
    clickToPay?: string;
  };
  storeInfo?: {
    name: string;
    slug: string;
    profileImage?: string;
    products?: any[];
  };
}

export const MobilePreview = ({ formData, storeInfo }: MobilePreviewProps) => {
  const [previewImage, setPreviewImage] = useState<string | undefined>(undefined);

  // Set the preview image
  useEffect(() => {
    if (formData.thumbnailUrl) {
      setPreviewImage(formData.thumbnailUrl);
    }
  }, [formData.thumbnailUrl]);

  return (
    <div className="max-w-sm mx-auto">
      {/* Mobile device frame */}
      <div className="border-8 border-gray-800 rounded-[40px] overflow-hidden bg-white shadow-xl">
        {/* Status bar */}
        <div className="bg-gray-800 text-white p-2 flex justify-between items-center text-xs">
          <span>9:41 AM</span>
          <div className="flex items-center space-x-1">
            <svg xmlns="http://www.w3.org/2000/svg" className="h-3 w-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 12h14M12 5l7 7-7 7" />
            </svg>
            <svg xmlns="http://www.w3.org/2000/svg" className="h-3 w-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
            </svg>
            <svg xmlns="http://www.w3.org/2000/svg" className="h-3 w-3" fill="currentColor" viewBox="0 0 24 24">
              <path d="M0 0h24v24H0z" fill="none" />
              <path d="M12.01 21.49L23.64 7c-.45-.34-4.93-4-11.64-4C5.28 3 .81 6.66.36 7l11.63 14.49.01.01.01-.01z" />
            </svg>
          </div>
        </div>

        {/* Content */}
        <div className="h-[500px] overflow-y-auto">
          {/* Store header */}
          <div className="p-4 border-b text-center">
            <Avatar className="mx-auto w-20 h-20 mb-2">
              <div className="bg-gray-200 w-full h-full flex items-center justify-center text-2xl font-bold text-gray-500">
                {storeInfo?.name?.charAt(0) || 'L'}
              </div>
            </Avatar>
            <h1 className="text-xl font-bold">{storeInfo?.name || 'Linki Store'}</h1>
            <p className="text-gray-500 text-sm">@{storeInfo?.slug || 'your-store'}</p>
          </div>

          {/* Product card */}
          <div className="p-4">
            <div className="border rounded-lg overflow-hidden shadow-sm">
              {previewImage ? (
                <div className="h-40 bg-cover bg-center" style={{ backgroundImage: `url(${previewImage})` }} />
              ) : (
                <div className="h-40 bg-gray-100 flex items-center justify-center text-gray-400">Product Image</div>
              )}

              <div className="p-4">
                <h2 className="font-bold text-lg">{formData.title || 'Product Title'}</h2>
                {formData.subtitle && <p className="text-sm text-gray-500">{formData.subtitle}</p>}
                <div className="mt-2 flex justify-between items-center">
                  <span className="font-medium">${formData.price?.toFixed(2) || '0.00'}</span>
                  <button
                    className={cn(
                      'px-4 py-2 rounded text-sm font-medium',
                      formData.productType === 'Session' ? 'bg-blue-600 text-white' : 'bg-green-600 text-white',
                    )}
                  >
                    {formData.clickToPay || (formData.productType === 'Session' ? 'Book Now' : 'Buy Now')}
                  </button>
                </div>
              </div>
            </div>

            {/* Description preview */}
            {formData.description && (
              <div className="mt-4 p-4 bg-gray-50 rounded-lg">
                <h3 className="font-medium mb-2">Description</h3>
                <p className="text-sm text-gray-600">{formData.description}</p>
              </div>
            )}
          </div>
        </div>

        {/* Home button */}
        <div className="bg-gray-800 p-1 flex justify-center">
          <div className="w-16 h-1 bg-gray-400 rounded-full"></div>
        </div>
      </div>
      <p className="text-center text-sm text-gray-500 mt-2">Live preview updates as you edit</p>
    </div>
  );
};
