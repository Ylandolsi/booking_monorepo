import { useState, useEffect } from 'react';
import { DeepCopy, GenerateId } from '@/utils';
import {
  DAYS_OF_WEEK,
  type DayOfWeek,
  type DaySchedule,
} from '@/features/booking/set-availability';

interface UseAvailabilityScheduleReturn {
  schedule: DaySchedule[];
  hasChanges: boolean;
  isSaving: boolean;
  saveSuccess: boolean;
  selectedCopySource: DayOfWeek | null;
  actions: {
    setSelectedCopySource: (day: DayOfWeek | null) => void;
    toggleDay: (day: DayOfWeek) => void;
    addTimeSlot: (day: DayOfWeek, slot: { start: string; end: string }) => void;
    addCustomTimeSlot: (day: DayOfWeek) => void;
    updateTimeRange: (
      day: DayOfWeek,
      rangeId: string,
      field: 'start' | 'end',
      value: string,
    ) => void;
    removeTimeRange: (day: DayOfWeek, rangeId: string) => void;
    copyAvailability: (fromDay: DayOfWeek, toDay: DayOfWeek) => void;
    saveAvailability: () => Promise<void>;
    resetChanges: () => void;
  };
  getScheduleSummary: () => { enabledDays: number; totalSlots: number };
}

export function useAvailabilityScheduleMock(): UseAvailabilityScheduleReturn {
  const [schedule, setSchedule] = useState<DaySchedule[]>(() =>
    DAYS_OF_WEEK.map((day) => ({
      day: day.key,
      enabled: false,
      timeRanges: [],
    })),
  );
  const [selectedCopySource, setSelectedCopySource] =
    useState<DayOfWeek | null>(null);
  const [hasChanges, setHasChanges] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [saveSuccess, setSaveSuccess] = useState(false);
  const [originalSchedule, setOriginalSchedule] = useState<DaySchedule[]>([]);

  useEffect(() => {
    const loadedSchedule = DAYS_OF_WEEK.map((day) => ({
      day: day.key,
      enabled: false,
      timeRanges: [],
    }));
    setSchedule(loadedSchedule);
    setOriginalSchedule(DeepCopy(loadedSchedule));
  }, []);

  useEffect(() => {
    setHasChanges(
      JSON.stringify(schedule) !== JSON.stringify(originalSchedule),
    );
  }, [schedule, originalSchedule]);

  const updateSchedule = (
    day: DayOfWeek,
    updater: (ds: DaySchedule) => DaySchedule,
  ) => {
    setSchedule((prev) =>
      prev.map((ds) => (ds.day === day ? updater(ds) : ds)),
    );
  };

  const toggleDay = (day: DayOfWeek) => {
    updateSchedule(day, (ds) => ({ ...ds, enabled: !ds.enabled }));
  };

  const addTimeSlot = (
    day: DayOfWeek,
    slot: { start: string; end: string },
  ) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      timeRanges: [...ds.timeRanges, { id: GenerateId(), ...slot }],
    }));
  };

  const addCustomTimeSlot = (day: DayOfWeek) =>
    addTimeSlot(day, { start: '09:00', end: '10:00' });

  const updateTimeRange = (
    day: DayOfWeek,
    rangeId: string,
    field: 'start' | 'end',
    value: string,
  ) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      timeRanges: ds.timeRanges.map((r) =>
        r.id === rangeId ? { ...r, [field]: value } : r,
      ),
    }));
  };

  const removeTimeRange = (day: DayOfWeek, rangeId: string) => {
    updateSchedule(day, (ds) => ({
      ...ds,
      timeRanges: ds.timeRanges.filter((r) => r.id !== rangeId),
    }));
  };

  const copyAvailability = (fromDay: DayOfWeek, toDay: DayOfWeek) => {
    const source = schedule.find((s) => s.day === fromDay);
    if (!source) return;
    updateSchedule(toDay, (ds) => ({
      ...ds,
      enabled: true,
      timeRanges: source.timeRanges.map((r) => ({ ...r, id: GenerateId() })),
    }));
  };

  const saveAvailability = async () => {
    setIsSaving(true);
    try {
      await new Promise((resolve) => setTimeout(resolve, 1000));
      setOriginalSchedule(DeepCopy(schedule));
      setSaveSuccess(true);
      setTimeout(() => setSaveSuccess(false), 3000);
    } catch (error) {
      console.error('Failed to save availability:', error);
    } finally {
      setIsSaving(false);
    }
  };

  const resetChanges = () => {
    setSchedule(DeepCopy(originalSchedule));
    setSelectedCopySource(null);
  };

  const getScheduleSummary = () => {
    const enabledDays = schedule.filter(
      (d) => d.enabled && d.timeRanges.length > 0,
    );
    const totalSlots = enabledDays.reduce(
      (sum, d) => sum + d.timeRanges.length,
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
