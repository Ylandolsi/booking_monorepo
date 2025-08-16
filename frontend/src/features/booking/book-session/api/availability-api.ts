import { api } from '@/lib';
import {
  queryOptions,
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { availabilityQueryKeys } from '@/features/booking/book-session';
import type {
  DayAvailabilityType,
  MonthAvailability,
} from '@/features/booking/book-session';

// GET

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
      queryKey: availabilityQueryKeys.dailyAvailability(mentorSlug, date),
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
  overrides? : Partial<UseQueryOptions<any, unknown>> ,
): UseQueryResult<MonthAvailability, unknown> {
  return useQuery(
    queryOptions({
      queryKey: availabilityQueryKeys.monthlyAvailability(
        mentorSlug,
        year,
        month,
      ),
      queryFn: () => getMonthlyAvailability(mentorSlug!, year!, month!),
      enabled: !!mentorSlug && !!year && !!month,
      ...overrides,
    }),
  );
}
