export type SessionSlot = {
  startTime: string; // 16:00
  endTime: string; // ISO string format
  isAvailable: boolean;
  isBooked: boolean;
};

export type DayAvailabilityType = {
  date: string; // YYYY-MM-DD format
  isAvailable: boolean; // exists at least on sessionSlot available
  timeSlots: SessionSlot[];
  summary: DailySummaryType;
};

export type DailySummaryType = {
  totalSlots: number;
  availableSlots: number;
  bookedSlots: number;
  availabilityPercentage: number;
};

export type MonthAvailability = {
  year: number;
  month: number; // 1-12
  days: DayAvailabilityType[];
};
