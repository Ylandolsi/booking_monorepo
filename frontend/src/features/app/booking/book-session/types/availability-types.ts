export type SessionSlotType = {
  startTime: string; // 16:00
  endTime: string; // ISO string format
  isAvailable: boolean;
  isBooked: boolean;
};

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
