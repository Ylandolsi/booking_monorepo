import { ProductType } from '../types';
import type { Product } from '../types';
import { Button } from '@/components/ui/button';

interface ProductDetailsProps {
  product: Product;
}

export const ProductDetailPage = ({ product }: ProductDetailsProps) => {
  return (
    <div className="max-w-lg mx-auto px-4 py-8">
      {/* Mobile-style product view */}
      <div className="border rounded-xl overflow-hidden bg-white shadow-lg">
        {/* Product image/header */}
        {product.thumbnailUrl ? (
          <div className="h-64 bg-cover bg-center" style={{ backgroundImage: `url(${product.thumbnailUrl})` }} />
        ) : (
          <div className="h-64 bg-gray-100 flex items-center justify-center text-gray-400">No Image</div>
        )}

        {/* Product info */}
        <div className="p-6">
          <h1 className="text-2xl font-bold">{product.title}</h1>
          {product.subtitle && <p className="text-gray-500 mt-1">{product.subtitle}</p>}

          <div className="mt-4 text-xl font-semibold">${product.price.toFixed(2)}</div>

          {product.description && (
            <div className="mt-6">
              <h2 className="text-lg font-medium mb-2">About</h2>
              <div className="prose prose-sm text-gray-600">{product.description}</div>
            </div>
          )}

          {/* Product-specific details */}
          {product.productType === ProductType.Session && (
            <div className="mt-6 bg-blue-50 p-4 rounded-lg">
              <h2 className="text-lg font-medium mb-2 text-blue-700">Session Details</h2>
              <div className="space-y-2 text-sm text-blue-700">
                <div className="flex justify-between">
                  <span>Duration:</span>
                  <span>{(product as any).duration?.minutes || 30} minutes</span>
                </div>
                {(product as any).meetingInstructions && (
                  <div className="mt-3">
                    <h3 className="font-medium mb-1">Meeting Instructions:</h3>
                    <p className="text-sm">{(product as any).meetingInstructions}</p>
                  </div>
                )}
              </div>
            </div>
          )}

          {product.productType === ProductType.DigitalDownload && (
            <div className="mt-6 bg-green-50 p-4 rounded-lg">
              <h2 className="text-lg font-medium mb-2 text-green-700">Download Details</h2>
              <div className="space-y-2 text-sm text-green-700">
                <p>After purchase, you'll receive immediate access to download this product.</p>
                {(product as any).files && (product as any).files.length > 0 && (
                  <div className="mt-3">
                    <h3 className="font-medium mb-1">Files included:</h3>
                    <ul className="list-disc pl-5">
                      {(product as any).files.map((file: any, index: number) => (
                        <li key={index}>{file.fileName || `File ${index + 1}`}</li>
                      ))}
                    </ul>
                  </div>
                )}
              </div>
            </div>
          )}
        </div>

        {/* Sticky Buy/Book button */}
        <div className="sticky bottom-0 w-full p-4 bg-white border-t">
          <Button className="w-full py-6 text-lg">{product.productType === ProductType.Session ? 'Book a Call' : 'Buy Now'}</Button>
        </div>
      </div>
    </div>
  );
};
