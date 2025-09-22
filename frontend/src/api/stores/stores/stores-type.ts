import type { Picture, Product } from '@/api/stores';

export const initialStore: Store = {
  title: 'Linki Store',
  slug: 'linki-store',
  description: 'Welcome to my Linki store! Here you can find all my digital products and book 1:1 sessions.',
  isPublished: true,
  step: 2,
  socialLinks: [
    { platform: 'twitter', url: 'https://twitter.com/linki' },
    { platform: 'instagram', url: 'https://instagram.com/linki' },
  ],
  products: [],
  createdAt: new Date().toISOString(),
  picture: {
    mainLink: 'https://i.pravatar.cc/150?u=linki',
    thumbnailLink: 'https://i.pravatar.cc/150?u=linki',
  },
};

export interface Store {
  title: string;
  slug: string;
  description?: string;
  picture?: Picture;
  isPublished: boolean;
  step: number;
  socialLinks: SocialLink[];
  products: Product[];
  createdAt: string;
  updatedAt?: string;
}

export interface SocialLink {
  platform: string;
  url: string;
}
