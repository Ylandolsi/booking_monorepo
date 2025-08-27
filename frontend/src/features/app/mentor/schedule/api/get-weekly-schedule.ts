import { scheduleQueryKeys } from '@/features/app/mentor/schedule/api/schedule-keys';
import type { DailySchedule } from '@/features/app/mentor/schedule/types';
import { api } from '@/lib';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import {
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';

export const getWeeklySchedule = async (): Promise<DailySchedule[]> => {
  const res = await api.get<DailySchedule[]>(
    MentorshipEndpoints.Availability.GetSchedule,
  );
  return res;
};

export function useWeeklySchedule(
  overrides?: Partial<UseQueryOptions<any, Error>>,
): UseQueryResult<DailySchedule[], Error> {
  return useQuery({
    queryKey: scheduleQueryKeys.weeklySchedule,
    queryFn: getWeeklySchedule,
    ...overrides,
  });
}
