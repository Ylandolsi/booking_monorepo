import { useState, useEffect } from 'react';
import type { UseFormReturn } from 'react-hook-form';
import { mapNumberToDay } from '@/utils/enum-days-week';
import { GenerateIdNumber } from '@/lib';
import type { DailySchedule, AvailabilityRangeType, CreateProductInput } from '@/api/stores';
import type { DayOfWeek } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

export interface UseFormScheduleReturn {
  error: string;
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
    dayOfWeek: mapNumberToDay(i)!,
    isActive: false,
    availabilityRanges: [],
  }));
};

const DAY_AVAILABILITES_KEY_FORM = 'dayAvailabilities';

export function useFormSchedule(form: UseFormReturn<CreateProductInput>): UseFormScheduleReturn {
  const [selectedCopySource, setSelectedCopySource] = useState<DayOfWeek | null>(null);
  const [error, setError] = useState<string>('');
  // Watch the dayAvailabilities field from the form
  const formSchedule = form.watch(DAY_AVAILABILITES_KEY_FORM) as DailySchedule[] | undefined;

  // Initialize schedule if not exists
  useEffect(() => {
    if (!formSchedule || formSchedule.length === 0) {
      form.setValue(DAY_AVAILABILITES_KEY_FORM, createDefaultSchedule());
    }
    const message = verifyScheduleIntegrity();
    setError(message);
  }, [form, formSchedule]);

  const schedule = formSchedule || createDefaultSchedule();

  const verifyScheduleIntegrity = () => {
    const currentSchedule = form.getValues(DAY_AVAILABILITES_KEY_FORM) as DailySchedule[];
    let message: string = '';
    if (!currentSchedule) return message;
    for (const daySchedule of currentSchedule) {
      if (daySchedule.isActive) {
        // ensure daily schedules :
        // - availability ranges do not overlap
        // - startTime is before endTime

        let ranges = daySchedule.availabilityRanges;
        ranges.sort((a, b) => parseInt(a.startTime.replace(':', ''), 10) - parseInt(b.startTime.replace(':', ''), 10));

        for (let i = 0; i < ranges.length; i++) {
          const rangeA = ranges[i];
          const startA = parseInt(rangeA.startTime.replace(':', ''), 10); // base 10 : remove leading zeros : 09:00 -> 900
          const endA = parseInt(rangeA.endTime.replace(':', ''), 10);

          if (startA >= endA) {
            message = `Time ranges on day ${daySchedule.dayOfWeek} overlap or are invalid.`;
            return message;
          }

          // Check for overlaps with the next range
          if (i < ranges.length - 1) {
            const rangeB = ranges[i + 1];
            const startB = parseInt(rangeB.startTime.replace(':', ''), 10);
            if (endA > startB) {
              message = `Time ranges on day ${daySchedule.dayOfWeek} overlap or are invalid.`;
              return message;
            }
          }
        }
      }
    }
    return message;
  };

  const updateSchedule = (day: DayOfWeek, updater: (ds: DailySchedule) => DailySchedule) => {
    const currentSchedule = (form.getValues(DAY_AVAILABILITES_KEY_FORM) as DailySchedule[]) || createDefaultSchedule();
    const newSchedule = currentSchedule.map((ds) => (ds.dayOfWeek === day ? updater(ds) : ds));
    form.setValue(DAY_AVAILABILITES_KEY_FORM, newSchedule, { shouldValidate: true });
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
    const source = schedule.find((s) => s.dayOfWeek === fromDay);
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
    form.setValue(DAY_AVAILABILITES_KEY_FORM, createDefaultSchedule());
    setSelectedCopySource(null);
  };

  const getScheduleSummary = () => {
    const enabledDays = schedule.filter((d) => d.isActive && d.availabilityRanges.length > 0);
    const totalSlots = enabledDays.reduce((sum, d) => sum + d.availabilityRanges.length, 0);
    return { enabledDays: enabledDays.length, totalSlots };
  };

  return {
    error,
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
