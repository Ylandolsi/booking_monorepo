import type { Picture } from '@/api/stores/types';

export interface Product {
  productSlug: string;
  storeSlug: string;
  title: string;
  clickToPay: string;
  subtitle?: string;
  description?: string;
  productType: ProductType;
  price: number;
  displayOrder: number;
  isPublished: boolean;
  createdAt: string;
  updatedAt?: string;
  thumbnail?: Picture;
  preview?: Picture;
}

export const ProductType = {
  Session: 'Session',
  DigitalDownload: 'DigitalDownload',
} as const;
export type ProductType = (typeof ProductType)[keyof typeof ProductType];

export interface BuyProductRequest {
  startTime: string; // ISO date string
  customerName: string;
  customerEmail: string;
  customerPhone?: string;
  customerMessage?: string;
}
