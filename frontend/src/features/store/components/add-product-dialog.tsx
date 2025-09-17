// import { useState } from 'react';
// import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
// import { Card, CardContent } from '@/components/ui/card';
// import { ProductType } from '../types';

// // ProductTypeSelectionStep component - displayed first in the workflow
// interface ProductTypeSelectionStepProps {
//   onSelect: (type: ProductType) => void;
// }

// const ProductTypeSelectionStep = ({ onSelect }: ProductTypeSelectionStepProps) => (
//   <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
//     <Card className="cursor-pointer hover:bg-gray-50" onClick={() => onSelect(ProductType.Session)}>
//       <CardContent className="p-6">
//         <h3 className="font-medium text-lg mb-2">1:1 Call Booking</h3>
//         <p>Let clients book time with you for calls or consultations.</p>
//       </CardContent>
//     </Card>
//     <Card className="cursor-pointer hover:bg-gray-50" onClick={() => onSelect(ProductType.DigitalDownload)}>
//       <CardContent className="p-6">
//         <h3 className="font-medium text-lg mb-2">Digital Download</h3>
//         <p>Sell digital files like ebooks, guides, or templates.</p>
//       </CardContent>
//     </Card>
//   </div>
// );

// // Main component props
// interface AddProductDialogProps {
//   open: boolean;
//   onOpenChange: (open: boolean) => void;
//   storeId: number;
//   storeSlug: string;
// }

// export const AddProductDialog = ({ open, onOpenChange, storeId, storeSlug }: AddProductDialogProps) => {
//   const [step, setStep] = useState(1);
//   const [selectedProductType, setSelectedProductType] = useState<string>('');

//   // Reset state when dialog opens/closes
//   const handleOpenChange = (open: boolean) => {
//     if (!open) {
//       setStep(1);
//       setSelectedProductType('');
//     }
//     onOpenChange(open);
//   };

//   // Handle product type selection
//   const handleTypeSelection = (type: string) => {
//     setSelectedProductType(type);
//     setStep(2);
//   };

//   return (
//     <Dialog open={open} onOpenChange={handleOpenChange}>
//       <DialogContent className="sm:max-w-lg">
//         <DialogHeader>
//           <DialogTitle>{step === 1 ? 'Select Product Type' : 'Add Product Details'}</DialogTitle>
//         </DialogHeader>

//         {/* Step 1: Product Type Selection */}
//         {step === 1 && <ProductTypeSelectionStep onSelect={handleTypeSelection} />}

//         {/* Step 2: Product Details Form */}
//         {step === 2 && selectedProductType && (
//           <div className="py-4">
//             {selectedProductType === ProductType.Session ? (
//               <SessionProductForm storeId={storeId} storeSlug={storeSlug} onComplete={() => handleOpenChange(false)} />
//             ) : (
//               <DigitalProductForm storeId={storeId} storeSlug={storeSlug} onComplete={() => handleOpenChange(false)} />
//             )}
//           </div>
//         )}
//       </DialogContent>
//     </Dialog>
//   );
// };
import { useState } from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { AddProductForm } from './add-product-form';
import { PlusIcon } from 'lucide-react';

interface AddProductDialogProps {
  // Optional props for flexibility in usage
  storeId?: number;
  storeSlug?: string;
  triggerButton?: React.ReactNode;
  onProductAdded?: () => void;
}

export const AddProductDialog = ({ triggerButton, onProductAdded }: AddProductDialogProps) => {
  const [open, setOpen] = useState(false);

  const handleSuccess = () => {
    setOpen(false);
    if (onProductAdded) {
      onProductAdded();
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        {triggerButton || (
          <Button variant="default" className="flex items-center gap-2">
            <PlusIcon className="h-4 w-4" />
            Add Product
          </Button>
        )}
      </DialogTrigger>
      <DialogContent className="max-w-md overflow-y-auto max-h-[85vh]">
        <DialogHeader>
          <DialogTitle>Add New Product</DialogTitle>
        </DialogHeader>
        <AddProductForm onSuccess={handleSuccess} />
      </DialogContent>
    </Dialog>
  );
};
