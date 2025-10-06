import { MobileContainer, ProductCheckout } from '@/pages/store';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/app/store/checkout')({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div className="mx-auto flex min-h-screen flex-col items-center justify-center">
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
