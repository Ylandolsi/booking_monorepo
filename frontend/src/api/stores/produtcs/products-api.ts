// /**
//  * Products API Implementation
//  * Handles session products and other product types
//  */

// import { api } from '@/lib/api/api-client';
// import { CatalogEndpoints } from '@/lib/api/catalog-endpoints';
// import { toFormData, validateFile } from '@/types/catalog-api';
// import type {
//   SessionProductResponse,
//   PatchPostProductResponse,
//   DailyAvailabilityResponse,
//   MonthlyAvailabilityResponse,
//   BookSessionRequest,
//   BookSessionResponse,
//   DayAvailability,
// } from '@/types/catalog-api';

// // ===== SESSION PRODUCT TYPES =====

// export interface CreateSessionProductInput {
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
//   timeZoneId?: string;
// }

// export interface UpdateSessionProductInput extends CreateSessionProductInput {
//   productSlug: string;
// }

// // ===== CREATE SESSION PRODUCT =====

// export const createSessionProduct = async (data: CreateSessionProductInput): Promise<PatchPostProductResponse> => {
//   // Validate files if provided
//   if (data.previewImage) {
//     const validation = validateFile(data.previewImage, {
//       maxSizeInMB: 10,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!validation.isValid) {
//       throw new Error(`Preview image: ${validation.error}`);
//     }
//   }

//   if (data.thumbnailImage) {
//     const validation = validateFile(data.thumbnailImage, {
//       maxSizeInMB: 5,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!validation.isValid) {
//       throw new Error(`Thumbnail image: ${validation.error}`);
//     }
//   }

//   // Create FormData for the request
//   const formData = toFormData({
//     title: data.title,
//     subtitle: data.subtitle,
//     description: data.description,
//     previewImage: data.previewImage,
//     thumbnailImage: data.thumbnailImage,
//     clickToPay: data.clickToPay,
//     price: data.price,
//     durationMinutes: data.durationMinutes,
//     bufferTimeMinutes: data.bufferTimeMinutes,
//     meetingInstructions: data.meetingInstructions,
//     dayAvailabilities: data.dayAvailabilities,
//     timeZoneId: data.timeZoneId || 'Africa/Tunis',
//   });

//   try {
//     const response = await api.post<PatchPostProductResponse>(CatalogEndpoints.Products.Sessions.Create, formData);

//     return response;
//   } catch (error) {
//     console.error('Error creating session product:', error);
//     throw error;
//   }
// };

// // ===== UPDATE SESSION PRODUCT =====

// export const updateSessionProduct = async (data: UpdateSessionProductInput): Promise<PatchPostProductResponse> => {
//   // Validate files if provided
//   if (data.previewImage) {
//     const validation = validateFile(data.previewImage, {
//       maxSizeInMB: 10,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!validation.isValid) {
//       throw new Error(`Preview image: ${validation.error}`);
//     }
//   }

//   if (data.thumbnailImage) {
//     const validation = validateFile(data.thumbnailImage, {
//       maxSizeInMB: 5,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!validation.isValid) {
//       throw new Error(`Thumbnail image: ${validation.error}`);
//     }
//   }

//   // Create FormData for the request
//   const formData = toFormData({
//     title: data.title,
//     subtitle: data.subtitle,
//     description: data.description,
//     previewImage: data.previewImage,
//     thumbnailImage: data.thumbnailImage,
//     clickToPay: data.clickToPay,
//     price: data.price,
//     durationMinutes: data.durationMinutes,
//     bufferTimeMinutes: data.bufferTimeMinutes,
//     meetingInstructions: data.meetingInstructions,
//     dayAvailabilities: data.dayAvailabilities,
//     timeZoneId: data.timeZoneId || 'Africa/Tunis',
//   });

//   try {
//     const response = await api.put<PatchPostProductResponse>(CatalogEndpoints.Products.Sessions.Update(data.productSlug), formData);

//     return response;
//   } catch (error) {
//     console.error('Error updating session product:', error);
//     throw error;
//   }
// };

// // ===== GET SESSION PRODUCT =====

// export const getSessionProduct = async (productSlug: string): Promise<SessionProductResponse> => {
//   try {
//     const response = await api.get<SessionProductResponse>(CatalogEndpoints.Products.Sessions.Get(productSlug));
//     return response;
//   } catch (error) {
//     console.error(`Error fetching session product ${productSlug}:`, error);
//     throw error;
//   }
// };

// // ===== GET BOOKED SESSIONS =====

