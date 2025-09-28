import { BookingPage } from '@/features/app/store/products/components/checkout/book';
import type { Product } from '@/api/stores';
import { FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL } from '@/assets';
import { Form, FormControl, FormField, FormItem, FormLabel, Input, Link } from '@/components/ui';
import { routes } from '@/config';
import { X } from 'lucide-react';
import z from 'zod';
import { createProductSchema } from '@/api/stores/produtcs/sessions';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Label } from '@radix-ui/react-label';
import { useAppNavigation } from '@/hooks';

// TODO : needs to be generic to handle both form / api data

// export type ProductCheckoutType = Pick<Product, 'thumbnailPicture' | 'description' | 'title' | 'subtitle' | 'price' | 'clickToPay' | 'productType'>;

export function CheckoutPageProduct({ productData }: { productData: Product }) {
  return (
    <div className="flex w-full flex-col items-start justify-center gap-5">
      <ProductCheckout product={productData}>{productData.productType == 'Session' && <BookingPage product={productData} />}</ProductCheckout>
    </div>
  );
}

export const COVER_IMAGE = {
  width: 100, // rem
  height: 80, // rem
};

const userDataBookFromSchema = z.object({
  fullName: z.string().min(2, 'Full name must be at least 2 characters').max(100, 'Full name must be at most 100 characters'),
  email: z.string().email('Invalid email address'),
  phone: z.string().min(10, 'Phone number must be at least 10 digits').max(15, 'Phone number must be at most 15 digits').optional().or(z.literal('')),
});

type userDataBookInput = z.infer<typeof userDataBookFromSchema>;

export function ProductCheckout({ product, children }: { product: Product; children?: React.ReactNode }) {
  const form = useForm<userDataBookInput>({
    resolver: zodResolver(userDataBookFromSchema),
    defaultValues: {
      fullName: '',
      email: '',
      phone: '',
    },
  });

  return (
    <div className="bg-background-light dark:bg-background-dark relative text-slate-800 dark:text-slate-200">
      <header className="absolute top-0 right-0 left-0 z-10 flex items-center justify-between p-4">
        {/* todo change this to home page for the user  */}
        <Link className="flex items-center gap-2" to={routes.to.store.publicStorePreview({ storeSlug: product.storeSlug }) as string | undefined}>
          <X />
          <span className="font-bold text-slate-900 dark:text-white">Back to Store</span>
        </Link>
      </header>
      <main className="w-full pt-16 pb-28 break-words">
        <div className="bg-accent h-${COVER_IMAGE.height} w-${COVER_IMAGE.width} flex items-center justify-center">
          {/* Change to lazy Image */}
          <img
            alt="Product Image"
            className="h-full w-full object-cover"
            src={product.thumbnailPicture?.mainLink ?? FALLBACK_SESSION_PRODUCT_PICTURE_THUMBNAIL}
          />
        </div>
        <div className="flex flex-col overflow-x-hidden p-6">
          <h1 className="line-clamp-2 text-3xl font-extrabold break-all text-slate-900 dark:text-white">{product.title}</h1>
          <p className="mt-2 line-clamp-3 text-lg break-all text-slate-600 dark:text-slate-400">{product.subtitle}</p>
          <div className="mt-4">
            <span className="text-primary text-4xl font-bold">{product.price?.toFixed(2) || 0}$</span>
          </div>
          <div className="mt-6 mb-10 max-h-40 space-y-4 overflow-y-auto text-slate-700 dark:text-slate-300">
            <h2 className="text-xl font-bold break-all text-slate-900 dark:text-white">Description</h2>
            <p className="break-all">{product.description}</p>
          </div>

          {children}

          <Form {...form}>
            <form>
              <div className="mt-10 flex w-full flex-col gap-5">
                <FormField
                  control={form.control}
                  name="fullName"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground">Full Name</FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="Full Name" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
                <FormField
                  control={form.control}
                  name="phone"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground">Phone Number</FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="Phone Number" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground">Email Address</FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="falten@gmail.com" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
              </div>
              <button
                onClick={() => alert('Purchase completed!')}
                className="bg-primary shadow-primary/30 hover:bg-opacity-90 mt-10 h-14 w-full rounded-xl text-lg font-bold text-white shadow-lg transition-all duration-300"
              >
                Buy Now
              </button>
            </form>
          </Form>
        </div>
      </main>
    </div>
  );
}

export function UserDataForm() {}
