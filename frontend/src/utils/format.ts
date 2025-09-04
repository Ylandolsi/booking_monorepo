import { default as dayjs } from 'dayjs';

export const timeStampFormat = (date: number) =>
  dayjs(date).format('MMMM D, YYYY h:mm A');

// convert UTC TO Local date
export const formatDate = (date: Date | string): string => {
  const dateObj = date instanceof Date ? date : new Date(date);
  return dateObj.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};

export const formatTime = (timeString: string): string => {
  // Parse HH:mm format
  const [hours, minutes] = timeString.split(':');
  const time = new Date();
  time.setHours(parseInt(hours), parseInt(minutes));

  return time.toLocaleTimeString('en-US', {
    hour: '2-digit',
    minute: '2-digit',
    hour12: true,
  });
};

export const formatTimeRange = (start: string, end: string) => {
  return `${formatTime(start)} - ${formatTime(end)}`;
};
// const formatDate = (dateStr: string | undefined) => {

export const formatISODateTime = (iso: string): string => {
  return dayjs(iso).format('MMMM D, YYYY h:mm A');
  // from : 2025-08-30T10:45:00
  // to : August 30, 2025 10:45 AM
};
