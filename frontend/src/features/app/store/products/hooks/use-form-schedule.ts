import { useState, useEffect } from 'react';
import type { UseFormReturn } from 'react-hook-form';
import { mapDayToNumber } from '@/utils/enum-days-week';
import type { DayOfWeek } from '@/features/app/session/booking/shared';
import { GenerateIdNumber } from '@/lib';
import type { DailySchedule, AvailabilityRangeType, CreateProductInput } from '@/api/stores';

export interface UseFormScheduleReturn {
  schedule: DailySchedule[];
  selectedCopySource: DayOfWeek | null;
  actions: {
    setSelectedCopySource: (day: DayOfWeek | null) => void;
    toggleDay: (day: DayOfWeek) => void;
    addTimeSlot: (day: DayOfWeek, slot: { startTime: string; endTime: string }) => void;
    addCustomTimeSlot: (day: DayOfWeek) => void;
    updateTimeRange: (day: DayOfWeek, rangeId: string, field: 'startTime' | 'endTime', value: string) => void;
    removeTimeRange: (day: DayOfWeek, rangeId: string) => void;
    copyAvailability: (fromDay: DayOfWeek, toDay: DayOfWeek) => void;
    resetChanges: () => void;
  };
  getScheduleSummary: () => { enabledDays: number; totalSlots: number };
}

// Default schedule for 7 days of the week
const createDefaultSchedule = (): DailySchedule[] => {
  return Array.from({ length: 7 }, (_, i) => ({
    dayOfWeek: i,
    isActive: false,
    availabilityRanges: [],
  }));
};

export function useFormSchedule(form: UseFormReturn<CreateProductInput>): UseFormScheduleReturn {
  const [selectedCopySource, setSelectedCopySource] = useState<DayOfWeek | null>(null);

  // Watch the dailySchedule field from the form
  const formSchedule = form.watch('dailySchedule') as DailySchedule[] | undefined;

  // Initialize schedule if not exists
  useEffect(() => {
    if (!formSchedule || formSchedule.length === 0) {
      form.setValue('dailySchedule', createDefaultSchedule());
    }
  }, [form, formSchedule]);

  const schedule = formSchedule || createDefaultSchedule();

  const updateSchedule = (day: DayOfWeek, updater: (ds: DailySchedule) => DailySchedule) => {
    const currentSchedule = (form.getValues('dailySchedule') as DailySchedule[]) || createDefaultSchedule();
    const newSchedule = currentSchedule.map((ds) => (ds.dayOfWeek === mapDayToNumber(day) ? updater(ds) : ds));
    form.setValue('dailySchedule', newSchedule, { shouldValidate: true });
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

  const resetChanges = () => {
    form.setValue('dailySchedule', createDefaultSchedule());
    setSelectedCopySource(null);
  };

  const getScheduleSummary = () => {
    const enabledDays = schedule.filter((d) => d.isActive && d.availabilityRanges.length > 0);
    const totalSlots = enabledDays.reduce((sum, d) => sum + d.availabilityRanges.length, 0);
    return { enabledDays: enabledDays.length, totalSlots };
  };

  return {
    schedule,
    selectedCopySource,
    actions: {
      setSelectedCopySource,
      toggleDay,
      addTimeSlot,
      addCustomTimeSlot,
      updateTimeRange,
      removeTimeRange,
      copyAvailability,
      resetChanges,
    },
    getScheduleSummary,
  };
}
