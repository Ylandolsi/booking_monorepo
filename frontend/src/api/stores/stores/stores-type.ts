import type { Picture, Product } from '@/api/stores';

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
