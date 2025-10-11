import type { Product } from '@/api/stores';
import type { Duration } from '@/api/stores/types';

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
