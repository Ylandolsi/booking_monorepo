import type { Product } from '@/api/stores';
import { MobileContainer } from '@/components';
import { createFileRoute, Link } from '@tanstack/react-router';
import { X } from 'lucide-react';

export const Route = createFileRoute('/app/store/checkout')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="justify- mx-auto flex min-h-screen flex-col items-center">
      <MobileContainer>
        <ProductCheckout
          product={{
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString(),
            clickToPay: 'Buy now',
            isPublished: true,
            displayOrder: 1,
            productType: 'Session',
            storeSlug: 'sophia-carter-store',
            productSlug: 'ultimate-guide-design',
            title: 'The Ultimate Guide to Design',
            subtitle: 'Master the art of modern UI/UX',
            description:
              "This is a detailed description of the product, highlighting its features, benefits, and any other relevant information that would help a customer make a purchase decision. It's designed to be comprehensive yet easy to read. Dive deep into principles of visual hierarchy, color theory, and interaction design.",
            price: 49.0,

            thumbnailPicture: {
              mainLink:
                'https://lh3.googleusercontent.com/aida-public/AB6AXuA7k7Y1yTUsoGw4kHQ-au2cb85NtWAxnzb8R4sAvMlM73WrWUrUhuzqa4i7fvFJkbZWWSamtgVPoY-LtYj2gEnfzCm_dM2sMbL8X7I9DyaGB-5lL6f2r-p6sHYczA-ID-kqquaLSMCTtOhr2ykI8_fET6STytxU4CjxNKoOZN_VDA-lcAdOBxwmj8rzTr0mciZU8q_VMQfWb5WYSZO7zwbSrPShrVxxD30-i6ZZsBXgQjO5wAHROE-LgHbSv_-CIKLF4K2o7rbJB-Zs',
              thumbnailLink: '',
            },
          }}
        />
      </MobileContainer>
    </div>
  );
}

export function ProductCheckout({ product, children }: { product: Product; children?: React.ReactNode }) {
  return (
    // <MobileContainer>
    <div className="bg-background-light dark:bg-background-dark relative min-h-screen text-slate-800 dark:text-slate-200">
      <header className="absolute top-0 right-0 left-0 z-10 flex items-center justify-between p-4">
        <Link className="flex items-center gap-2" to={'/app/store/builder'}>
          <X />
          <span className="font-bold text-slate-900 dark:text-white">My Store</span>
        </Link>
      </header>
      <main className="pb-28">
        <div className="bg-accent flex h-80 w-full items-center justify-center">
          {/* Change to lazy Image */}
          <img alt="Product Image" className="h-full w-full object-cover" src={product.thumbnailPicture?.mainLink} />
        </div>
        <div className="space-y-5 p-6">
          <h1 className="text-3xl font-extrabold text-slate-900 dark:text-white">{product.title}</h1>
          <p className="mt-2 text-lg text-slate-600 dark:text-slate-400">{product.subtitle}</p>
          <div className="mt-4">
            <span className="text-primary text-4xl font-bold">{product.price.toFixed(2)}$</span>
          </div>
          <div className="mt-6 space-y-4 text-slate-700 dark:text-slate-300">
            <h2 className="text-xl font-bold text-slate-900 dark:text-white">Description</h2>
            <p>{product.description}</p>
          </div>
          {/* <div className="bg-primary/10 dark:bg-primary/20 mt-6 rounded-lg p-4">
              <h3 className="font-bold text-slate-900 dark:text-white">Fulfillment Note</h3>
              <p className="mt-1 text-sm text-slate-700 dark:text-slate-300">{fulfillmentNote}</p>
            </div> */}

          {children}
          <button
            onClick={() => alert('Purchase completed!')}
            className="bg-primary shadow-primary/30 hover:bg-opacity-90 mt-10 h-14 w-full rounded-xl text-lg font-bold text-white shadow-lg transition-all duration-300"
          >
            Buy Now
          </button>
        </div>
      </main>
    </div>
    // </MobileContainer>
  );
}
