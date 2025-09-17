import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { ProductType } from '../types';
import type { CreateProductInput } from '../types';
import { useCreateProduct, useMyStore } from '../hooks';
import { Button } from '@/components/ui/button';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

interface ProductTypeSelectionStepProps {
  onSelect: (type: ProductType) => void;
}

const ProductTypeSelectionStep = ({ onSelect }: ProductTypeSelectionStepProps) => (
  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
    <Card className="cursor-pointer hover:bg-gray-50" onClick={() => onSelect(ProductType.Session)}>
      <CardHeader>
        <CardTitle>1:1 Call Booking</CardTitle>
      </CardHeader>
      <CardContent>
        <p>Let clients book a paid session with you.</p>
      </CardContent>
    </Card>
    <Card className="cursor-pointer hover:bg-gray-50" onClick={() => onSelect(ProductType.DigitalDownload)}>
      <CardHeader>
        <CardTitle>Digital Download</CardTitle>
      </CardHeader>
      <CardContent>
        <p>Sell files like ebooks, guides, or presets.</p>
      </CardContent>
    </Card>
  </div>
);

interface ProductDetailsFormProps {
  productType: ProductType;
  onSuccess: () => void;
}

const ProductDetailsForm = ({ productType, onSuccess }: ProductDetailsFormProps) => {
  const { data: store } = useMyStore();
  const createProductMutation = useCreateProduct(store?.id ?? 0);

  // Define type for the form data based on product type
  type FormData = {
    title: string;
    subtitle?: string;
    description?: string;
    price: number;
    clickToPay: string;
    productType: ProductType;
    duration?: number;
    bufferTime?: number;
    timeZoneId?: string;
    files?: FileList | null;
  };

  const form = useForm<FormData>({
    defaultValues: {
      productType,
      title: '',
      subtitle: '',
      description: '',
      price: 0,
      clickToPay: productType === ProductType.Session ? 'Book Now' : 'Buy Now',
      ...(productType === ProductType.Session && {
        duration: 30,
        bufferTime: 15,
        timeZoneId: 'UTC',
      }),
    },
  });

  const onSubmit = (values: FormData) => {
    // Transform the form data into the expected format
    const productData: CreateProductInput = {
      title: values.title,
      subtitle: values.subtitle,
      description: values.description,
      price: values.price,
      clickToPay: values.clickToPay,
      productType: values.productType,
    };

    // Add type-specific fields
    if (productType === ProductType.Session && values.duration && values.bufferTime && values.timeZoneId) {
      Object.assign(productData, {
        duration: values.duration,
        bufferTime: values.bufferTime,
        timeZoneId: values.timeZoneId,
      });
    } else if (productType === ProductType.DigitalDownload && values.files) {
      const filesArray: File[] = [];
      for (let i = 0; i < values.files.length; i++) {
        filesArray.push(values.files[i]);
      }
      Object.assign(productData, {
        files: filesArray,
      });
    }

    createProductMutation.mutate(productData, {
      onSuccess: () => {
        onSuccess();
      },
    });
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
        {/* Shared Fields */}
        <FormField
          name="title"
          control={form.control}
          render={({ field }) => (
            <FormItem>
              <FormLabel>Title</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          name="price"
          control={form.control}
          render={({ field }) => (
            <FormItem>
              <FormLabel>Price</FormLabel>
              <FormControl>
                <Input type="number" {...field} onChange={(e) => field.onChange(parseFloat(e.target.value))} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          name="description"
          control={form.control}
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        {/* Conditional Fields */}
        {productType === ProductType.Session && (
          <>
            <FormField
              name="duration"
              control={form.control}
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Duration (minutes)</FormLabel>
                  <FormControl>
                    <Input type="number" {...field} onChange={(e) => field.onChange(parseInt(e.target.value, 10))} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </>
        )}
        {productType === ProductType.DigitalDownload && (
          <FormField
            name="files"
            control={form.control}
            render={({ field }) => (
              <FormItem>
                <FormLabel>File</FormLabel>
                <FormControl>
                  <Input type="file" onChange={(e) => field.onChange(e.target.files)} onBlur={field.onBlur} name={field.name} ref={field.ref} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        )}

        <Button type="submit" disabled={createProductMutation.isPending}>
          {createProductMutation.isPending ? 'Creating...' : 'Create Product'}
        </Button>
      </form>
    </Form>
  );
};

export const AddProductForm = ({ onSuccess }: { onSuccess: () => void }) => {
  const [productType, setProductType] = useState<ProductType | null>(null);

  if (!productType) {
    return <ProductTypeSelectionStep onSelect={setProductType} />;
  }

  return <ProductDetailsForm productType={productType} onSuccess={onSuccess} />;
};
