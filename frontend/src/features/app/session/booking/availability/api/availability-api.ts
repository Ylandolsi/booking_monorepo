import { api } from '@/lib';
import {
  queryOptions,
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { MentorshipEndpoints } from '@/lib/api/mentor-endpoints';
import type {
  DayAvailabilityType,
  MonthAvailabilityType,
} from '@/features/app/session/booking/availability/types';
import { availabilityQueryKeys } from '@/features/app/session/booking/availability/api/availability-keys';

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
): Promise<MonthAvailabilityType> => {
  if (!mentorSlug || !year || !month) {
    throw new Error('mentorSlug, year, and month are required');
  }

  return await api.get<MonthAvailabilityType>(
    `${MentorshipEndpoints.Availability.GetMonthly}?mentorSlug=${mentorSlug}&year=${year}&month=${month}`,
  );
};

export function useDailyAvailability(
  mentorSlug?: string,
  date?: string,
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<DayAvailabilityType, Error> {
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
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<MonthAvailabilityType, Error> {
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
