// /**
//  * TypeScript interfaces for Catalog API
//  * Based on backend C# models and DTOs
//  */

// // ===== ENUMS =====

// export type ProductType = 'Session' | 'Course' | 'DigitalProduct' | 'Physical';

// export type DayOfWeek = 'Sunday' | 'Monday' | 'Tuesday' | 'Wednesday' | 'Thursday' | 'Friday' | 'Saturday';

// // ===== VALUE OBJECTS =====

// // ===== PRODUCT TYPES =====

// export interface UpdateSessionProductRequest {
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

// export interface PatchPostProductResponse {
//   productSlug: string;
// }

// // ===== AVAILABILITY TYPES =====

// export interface DailyAvailabilityRequest {
//   date: string; // Format: "YYYY-MM-DD"
// }

// export interface MonthlyAvailabilityRequest {
//   year: number;
//   month: number; // 1-12
//   timeZoneId?: string;
// }

// export interface AvailabilitySlot {
//   startTime: string; // ISO date string
//   endTime: string; // ISO date string
//   isAvailable: boolean;
// }

// export interface DailyAvailabilityResponse {
//   date: string;
//   slots: AvailabilitySlot[];
// }

// export interface MonthlyAvailabilityResponse {
//   year: number;
//   month: number;
//   availableDays: string[]; // Array of date strings "YYYY-MM-DD"
// }

// export interface GetAllPayoutsParams {
//   status?: 'Pending' | 'Approved' | 'Rejected' | 'Completed';
//   upToDate?: string;
//   timeZoneId?: string;
// }
