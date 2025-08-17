import { useWeeklySchedule } from '../api';

/**
 * Data-focused hook for schedule information
 */
export function useScheduleData() {
  const scheduleQuery = useWeeklySchedule();

  return {
    scheduleQuery,
    schedule: scheduleQuery.data,
    isLoading: scheduleQuery.isLoading,
    isError: scheduleQuery.isError,
    error: scheduleQuery.error,
    refetch: scheduleQuery.refetch,
  };
}
