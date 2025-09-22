import { useState, useEffect, useMemo } from 'react';

import { mapDayToNumber } from '@/utils/enum-days-week';
import type { AvailabilityRangeType } from '@/features/app/mentor/schedule/types';
import type { DayOfWeek } from '@/features/app/session/booking/shared';
import { useSetWeeklySchedule, useWeeklySchedule } from '@/features/app/mentor/schedule';
import { GenerateIdNumber } from '@/lib';
import type { DailySchedule } from '@/api/stores/produtcs/sessions';

export interface UseAvailabilityScheduleReturn {
  schedule: DailySchedule[];
  scheduleQuery: any;
  hasChanges: boolean;
  isSaving: boolean;
  saveSuccess: boolean;
  selectedCopySource: DayOfWeek | null;
  actions: {
    setSelectedCopySource: (day: DayOfWeek | null) => void;
    toggleDay: (day: DayOfWeek) => void;
    addTimeSlot: (day: DayOfWeek, slot: { startTime: string; endTime: string }) => void;
    addCustomTimeSlot: (day: DayOfWeek) => void;
    updateTimeRange: (day: DayOfWeek, rangeId: string, field: 'startTime' | 'endTime', value: string) => void;
    removeTimeRange: (day: DayOfWeek, rangeId: string) => void;
    copyAvailability: (fromDay: DayOfWeek, toDay: DayOfWeek) => void;
    saveAvailability: () => Promise<void>;
    resetChanges: () => void;
  };
  getScheduleSummary: () => { enabledDays: number; totalSlots: number };
}

export function useAvailabilitySchedule() {
  const useWeeklyScheduleMutation = useSetWeeklySchedule();
  const scheduleQuery = useWeeklySchedule();
  const { data: apiSchedule } = scheduleQuery;
  const [schedule, setSchedule] = useState<DailySchedule[]>([]);

  const [selectedCopySource, setSelectedCopySource] = useState<DayOfWeek | null>(null);

  const hasChanges = useMemo(() => {
    if (!apiSchedule) return false;
    return JSON.stringify(schedule) !== JSON.stringify(apiSchedule);
  }, [schedule, apiSchedule]);

  const [isSaving, setIsSaving] = useState(false);
  const [saveSuccess, setSaveSuccess] = useState(false);

  useEffect(() => {
    if (apiSchedule) {
      setSchedule(apiSchedule);
    }
  }, [apiSchedule]);

  const updateSchedule = (day: DayOfWeek, updater: (ds: DailySchedule) => DailySchedule) => {
    setSchedule((prev) => {
      return prev.map((ds) => (ds.dayOfWeek === mapDayToNumber(day) ? updater(ds) : ds));
    });
  };

  const toggleDay = (day: DayOfWeek) => {
    updateSchedule(day, (ds) => ({ ...ds, isActive: !ds.isActive }));
  };

  const addTimeSlot = (day: DayOfWeek, slot: { startTime: string; endTime: string }) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: [
        ...ds.availabilityRanges,
        {
          id: GenerateIdNumber(),
          startTime: slot.startTime,
          endTime: slot.endTime,
        },
      ],
    }));
  };

  const addCustomTimeSlot = (day: DayOfWeek) =>
    addTimeSlot(day, {
      startTime: '09:00',
      endTime: '10:00',
    });

  const updateTimeRange = (day: DayOfWeek, rangeId: string, field: 'startTime' | 'endTime', value: string) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: ds.availabilityRanges.map((r: AvailabilityRangeType) => (r.id?.toString() === rangeId ? { ...r, [field]: value } : r)),
    }));
  };

  const removeTimeRange = (day: DayOfWeek, rangeId: string) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      availabilityRanges: ds.availabilityRanges.filter((r) => r.id?.toString() !== rangeId),
    }));
  };

  const copyAvailability = (fromDay: DayOfWeek, toDay: DayOfWeek) => {
    const source = schedule.find((s) => s.dayOfWeek === mapDayToNumber(fromDay));
    if (!source) return;
    updateSchedule(toDay, (ds) => ({
      ...ds,
      isActive: true,
      availabilityRanges: source.availabilityRanges.map((r) => ({
        ...r,
        id: GenerateIdNumber(),
      })),
    }));
    setSelectedCopySource(null);
  };

  const saveAvailability = async () => {
    setIsSaving(true);
    console.log(schedule);
    try {
      // apply mutation here of update
      await useWeeklyScheduleMutation.mutateAsync(schedule);
      setSaveSuccess(true);
      setTimeout(() => setSaveSuccess(false), 3000);
    } catch (error) {
      console.error('Failed to save availability:', error);
    } finally {
      setIsSaving(false);
    }
  };

  const resetChanges = () => {
    if (apiSchedule) {
      setSchedule(apiSchedule);
    }
    setSelectedCopySource(null);
  };

  const getScheduleSummary = () => {
    const enabledDays = schedule.filter((d) => d.isActive && d.availabilityRanges.length > 0);
    const totalSlots = enabledDays.reduce((sum, d) => sum + d.availabilityRanges.length, 0);
    return { enabledDays: enabledDays.length, totalSlots };
  };

  return {
    schedule,
    scheduleQuery,
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
