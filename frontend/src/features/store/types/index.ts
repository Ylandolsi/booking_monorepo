export interface Store {
  id: number;
  userId: number;
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

export interface Picture {
  url: string;
  altText?: string;
}

export interface SocialLink {
  platform: string;
  url: string;
}

export interface Product {
  id: number;
  productSlug: string;
  storeId: number;
  storeSlug: string;
  title: string;
  clickToPay: string;
  subtitle?: string;
  description?: string;
  thumbnailUrl?: string;
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

export interface SessionProduct extends Product {
  duration: Duration;
  bufferTime: Duration;
  meetingInstructions?: string;
  timeZoneId: string;
  availabilities: SessionAvailability[];
}

export interface Duration {
  minutes: number;
}

export interface SessionAvailability {
  id: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
}

export interface DigitalProduct extends Product {
  files: ProductFile[];
  deliveryUrl?: string;
  previewImage?: string;
}

export interface ProductFile {
  id: number;
  fileName: string;
  fileUrl: string;
  fileSize: number;
  mimeType: string;
}

// Form types for creating/updating stores and products
export interface CreateStoreInput {
  title: string;
  slug: string;
  description?: string;
  picture?: File;
  socialLinks?: { platform: string; url: string }[];
}

export interface UpdateStoreInput extends Partial<CreateStoreInput> {
  id: number;
}

export interface CreateProductInput {
  title: string;
  subtitle?: string;
  description?: string;
  price: number;
  clickToPay: string;
  productType: ProductType;
  thumbnail?: File;
}

export interface CreateSessionProductInput extends CreateProductInput {
  productType: typeof ProductType.Session;
  duration: number; // in minutes
  bufferTime: number; // in minutes
  meetingInstructions?: string;
  timeZoneId: string;
}

export interface CreateDigitalProductInput extends CreateProductInput {
  productType: typeof ProductType.DigitalDownload;
  files: File[];
  deliveryUrl?: string;
  previewImage?: File;
}
