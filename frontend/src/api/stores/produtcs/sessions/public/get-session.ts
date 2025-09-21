import type { Picture } from '@/api/stores/types/picture-type';

export interface SessionProductResponse {
  productSlug: string;
  title: string;
  subtitle?: string;
  description?: string;
  clickToPay: string;
  price: number;
  durationMinutes: number;
  bufferTimeMinutes: number;
  meetingInstructions: string;
  dayAvailabilities: DayAvailability[];
  timeZoneId: string;
  previewImage?: Picture;
  thumbnailImage?: Picture;
  isPublished: boolean;
  displayOrder: number;
  createdAt: string;
  updatedAt: string;
}
