/**
 * TypeScript interfaces for Catalog API
 * Based on backend C# models and DTOs
 */

// ===== ENUMS =====

export type ProductType = 'Session' | 'Course' | 'DigitalProduct' | 'Physical';

export type DayOfWeek = 'Sunday' | 'Monday' | 'Tuesday' | 'Wednesday' | 'Thursday' | 'Friday' | 'Saturday';

// ===== VALUE OBJECTS =====

export interface Picture {
  mainLink: string;
  thumbnailLink: string;
}

export interface SocialLink {
  platform: string;
  url: string;
}

export interface AvailabilityRange {
  id?: number | null;
  startTime: string; // Format: "HH:mm"
  endTime: string; // Format: "HH:mm"
}

export interface DayAvailability {
  dayOfWeek: DayOfWeek;
  isActive: boolean;
  availabilityRanges: AvailabilityRange[];
}

// ===== STORE TYPES =====

export interface CreateStoreRequest {
  title: string;
  slug: string;
  file: File;
  description: string;
  socialLinks?: SocialLink[];
}

export interface CreateStoreResponse {
  slug: string;
}

export interface UpdateStoreRequest {
  title: string;
  slug: string;
  description?: string;
  socialLinks?: SocialLink[];
}

export interface UpdateStorePictureRequest {
  file: File;
}

export interface StoreResponse {
  title: string;
  slug: string;
  description?: string;
  picture: Picture;
  isPublished: boolean;
  createdAt: string; // ISO date string
  socialLinks: SocialLink[];
}

export interface PublicStoreResponse {
  title: string;
  slug: string;
  description?: string;
  picture: Picture;
  createdAt: string; // ISO date string
  socialLinks: SocialLink[];
  products: ProductPublic[];
}

export interface ProductPublic {
  slug: string;
  title: string;
  clickToPay: string;
  subtitle?: string;
  description?: string;
  productType: ProductType;
  price: number;
  displayOrder: number;
  isPublished: boolean;
}

// ===== PRODUCT TYPES =====

export interface CreateSessionProductRequest {
  title: string;
  subtitle: string;
  description: string;
  previewImage?: File;
  thumbnailImage?: File;
  clickToPay: string;
  price: number;
  durationMinutes: number;
  bufferTimeMinutes: number;
  meetingInstructions: string;
  dayAvailabilities: DayAvailability[];
  timeZoneId?: string; // Default: "Africa/Tunis"
}

export interface UpdateSessionProductRequest {
  title: string;
  subtitle: string;
  description: string;
  previewImage?: File;
  thumbnailImage?: File;
  clickToPay: string;
  price: number;
  durationMinutes: number;
  bufferTimeMinutes: number;
  meetingInstructions: string;
  dayAvailabilities: DayAvailability[];
  timeZoneId?: string;
}

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

export interface PatchPostProductResponse {
  productSlug: string;
}

// ===== AVAILABILITY TYPES =====

export interface DailyAvailabilityRequest {
  date: string; // Format: "YYYY-MM-DD"
}

export interface MonthlyAvailabilityRequest {
  year: number;
  month: number; // 1-12
  timeZoneId?: string;
}

export interface AvailabilitySlot {
  startTime: string; // ISO date string
  endTime: string; // ISO date string
  isAvailable: boolean;
}

export interface DailyAvailabilityResponse {
  date: string;
  slots: AvailabilitySlot[];
}

export interface MonthlyAvailabilityResponse {
  year: number;
  month: number;
  availableDays: string[]; // Array of date strings "YYYY-MM-DD"
}

// ===== BOOKING TYPES =====

export interface BookSessionRequest {
  startTime: string; // ISO date string
  customerName: string;
  customerEmail: string;
  customerPhone?: string;
  customerMessage?: string;
}

export interface BookSessionResponse {
  bookingId: string;
  sessionDetails: {
    startTime: string;
    endTime: string;
    meetingLink?: string;
    instructions: string;
  };
  paymentDetails: {
    orderId: number;
    amount: number;
    paymentUrl?: string;
  };
}

// ===== ORDER TYPES =====

export interface OrderResponse {
  id: number;
  productTitle: string;
  customerName: string;
  customerEmail: string;
  amount: number;
  status: string;
  productType: ProductType;
  createdAt: string;
  paidAt?: string;
}

// ===== PAYMENT TYPES =====

export interface CreatePaymentRequest {
  orderId: number;
  amount: number;
  currency?: string; // Default: "TND"
  returnUrl?: string;
  cancelUrl?: string;
}

export interface CreatePaymentResponse {
  paymentId: string;
  paymentUrl: string;
  expiresAt: string;
}

export interface WalletResponse {
  balance: number;
  currency: string;
  lastUpdated: string;
}

// ===== PAYOUT TYPES =====

export interface PayoutRequest {
  amount: number;
  bankAccountDetails: {
    accountNumber: string;
    bankName: string;
    beneficiaryName: string;
  };
}

export interface PayoutResponse {
  payoutId: string;
  amount: number;
  status: 'Pending' | 'Approved' | 'Rejected' | 'Completed';
  requestedAt: string;
  processedAt?: string;
}

// ===== API ERROR TYPES =====

export interface ValidationError {
  code: string;
  description: string;
  type: number;
}

export interface ApiError {
  type: string;
  title: string;
  status: number;
  detail: string;
  errors?: ValidationError[];
}

// ===== QUERY PARAMETER TYPES =====

export interface SlugAvailabilityParams {
  slug: string;
}

export interface GetAllPayoutsParams {
  status?: 'Pending' | 'Approved' | 'Rejected' | 'Completed';
  upToDate?: string;
  timeZoneId?: string;
}

// ===== FORM DATA HELPERS =====

/**
 * Helper function to convert object to FormData
 * Handles File objects, arrays, and nested objects
 */
export function toFormData<T extends Record<string, any>>(obj: T): FormData {
  const formData = new FormData();

  Object.entries(obj).forEach(([key, value]) => {
    if (value === undefined || value === null) {
      return; // Skip undefined/null values
    }

    if (value instanceof File) {
      formData.append(key, value);
    } else if (Array.isArray(value)) {
      formData.append(key, JSON.stringify(value));
    } else if (typeof value === 'object') {
      formData.append(key, JSON.stringify(value));
    } else {
      formData.append(key, String(value));
    }
  });

  return formData;
}

/**
 * Helper function to validate file types and sizes
 */
export interface FileValidationOptions {
  maxSizeInMB?: number;
  allowedTypes?: string[];
  required?: boolean;
}

export function validateFile(file: File | undefined, options: FileValidationOptions = {}): { isValid: boolean; error?: string } {
  const { maxSizeInMB = 5, allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'], required = false } = options;

  if (!file) {
    return { isValid: !required, error: required ? 'File is required' : undefined };
  }

  if (!allowedTypes.includes(file.type)) {
    return {
      isValid: false,
      error: `File type not allowed. Allowed types: ${allowedTypes.join(', ')}`,
    };
  }

  const maxSizeInBytes = maxSizeInMB * 1024 * 1024;
  if (file.size > maxSizeInBytes) {
    return {
      isValid: false,
      error: `File size exceeds ${maxSizeInMB}MB limit`,
    };
  }

  return { isValid: true };
}
