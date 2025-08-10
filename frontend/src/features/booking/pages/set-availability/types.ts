export const DAYS_OF_WEEK: { key: DayOfWeek; label: string; short: string }[] =
  [
    { key: 'monday', label: 'Monday', short: 'Mon' },
    { key: 'tuesday', label: 'Tuesday', short: 'Tue' },
    { key: 'wednesday', label: 'Wednesday', short: 'Wed' },
    { key: 'thursday', label: 'Thursday', short: 'Thu' },
    { key: 'friday', label: 'Friday', short: 'Fri' },
    { key: 'saturday', label: 'Saturday', short: 'Sat' },
    { key: 'sunday', label: 'Sunday', short: 'Sun' },
  ];

export const PREDEFINED_TIME_SLOTS = [
  { start: '09:00', end: '12:00', label: '9:00 AM - 11:00 AM' },
  { start: '14:00', end: '17:00', label: '2:00 PM - 3:00 PM' },
  { start: '10:00', end: '11:00', label: '10:00 AM - 11:00 AM' },
  { start: '11:00', end: '12:00', label: '11:00 AM - 12:00 PM' },
  { start: '13:00', end: '14:00', label: '1:00 PM - 2:00 PM' },
  { start: '15:00', end: '16:00', label: '3:00 PM - 4:00 PM' },
  { start: '16:00', end: '17:00', label: '4:00 PM - 5:00 PM' },
  { start: '17:00', end: '18:00', label: '5:00 PM - 6:00 PM' },
];

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

export interface TimeRange {
  id: string;
  start: string; // HH:mm format
  end: string; // HH:mm format
}

export interface DaySchedule {
  day: DayOfWeek;
  enabled: boolean;
  timeRanges: TimeRange[];
}
