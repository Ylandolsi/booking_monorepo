export type AvailabilityRangeType = {
  id: number | undefined;
  startTime: string;
  endTime: string;
};

export type DailySchedule = {
  dayOfWeek: number; // // 0-6 (Sunday-Saturday)
  isActive: boolean;
  availabilityRanges: AvailabilityRangeType[];
};