// export interface BookedSession {
//   id: string;
//   customerName: string;
//   customerEmail: string;
//   startTime: string;
//   endTime: string;
//   status: 'pending' | 'confirmed' | 'cancelled' | 'completed';
//   meetingLink?: string;
//   productTitle: string;
//   productSlug: string;
// }

// export const getBookedSessions = async (): Promise<BookedSession[]> => {
//   try {
//     const response = await api.get<BookedSession[]>(CatalogEndpoints.Products.Sessions.GetSessions);
//     return response;
//   } catch (error) {
//     console.error('Error fetching booked sessions:', error);
//     throw error;
//   }
// };

// // ===== AVAILABILITY FUNCTIONS =====

// export const getDailyAvailability = async (productSlug: string, date: string): Promise<DailyAvailabilityResponse> => {
//   try {
//     const response = await api.get<DailyAvailabilityResponse>(CatalogEndpoints.Products.Sessions.GetDailyAvailability(productSlug), {
//       params: { date },
//     });
//     return response;
//   } catch (error) {
//     console.error(`Error fetching daily availability for ${productSlug} on ${date}:`, error);
//     throw error;
//   }
// };

// export const getMonthlyAvailability = async (
//   productSlug: string,
//   year: number,
//   month: number,
//   timeZoneId: string = 'Africa/Tunis',
// ): Promise<MonthlyAvailabilityResponse> => {
//   try {
//     const response = await api.get<MonthlyAvailabilityResponse>(CatalogEndpoints.Products.Sessions.GetMonthlyAvailability(productSlug), {
//       params: { year, month, timeZoneId },
//     });
//     return response;
//   } catch (error) {
//     console.error(`Error fetching monthly availability for ${productSlug}:`, error);
//     throw error;
//   }
// };

// // ===== BOOKING FUNCTIONS =====

// export const bookSession = async (productSlug: string, data: BookSessionRequest): Promise<BookSessionResponse> => {
//   try {
//     const response = await api.post<BookSessionResponse>(CatalogEndpoints.Products.Sessions.Book(productSlug), data);
//     return response;
//   } catch (error) {
//     console.error(`Error booking session for ${productSlug}:`, error);
//     throw error;
//   }
// };

// // ===== VALIDATION HELPERS =====

// /**
//  * Validate session product creation input
//  */
// export function validateCreateSessionProductInput(data: CreateSessionProductInput): { isValid: boolean; errors: string[] } {
//   const errors: string[] = [];

//   // Title validation
//   if (!data.title?.trim()) {
//     errors.push('Product title is required');
//   } else if (data.title.length > 100) {
//     errors.push('Product title cannot exceed 100 characters');
//   }

//   // Subtitle validation
//   if (!data.subtitle?.trim()) {
//     errors.push('Product subtitle is required');
//   } else if (data.subtitle.length > 200) {
//     errors.push('Product subtitle cannot exceed 200 characters');
//   }

//   // Description validation
//   if (!data.description?.trim()) {
//     errors.push('Product description is required');
//   } else if (data.description.length > 2000) {
//     errors.push('Product description cannot exceed 2000 characters');
//   }

//   // Click to pay validation
//   if (!data.clickToPay?.trim()) {
//     errors.push('Click to pay text is required');
//   } else if (data.clickToPay.length > 50) {
//     errors.push('Click to pay text cannot exceed 50 characters');
//   }

//   // Price validation
//   if (typeof data.price !== 'number' || data.price < 0) {
//     errors.push('Price must be a non-negative number');
//   } else if (data.price > 10000) {
//     errors.push('Price cannot exceed 10,000');
//   }

//   // Duration validation
//   if (!data.durationMinutes || data.durationMinutes < 15) {
//     errors.push('Session duration must be at least 15 minutes');
//   } else if (data.durationMinutes > 480) {
//     errors.push('Session duration cannot exceed 8 hours');
//   }

//   // Buffer time validation
//   if (data.bufferTimeMinutes < 0) {
//     errors.push('Buffer time cannot be negative');
//   } else if (data.bufferTimeMinutes > 60) {
//     errors.push('Buffer time cannot exceed 60 minutes');
//   }

//   // Meeting instructions validation
//   if (!data.meetingInstructions?.trim()) {
//     errors.push('Meeting instructions are required');
//   } else if (data.meetingInstructions.length > 1000) {
//     errors.push('Meeting instructions cannot exceed 1000 characters');
//   }

