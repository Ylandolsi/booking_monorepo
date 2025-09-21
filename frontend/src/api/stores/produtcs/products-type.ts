import type { Picture } from '@/api/stores/types';

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
