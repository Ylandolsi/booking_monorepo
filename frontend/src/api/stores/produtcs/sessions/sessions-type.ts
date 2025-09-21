import type { Duration } from '@/api/stores/types';
import type { Product } from '@/types/product';

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
