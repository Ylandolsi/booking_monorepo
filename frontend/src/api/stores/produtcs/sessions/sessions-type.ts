import type { Product } from '@/api/stores';
import type { Duration } from '@/api/stores/types';
import z from 'zod';

export const createProductBaseSchema = z.object({
  title: z.string().min(3, 'Product title is required'),
  subtitle: z.string().optional(),
  description: z.string().optional(),
  price: z.number().min(0, 'Price cannot be negative'),
  clickToPay: z.string().min(1, 'Button text is required'),
  thumbnail: z.instanceof(File).optional(),
});

export interface SessionProduct extends Product {
  duration: Duration;
  bufferTime: Duration;
  meetingInstructions?: string;
  timeZoneId: string;
  availabilities: SessionAvailability[];
}

export interface SessionAvailability {
  id: number;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
}

// export interface AvailabilityRange {
//   id?: number | null;
//   startTime: string; // Format: "HH:mm"
//   endTime: string; // Format: "HH:mm"
// }

// export interface DayAvailability {
//   dayOfWeek: number;
//   isActive: boolean;
//   availabilityRanges: AvailabilityRange[];
// }
