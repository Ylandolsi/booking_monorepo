import type { DayAvailabilityType } from '@/features/booking/book-session/types/availability-types';

export type SessionBookingRequestType = {
  mentorSlug: string;
  date: string; // YYYY-MM-DD format
  startTime: string; // HH:mm format
  duration: number; // in minutes (default 30)
  notes?: string;
};

export type TimeSlotType = {
  startTime: string;
  endTime: string;
  isBooked: boolean;
  isAvailable: boolean;
};

export type BookingStateType = {
  selectedDate: string | null; // YYYY-MM-DD format
  selectedSlot: TimeSlotType | null;
  currentMonth: number; // 1-12
  currentYear: number;
  availability: DayAvailabilityType[];
  isLoading: boolean;
  error: string | null;
};

export type BookingContextType = {
  state: BookingStateType;
  actions: {
    setSelectedDate: (date: string | null) => void;
    setSelectedSlot: (slot: TimeSlotType | null) => void;
    setCurrentMonth: (month: number) => void;
    setCurrentYear: (year: number) => void;
    setAvailability: (availability: DayAvailabilityType[]) => void;
    setLoading: (loading: boolean) => void;
    setError: (error: string | null) => void;
    resetBooking: () => void;
  };
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
