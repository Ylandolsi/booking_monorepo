import { api } from '@/lib';
import {
  queryOptions,
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import type {
  DayAvailability,
  MonthAvailability,
} from '../types/availability-types';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { bookingQueryKeys } from './booking-keys';

export const getDailyAvailability = async (
  mentorSlug: string,
  date: string,
): Promise<DayAvailability> => {
  if (!mentorSlug || !date) {
    throw new Error('mentorSlug and date are required');
  }

  return await api.get<DayAvailability>(
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
): UseQueryResult<DayAvailability, unknown> {
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
