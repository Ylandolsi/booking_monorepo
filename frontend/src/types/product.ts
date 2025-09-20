interface DigitalProductDetails {
  downloadFile?: File | string | null;
  downloadLink?: string;
  previewMedia?: File | string | null;
}

interface BookingDetails {
  duration: number; // in minutes
  bufferTime: number; // in minutes
  timezone: string;
  meetingLink?: string;
  meetingPlatform: 'zoom' | 'google-meet' | 'custom';
  maxAttendees: number;
  availableSlots: Array<{
    day: string;
    startTime: string;
    endTime: string;
  }>;
}

interface Integration {
  id: string;
  type: 'social' | 'music' | 'custom';
  platform: string;
  url: string;
  icon: string;
  label: string;
}

interface Product {
  id: string;
  title: string;
  subtitle: string;
  price: string;
  coverImage: string;
  ctaText: string;
  type: 'booking' | 'digital';
  description?: string;
  thumbnailMode?: 'compact' | 'expanded';
  digitalDetails?: DigitalProductDetails;
  bookingDetails?: BookingDetails;
  integrations?: Integration[];
}

export type { Product, DigitalProductDetails, BookingDetails, Integration };
