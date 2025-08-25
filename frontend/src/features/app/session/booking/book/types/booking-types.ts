// import type { DayAvailabilityType } from '@/features/app/booking/availability/types/availability-types';
// import type { SessionSlotType } from '@/features/app/booking/shared';

// export type BookingStateType = {
//   selectedDate: string | null; // YYYY-MM-DD format
//   selectedSlot: SessionSlotType | null;
//   currentMonth: number; // 1-12
//   currentYear: number;
//   availability: DayAvailabilityType[];
//   isLoading: boolean;
//   error: string | null;
// };

// export type BookingContextType = {
//   state: BookingStateType;
//   actions: {
//     setSelectedDate: (date: string | null) => void;
//     setSelectedSlot: (slot: SessionSlotType | null) => void;
//     setCurrentMonth: (month: number) => void;
//     setCurrentYear: (year: number) => void;
//     setAvailability: (availability: DayAvailabilityType[]) => void;
//     setLoading: (loading: boolean) => void;
//     setError: (error: string | null) => void;
//     resetBooking: () => void;
//   };
// };

// export type SessionStatusType =
//   | 'scheduled'
//   | 'in-progress'
//   | 'completed'
//   | 'cancelled'
//   | 'no-show';

export type BookSessionRequestType = {
  mentorSlug: string;
  date: string; // "YYYY-MM-DD"
  startTime: string; // HH:MM
  endTime: string;
  notes?: string;
};

export type BookingSummaryType = {
  mentor: {
    slug: string;
    name: string;
    avatar?: string;
    title?: string;
    expertise: string[];
    hourlyRate: number;
  };
  session: {
    date: string;
    time: string;
    duration: number;
    price: number;
    currency: string;
  };
  total: number;
};
