import type { Product } from '@/api/stores';

export const mockProducts: Product[] = [
  {
    productSlug: 'strategy-session',
    storeSlug: 'example-store',
    title: '1-on-1 Strategy Session',
    subtitle: 'Get personalized business advice',
    description: 'A focused 60-minute session to help you overcome business challenges and plan your next steps.',
    thumbnailPicture: {
      mainLink: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=400&h=400&fit=crop',
      thumbnailLink: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=200&h=200&fit=crop',
    },
    clickToPay: 'Book Now',
    productType: 'Session',
    price: 150,
    displayOrder: 1,
    isPublished: true,
    createdAt: new Date().toISOString(),
  },
  {
    productSlug: 'digital-course',
    storeSlug: 'example-store',
    title: 'Complete Digital Marketing Course',
    subtitle: 'Master digital marketing in 30 days',
    description: 'A comprehensive course covering SEO, social media, email marketing, and more.',
    thumbnailPicture: {
      mainLink: 'https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=400&h=400&fit=crop',
      thumbnailLink: 'https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=200&h=200&fit=crop',
    },
    clickToPay: 'Buy Course',
    productType: 'DigitalDownload',
    price: 99,
    displayOrder: 2,
    isPublished: true,
    createdAt: new Date().toISOString(),
  },
];
