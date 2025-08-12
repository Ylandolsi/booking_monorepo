import type { DayAvailability, TimeSlot } from './availability-types';

export type BookingState = {
  selectedDate: string | null; // YYYY-MM-DD format
  selectedSlot: TimeSlot | null;
  currentMonth: number; // 1-12
  currentYear: number;
  availability: DayAvailability[];
  isLoading: boolean;
  error: string | null;
};

export type BookingContextType = {
  state: BookingState;
  actions: {
    setSelectedDate: (date: string | null) => void;
    setSelectedSlot: (slot: TimeSlot | null) => void;
    setCurrentMonth: (month: number) => void;
    setCurrentYear: (year: number) => void;
    setAvailability: (availability: DayAvailability[]) => void;
    setLoading: (loading: boolean) => void;
    setError: (error: string | null) => void;
    resetBooking: () => void;
  };
};

export type BookingSummary = {
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
