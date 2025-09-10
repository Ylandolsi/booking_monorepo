import { api } from '@/lib';
import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';
import { MentorshipEndpoints } from '@/lib/api/mentor-endpoints';
import type { MonthAvailabilityType } from '@/features/app/session/booking/availability/types';
import { availabilityQueryKeys } from '@/features/app/session/booking/availability/api/availability-keys';

export const getMonthlyAvailability = async (mentorSlug: string, year: number, month: number): Promise<MonthAvailabilityType> => {
  if (!mentorSlug || !year || !month) {
    throw new Error('mentorSlug, year, and month are required');
  }

  return await api.get<MonthAvailabilityType>(`${MentorshipEndpoints.Availability.GetMonthly}?mentorSlug=${mentorSlug}&year=${year}&month=${month}`);
};

export function useMonthlyAvailability(
  mentorSlug?: string,
  year?: number,
  month?: number,
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<MonthAvailabilityType, Error> {
  return useQuery(
    queryOptions({
      queryKey: availabilityQueryKeys.monthlyAvailability(mentorSlug, year, month),
      queryFn: () => getMonthlyAvailability(mentorSlug!, year!, month!),
      enabled: !!mentorSlug && !!year && !!month,
      ...overrides,
    }),
  );
}
