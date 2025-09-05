import { default as dayjs } from 'dayjs';

export const  toLocalISOString = (date: Date): string => {
  // convert Date to this format and keep it local ! "2025-09-05 23:07:23"
  const pad = (n: number) => String(n).padStart(2, '0');
  const padMs = (n: number) => String(n).padStart(3, '0');

  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}.${padMs(date.getMilliseconds())}`;
}

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
