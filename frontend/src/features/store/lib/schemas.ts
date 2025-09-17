import { z } from 'zod';
import { ProductType } from '../types';

export const createStoreSchema = z.object({
  title: z.string().min(3, 'Store name must be at least 3 characters'),
  slug: z
    .string()
    .min(3, 'Slug must be at least 3 characters')
    .regex(/^[a-z0-9-]+$/, 'Slug can only contain lowercase letters, numbers, and hyphens'),
  description: z.string().optional(),
  picture: z.instanceof(File).optional(),
  socialLinks: z
    .array(
      z.object({
        platform: z.string(),
        url: z.string().url('Invalid URL'),
      }),
    )
    .optional(),
});

export const updateStoreSchema = createStoreSchema.partial().extend({
  id: z.number(),
});

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
