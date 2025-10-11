import type { Picture } from '@/api/stores/types';

export interface Product {
  productSlug: string;
  storeSlug: string;
  title: string;
  subtitle?: string;
  description?: string;
  productType: ProductType;
  price: number;
  clickToPay: string;
  displayOrder: number;
  isPublished: boolean;
  thumbnailPicture?: Picture;
  productStyle: ProductStyle;
  preview?: Picture;
  createdAt: string;
  updatedAt?: string;
}

export const ProductStyle = {
  Full: 'Full',
  Minimal: 'Minimal',
  Compact: 'Compact',
} as const;
export type ProductStyle = (typeof ProductStyle)[keyof typeof ProductStyle];

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
