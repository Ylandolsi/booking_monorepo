export type AvailabilityRangeType = {
  id: number | undefined;
  startTime: string;
  endTime: string;
};

export type DailySchedule = {
  dayOfWeek: number;
  isActive: boolean;
  availabilityRanges: AvailabilityRangeType[];
};
