export type TimeSlot = {
  id: string;
  start: string; // ISO string format
  end: string; // ISO string format
  isAvailable: boolean;
  isBooked: boolean;
};

export type AvailabilitySlot = {
  id: string;
  mentorSlug: string;
  date: string; // YYYY-MM-DD format
  startTime: string; // HH:mm format
  endTime: string; // HH:mm format
  isAvailable: boolean;
  bufferMinutes: number;
  createdAt: string;
  updatedAt: string;
};

export type DayAvailability = {
  date: string; // YYYY-MM-DD format
  slots: TimeSlot[];
};

export type MonthAvailability = {
  year: number;
  month: number; // 1-12
  days: DayAvailability[];
};
