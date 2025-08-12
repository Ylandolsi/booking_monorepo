import { api } from '@/lib';
import {
  queryOptions,
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { bookingQueryKeys } from '@/features/booking';
import type {
  DayAvailabilityType,
  MonthAvailability,
} from '@/features/booking/set-availability';

// GET

export type WeeklySchedule = {
  dayOfWeek: number;
  availabilityRanges: {
    id: number;
    timeRange: string; // $"{StartHour:D2}:{StartMinute:D2}-{EndHour:D2}:{EndMinute:D2}";
  };
};

export const getWeeklySchedule = async (): Promise<WeeklySchedule> => {
  return api.get<WeeklySchedule>(MentorshipEndpoints.Availability.GetSchedule);
};

export const getDailyAvailability = async (
  mentorSlug: string,
  date: string, // YYYY-MM-DD
): Promise<DayAvailabilityType> => {
  if (!mentorSlug || !date) {
    throw new Error('mentorSlug and date are required');
  }

  const dateRegex = /^\d{4}-\d{2}-\d{2}$/;
  if (!dateRegex.test(date)) {
    throw new Error('date must be in YYYY-MM-DD format');
  }

  return await api.get<DayAvailabilityType>(
    `${MentorshipEndpoints.Availability.GetDaily}?mentorSlug=${mentorSlug}&date=${date}`,
  );
};

export const getMonthlyAvailability = async (
  mentorSlug: string,
  year: number,
  month: number,
): Promise<MonthAvailability> => {
  if (!mentorSlug || !year || !month) {
    throw new Error('mentorSlug, year, and month are required');
  }

  return await api.get<MonthAvailability>(
    `${MentorshipEndpoints.Availability.GetMonthly}?mentorSlug=${mentorSlug}&year=${year}&month=${month}`,
  );
};

export function useDailyAvailability(
  mentorSlug?: string,
  date?: string,
  overrides?: Partial<UseQueryOptions<any, unknown>>,
): UseQueryResult<DayAvailabilityType, unknown> {
  return useQuery(
    queryOptions({
      queryKey: bookingQueryKeys.dailyAvailability(mentorSlug, date),
      queryFn: () => getDailyAvailability(mentorSlug!, date!),
      enabled: !!mentorSlug && !!date,
      ...overrides,
    }),
  );
}

export function useMonthlyAvailability(
  mentorSlug?: string,
  year?: number,
  month?: number,
  overrides?: Partial<UseQueryOptions<any, unknown>>,
): UseQueryResult<MonthAvailability, unknown> {
  return useQuery(
    queryOptions({
      queryKey: bookingQueryKeys.monthlyAvailability(mentorSlug, year, month),
      queryFn: () => getMonthlyAvailability(mentorSlug!, year!, month!),
      enabled: !!mentorSlug && !!year && !!month,
      ...overrides,
    }),
  );
}
// SET
export interface AvailabilityRequest {
  dayOfWeek: number; // 0 (Sunday) to 6 (Saturday)
  startTime: string; // "HH:mm" format
  endTime: string; // "HH:mm" format
}

export const setAvailability = async (
  request: AvailabilityRequest,
): Promise<number> => {
  if (!request) throw new Error('Availability request is required');
  return api.post<number>(MentorshipEndpoints.Availability.Set, request);
};

// BULK SET
export interface TimeRange {
  startTime: string;
  endTime: string;
}

export interface DayAvailabilityBulk {
  dayOfWeek: number;
  timeSlots: TimeRange[];
}

export interface BulkAvailabilityRequest {
  availabilities: DayAvailabilityBulk[];
}

// request :
/**
 * {
 *    [
 *       { dayOfWeek  , [ {startTime , endTime }]}
 *       { dayOfWeek  , [ {startTime , endTime }]}
 *       { dayOfWeek  , [ {startTime , endTime }]}
 *    ]
 * }
 *  */
export const setBulkAvailability = async (
  request: BulkAvailabilityRequest,
): Promise<number[]> => {
  if (!request) throw new Error('Bulk availability request is required');
  return api.post<number[]>(MentorshipEndpoints.Availability.SetBulk, request);
};
