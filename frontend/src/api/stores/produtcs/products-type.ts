import type { Picture } from '@/api/stores/types';
import { createDigitalProductSchema } from '@/api/stores/produtcs/digital';
import { createSessionProductSchema } from '@/api/stores/produtcs/sessions';
import z from 'zod';

export interface Product {
  productSlug: string;
  storeSlug: string;
  title: string;
  clickToPay: string;
  subtitle?: string;
  description?: string;
  thumbnail?: Picture;
  productType: ProductType;
  price: number;
  displayOrder: number;
  isPublished: boolean;
  createdAt: string;
  updatedAt?: string;
}

export const ProductType = {
  Session: 'Session',
  DigitalDownload: 'DigitalDownload',
} as const;
export type ProductType = (typeof ProductType)[keyof typeof ProductType];

const initProduct: Product = {
  productSlug: 'test-product',
  storeSlug: 'test-store',
  title: 'my beautiful product',
  clickToPay: 'Buy Now',
  subtitle: 'Subtitle here',
  description: 'lorem ipsum dolor sit amet, consectetur adipiscing elit.',
  thumbnail: {
    mainLink: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=400&h=400&fit=crop',
    thumbnailLink: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=400&h=400&fit=crop',
  },
  productType: ProductType.Session,
  price: 50,
  displayOrder: 1,
  isPublished: true,
  createdAt: new Date().toISOString(),
  updatedAt: new Date().toISOString(),
};
const initProduct2: Product = {
  productSlug: 'test-product-2',
  storeSlug: 'test-store',
  title: 'Title title 2 ',
  clickToPay: 'Buy Now',
  subtitle: 'Subtitle here',
  description: 'lorem ipsum dolor sit amet, consectetur adipiscing elit.',
  thumbnail: {
    mainLink: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=400&h=400&fit=crop',
    thumbnailLink: 'https://images.unsplash.com/photo-1551836022-deb4988cc6c0?w=400&h=400&fit=crop',
  },
  productType: ProductType.Session,
  price: 50,
  displayOrder: 1,
  isPublished: true,
  createdAt: new Date().toISOString(),
  updatedAt: new Date().toISOString(),
};
export const initProducts: Product[] = [initProduct, initProduct2];

export interface BuyProductRequest {
  startTime: string; // ISO date string
  customerName: string;
  customerEmail: string;
  customerPhone?: string;
  customerMessage?: string;
}
