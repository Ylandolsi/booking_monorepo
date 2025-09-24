import { createProductBaseSchema } from '@/api/stores/produtcs/base-schema';
import { ProductType } from '@/api/stores/produtcs/products-type';
import { toFormData } from '@/lib/api';
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
export type CreateSessionProductRequest = z.infer<typeof createSessionProductSchema>;

export const patchPostSessionSchemaToFormData = (data: CreateSessionProductRequest) => {
  const formData = toFormData({
    title: data.title,
    ThumbnailImage: data.thumbnail || new File([], ''),
    description: data.description || '',
    timeZoneId: data.timeZoneId || 'Africa/Tunis',
    durationMinutes: data.duration,
    bufferTimeMinutes: data.bufferTime,
    meetingInstructions: data.meetingInstructions || '',
    price: data.price,
    clickToPay: data.clickToPay,
    dayAvailabilitiesJson: data.dailySchedule,
    productType: data.productType,
  });

  return formData;
};

export interface PatchPostSessionResponse {
  slug: string;
}
