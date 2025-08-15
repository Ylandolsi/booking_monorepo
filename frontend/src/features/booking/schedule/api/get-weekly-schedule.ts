import { scheduleQueryKeys } from '@/features/booking/schedule/api/schedule-keys';
import type { DailySchedule } from '@/features/booking/schedule/types';
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
  overrides?: Partial<UseQueryOptions<any, unknown>>,
): UseQueryResult<DailySchedule[], unknown> {
  return useQuery({
    queryKey: scheduleQueryKeys.weeklySchedule,
    queryFn: getWeeklySchedule,
    ...overrides,
  });
}
