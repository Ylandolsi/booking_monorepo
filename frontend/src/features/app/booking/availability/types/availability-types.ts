import type { SessionSlotType } from '@/features/app/booking/shared';

export type DayAvailabilityType = {
  date: string; // YYYY-MM-DD format
  isAvailable: boolean; // exists at least on sessionSlot available
  timeSlots: SessionSlotType[];
  summary: DailySummaryType;
};

export type DailySummaryType = {
  totalSlots: number;
  availableSlots: number;
  bookedSlots: number;
  availabilityPercentage: number;
};

export type MonthAvailabilityType = {
  year: number;
  month: number; // 1-12
  days: DayAvailabilityType[];
};
// {
//     "year": 2025,
//     "month": 8,
//     "days": [
//         {
//             "date": "2025-08-01T00:00:00Z",
//             "isAvailable": false,
//             "timeSlots": [],
//             "summary": {
//                 "totalSlots": 0,
//                 "availableSlots": 0,
//                 "bookedSlots": 0,
//                 "availabilityPercentage": 0
//             }
//         },
//         {
//             "date": "2025-08-02T00:00:00Z",
//             "isAvailable": true,
//             "timeSlots": [
//                 {
//                     "startTime": "09:00",
//                     "endTime": "09:30",
//                     "isBooked": false,
//                     "isAvailable": true
//                 },
//                 {
//                     "startTime": "09:45",
//                     "endTime": "10:15",
//                     "isBooked": false,
//                     "isAvailable": true
//                 },
//                 {
//                     "startTime": "10:30",
//                     "endTime": "11:00",
//                     "isBooked": false,
//                     "isAvailable": true
//                 },
//                 {
//                     "startTime": "11:15",
//                     "endTime": "11:45",
//                     "isBooked": false,
//                     "isAvailable": true
//                 },
//                 {
//                     "startTime": "13:00",
//                     "endTime": "13:30",
//                     "isBooked": false,
//                     "isAvailable": true
//                 }
//             ],
//             "summary": {
//                 "totalSlots": 5,
//                 "availableSlots": 5,
//                 "bookedSlots": 0,
//                 "availabilityPercentage": 100
//             }
//         },
//         ....

//     ]
// }
