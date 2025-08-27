export const DAYS_OF_WEEK: { key: DayOfWeek; label: string; short: string }[] =
  [
    { key: 'sunday', label: 'Sunday', short: 'Sun' },
    { key: 'monday', label: 'Monday', short: 'Mon' },
    { key: 'tuesday', label: 'Tuesday', short: 'Tue' },
    { key: 'wednesday', label: 'Wednesday', short: 'Wed' },
    { key: 'thursday', label: 'Thursday', short: 'Thu' },
    { key: 'friday', label: 'Friday', short: 'Fri' },
    { key: 'saturday', label: 'Saturday', short: 'Sat' },
  ];

export const PREDEFINED_TIME_SLOTS = Array.from({ length: 11 }, (_, i) => {
  // Every hour from 8am untill 6 PM
  const startHour = 8 + i;
  const endHour = startHour + 1;
  const start = startHour.toString().padStart(2, '0') + ':00';
  const end = endHour.toString().padStart(2, '0') + ':00';
  const label = `${startHour <= 12 ? startHour : startHour - 12}:00 ${startHour < 12 ? 'AM' : 'PM'} - ${endHour <= 12 ? endHour : endHour - 12}:00 ${endHour < 12 ? 'AM' : 'PM'}`;
  return { start, end, label };
});

export const TIME_OPTIONS = Array.from({ length: 24 }, (_, i) => {
  const hour = i.toString().padStart(2, '0');
  return `${hour}:00`;
});

export type DayOfWeek =
  | 'monday'
  | 'tuesday'
  | 'wednesday'
  | 'thursday'
  | 'friday'
  | 'saturday'
  | 'sunday';

export type SessionSlotType = {
  startTime: string; // 16:00
  endTime: string; // 16:00
  isAvailable: boolean;
  isBooked: boolean;
};
