import { scheduleQueryKeys } from '@/features/booking/schedule/api/schedule-keys';
import type { DailySchedule } from '@/features/booking/schedule/types';
import { api } from '@/lib';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { useMutation } from '@tanstack/react-query';

// request :
/**
 * {
 *    [
 *       { dayOfWeek  , isActive ,  [ {startTime , endTime }]}
 *       { dayOfWeek  , isActive ,  [ {startTime , endTime }]}
 *       { dayOfWeek  , isActive ,  [ {startTime , endTime }]}
 *    ]
 * }
 *  */
export const setWeeklySchedule = async (
  request: DailySchedule[],
): Promise<number[]> => {
  if (!request) throw new Error('Bulk availability request is required');
  return api.post<number[]>(MentorshipEndpoints.Availability.SetBulk, request);
};

export const useSetWeeklySchedule = ({
  onSuccess = () => {},
}: {
  onSuccess?: () => void;
} = {}) => {
  return useMutation({
    mutationFn: setWeeklySchedule,
    meta: {
      invalidatesQuery: [scheduleQueryKeys.weeklySchedule],
      successMessage: 'Schedule updated succesfully',
      successAction: () => onSuccess(),
      errorMessage: 'Failed to update the schedule',
    },
  });
};
