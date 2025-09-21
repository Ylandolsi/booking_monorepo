// export interface CreateSessionProductRequest {
//   title: string;
//   subtitle: string;
//   description: string;
//   previewImage?: File;
//   thumbnailImage?: File;
//   clickToPay: string;
//   price: number;
//   durationMinutes: number;
//   bufferTimeMinutes: number;
//   meetingInstructions: string;
//   dayAvailabilities: DayAvailability[];
//   timeZoneId?: string; // Default: "Africa/Tunis"
// }

import { ProductType } from '@/api/stores/produtcs/products-type';
import z from 'zod';

const createProductBaseSchema = z.object({
  title: z.string().min(3, 'Product title is required'),
  subtitle: z.string().optional(),
  description: z.string().optional(),
  price: z.number().min(0, 'Price cannot be negative'),
  clickToPay: z.string().min(1, 'Button text is required'),
  thumbnail: z.instanceof(File).optional(),
});

export const createSessionProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.Session),
  duration: z.number().positive('Duration must be positive'),
  bufferTime: z.number().min(0, 'Buffer time cannot be negative'),
  meetingInstructions: z.string().optional(),
  timeZoneId: z.string().min(1, 'Time zone is required'),
});

export const createDigitalProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.DigitalDownload),
  files: z.array(z.instanceof(File)).min(1, 'At least one file is required'),
  deliveryUrl: z.string().url('Invalid URL').optional(),
  previewImage: z.instanceof(File).optional(),
});
export const createProductSchema = z.discriminatedUnion('productType', [createSessionProductSchema, createDigitalProductSchema]);