//   // Day availabilities validation
//   if (!data.dayAvailabilities || data.dayAvailabilities.length === 0) {
//     errors.push('At least one day availability must be configured');
//   } else {
//     const activeDays = data.dayAvailabilities.filter((day) => day.isActive);
//     if (activeDays.length === 0) {
//       errors.push('At least one day must be active');
//     }

//     activeDays.forEach((day) => {
//       if (!day.availabilityRanges || day.availabilityRanges.length === 0) {
//         errors.push(`Day ${day.dayOfWeek}: At least one time range is required for active days`);
//       } else {
//         day.availabilityRanges.forEach((range, rangeIndex) => {
//           if (!range.startTime || !range.endTime) {
//             errors.push(`Day ${day.dayOfWeek}, Range ${rangeIndex + 1}: Start and end times are required`);
//           } else {
//             const start = new Date(`2000-01-01 ${range.startTime}`);
//             const end = new Date(`2000-01-01 ${range.endTime}`);
//             if (start >= end) {
//               errors.push(`Day ${day.dayOfWeek}, Range ${rangeIndex + 1}: End time must be after start time`);
//             }
//           }
//         });
//       }
//     });
//   }

//   // File validations
//   if (data.previewImage) {
//     const validation = validateFile(data.previewImage, {
//       maxSizeInMB: 10,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!validation.isValid) {
//       errors.push(`Preview image: ${validation.error}`);
//     }
//   }

//   if (data.thumbnailImage) {
//     const validation = validateFile(data.thumbnailImage, {
//       maxSizeInMB: 5,
//       allowedTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
//       required: false,
//     });

//     if (!validation.isValid) {
//       errors.push(`Thumbnail image: ${validation.error}`);
//     }
//   }

//   return {
//     isValid: errors.length === 0,
//     errors,
//   };
// }

// /**
//  * Validate booking request
//  */
// export function validateBookSessionRequest(data: BookSessionRequest): { isValid: boolean; errors: string[] } {
//   const errors: string[] = [];

//   // Start time validation
//   if (!data.startTime) {
//     errors.push('Start time is required');
//   } else {
//     const startTime = new Date(data.startTime);
//     const now = new Date();
//     if (startTime <= now) {
//       errors.push('Start time must be in the future');
//     }
//   }

//   // Customer info validation
//   if (!data.customerName?.trim()) {
//     errors.push('Customer name is required');
//   } else if (data.customerName.length > 100) {
//     errors.push('Customer name cannot exceed 100 characters');
//   }

//   if (!data.customerEmail?.trim()) {
//     errors.push('Customer email is required');
//   } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.customerEmail)) {
//     errors.push('Invalid email format');
//   }

//   if (data.customerPhone && data.customerPhone.length > 20) {
//     errors.push('Phone number cannot exceed 20 characters');
//   }

//   if (data.customerMessage && data.customerMessage.length > 500) {
//     errors.push('Message cannot exceed 500 characters');
//   }

//   return {
//     isValid: errors.length === 0,
//     errors,
//   };
// }

// /**
//  * Check if error is a specific product-related error
//  */
// export function isProductError(error: any, type: 'not-found' | 'unavailable' | 'booking-conflict'): boolean {
//   if (!error?.message) return false;

//   const message = error.message.toLowerCase();

//   switch (type) {
//     case 'not-found':
//       return message.includes('not found') || message.includes('404');
//     case 'unavailable':
//       return message.includes('unavailable') || message.includes('not available');
//     case 'booking-conflict':
//       return message.includes('conflict') || message.includes('already booked') || message.includes('time slot taken');
//     default:
//       return false;
//   }
// }

// /**
//  * Get user-friendly error message for product operations
//  */
// export function getProductErrorMessage(error: any, operation: 'create' | 'update' | 'book' | 'fetch'): string {
//   if (isProductError(error, 'not-found')) {
//     return 'Product not found. It may have been deleted or the link is incorrect.';
//   }

//   if (isProductError(error, 'unavailable')) {
//     return 'This time slot is no longer available. Please select a different time.';
//   }

//   if (isProductError(error, 'booking-conflict')) {
//     return 'This time slot has been booked by someone else. Please select a different time.';
//   }

//   // Default messages based on operation
//   switch (operation) {
//     case 'create':
//       return 'Failed to create product. Please check your information and try again.';
//     case 'update':
//       return 'Failed to update product. Please try again.';
//     case 'book':
//       return 'Failed to book session. Please try again or select a different time.';
//     case 'fetch':
//       return 'Failed to load product information. Please try again.';
//     default:
//       return 'An unexpected error occurred. Please try again.';
//   }
// }
