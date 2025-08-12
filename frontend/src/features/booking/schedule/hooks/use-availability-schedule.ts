import { useState, useEffect, useMemo } from 'react';

import { mapDayToNumber } from '@/utils/enum-days-week';
import type {
  AvailabilityRangeType,
  DailySchedule,
} from '@/features/booking/schedule/types';
import type { DayOfWeek } from '@/features/booking/shared';
import {
  useSetWeeklySchedule,
  useWeeklySchedule,
} from '@/features/booking/schedule';
import { GenerateIdNumber } from '@/utils';

export interface UseAvailabilityScheduleReturn {
  schedule: DailySchedule[];

  hasChanges: boolean;
  isSaving: boolean;
  saveSuccess: boolean;
  selectedCopySource: DailySchedule[] | null;
  // actions: {
  //   setSelectedCopySource: (day: DailySchedule[] | null) => void;
  //   toggleDay: (day: WeeklySchedule) => void;
  //   addTimeSlot: (
  //     day: WeeklySchedule,
  //     slot: { start: string; end: string },
  //   ) => void;
  //   addCustomTimeSlot: (day: WeeklySchedule) => void;
  //   updateTimeRange: (
  //     day: WeeklySchedule,
  //     rangeId: string,
  //     field: 'start' | 'end',
  //     value: string,
  //   ) => void;
  //   removeTimeRange: (day: WeeklySchedule, rangeId: string) => void;
  //   copyAvailability: (fromDay: WeeklySchedule, toDay: WeeklySchedule) => void;
  //   saveAvailability: () => Promise<void>;
  //   resetChanges: () => void;
  // };
  // getScheduleSummary: () => { enabledDays: number; totalSlots: number };
}

export function useAvailabilitySchedule() {
  const useWeeklyScheduleMutation = useSetWeeklySchedule();
  const [schedule, setSchedule] = useState<DailySchedule[]>();
  const { data: apiSchedule, error, isLoading } = useWeeklySchedule();
  // once it loads , create state with that data !

  const [selectedCopySource, setSelectedCopySource] =
    useState<DailySchedule | null>(null);

  const hasChanges = useMemo(() => {
    if (!apiSchedule) return false;
    return JSON.stringify(schedule) !== JSON.stringify(apiSchedule);
  }, [schedule, apiSchedule]);

  const [isSaving, setIsSaving] = useState(false);
  const [saveSuccess, setSaveSuccess] = useState(false);

  useEffect(() => {
    setSchedule(apiSchedule);
  }, [apiSchedule]);

  const updateSchedule = (
    day: DayOfWeek,
    updater: (ds: DailySchedule) => DailySchedule,
  ) => {
    setSchedule((prev) => {
      if (prev == undefined) return;
      return prev.map((ds) =>
        ds.dayOfWeek === mapDayToNumber(day) ? updater(ds) : ds,
      );
    });
  };

  const toggleDay = (day: DayOfWeek) => {
    updateSchedule(day, (ds) => ({ ...ds, isActive: !ds.isActive }));
  };

  const addTimeSlot = (day: DayOfWeek, slot: AvailabilityRangeType) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: [...ds.availabilityRanges, { ...slot }],
    }));
  };

  const addCustomTimeSlot = (day: DayOfWeek) =>
    addTimeSlot(day, {
      startTime: '09:00',
      endTime: '10:00',
    } as AvailabilityRangeType);

  const updateTimeRange = (
    day: DayOfWeek,
    rangeId: number,
    field: 'start' | 'end',
    value: string,
  ) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: ds.availabilityRanges.map(
        (r: AvailabilityRangeType) =>
          r.id === rangeId ? { ...r, [field]: value } : r,
      ),
    }));
  };

  const removeTimeRange = (day: DayOfWeek, rangeId: number) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: ds.availabilityRanges.filter((r) => r.id !== rangeId),
    }));
  };

  const copyAvailability = (fromDay: DayOfWeek, toDay: DayOfWeek) => {
    if (schedule == undefined) return;
    const source = schedule.find(
      (s) => s.dayOfWeek === mapDayToNumber(fromDay),
    );
    if (!source) return;
    updateSchedule(toDay, (ds) => ({
      ...ds,
      isActive: true,
      availabilityRanges: source.availabilityRanges.map((r) => ({
        ...r,
        id: GenerateIdNumber(),
      })),
    }));
  };

  const saveAvailability = async () => {
    setIsSaving(true);
    try {
      // apply mutation here of update
      await useWeeklyScheduleMutation.mutateAsync(schedule!);
      setSaveSuccess(true);
      setTimeout(() => setSaveSuccess(false), 3000);
    } catch (error) {
      console.error('Failed to save availability:', error);
    } finally {
      setIsSaving(false);
    }
  };

  const resetChanges = () => {
    setSchedule(apiSchedule);
    setSelectedCopySource(null);
  };

  const getScheduleSummary = () => {
    if (schedule == undefined) return;
    const enabledDays = schedule.filter(
      (d) => d.isActive && d.availabilityRanges.length > 0,
    );
    const totalSlots = enabledDays.reduce(
      (sum, d) => sum + d.availabilityRanges.length,
      0,
    );
    return { enabledDays: enabledDays.length, totalSlots };
  };

  return {
    schedule,
    hasChanges,
    isSaving,
    saveSuccess,
    selectedCopySource,
    actions: {
      setSelectedCopySource,
      toggleDay,
      addTimeSlot,
      addCustomTimeSlot,
      updateTimeRange,
      removeTimeRange,
      copyAvailability,
      saveAvailability,
      resetChanges,
    },
    getScheduleSummary,
  };
}
