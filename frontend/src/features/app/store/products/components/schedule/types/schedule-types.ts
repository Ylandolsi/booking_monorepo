export type AvailabilityRangeType = {
  id: number | undefined;
  startTime: string; // 14:00
  endTime: string; // 15:00
};

export type DailySchedule = {
  dayOfWeek: number; // // 0-6 (Sunday-Saturday)
  isActive: boolean;
  availabilityRanges: AvailabilityRangeType[];
};
