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
import { createProductBaseSchema } from '@/api/stores/produtcs/sessions/sessions-type';
import z from 'zod';

export const availabilityRangeTypeSchema = z.object({
  id: z.union([z.number(), z.undefined()]),
  startTime: z.string(),
  endTime: z.string(),
});

export const dailyScheduleSchema = z.object({
  dayOfWeek: z.number(),
  isActive: z.boolean(),
  availabilityRanges: z.array(availabilityRangeTypeSchema),
});

export const createSessionProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.Session),
  duration: z.number().positive('Duration must be positive'),
  bufferTime: z.number().min(0, 'Buffer time cannot be negative'),
  meetingInstructions: z.string().optional(),
  timeZoneId: z.string().min(1, 'Time zone is required'),
  dailySchedule: z.array(dailyScheduleSchema).min(1, 'At least one day schedule is required'),
});

export type AvailabilityRangeType = z.infer<typeof availabilityRangeTypeSchema>;
export type DailySchedule = z.infer<typeof dailyScheduleSchema>;

export const createDigitalProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.DigitalDownload),
  files: z.array(z.instanceof(File)).min(1, 'At least one file is required'),
  deliveryUrl: z.string().url('Invalid URL').optional(),
  previewImage: z.instanceof(File).optional(),
});

export const createProductSchema = z.discriminatedUnion('productType', [createSessionProductSchema, createDigitalProductSchema]);

export type CreateProductInput = z.infer<typeof createProductSchema>;
