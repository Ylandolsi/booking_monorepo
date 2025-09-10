import { default as dayjs } from 'dayjs';

export const toLocalISOString = (date: Date): string => {
  // convert Date to this format and keep it local ! "2025-09-05 23:07:23"
  const pad = (n: number) => String(n).padStart(2, '0');
  const padMs = (n: number) => String(n).padStart(3, '0');

  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}.${padMs(date.getMilliseconds())}`;
};

export const timeStampFormat = (date: number) => dayjs(date).format('MMMM D, YYYY h:mm A');

// convert UTC TO Local date
export const formatDate = (date: Date | string): string => {
  // Sep 10, 2025
  const dateObj = date instanceof Date ? date : new Date(date);
  return dateObj.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};

export const formatDateLong = (date: Date): string => {
  // Wednesday, September 10, 2025
  return date.toLocaleDateString('en-US', {
    weekday: 'long',
    month: 'long',
    year: 'numeric',
    day: 'numeric',
  });
};

export function GenerateTimeZoneId() {
  return Intl.DateTimeFormat().resolvedOptions().timeZone;
}
export function ConvertUtcToLocal(timeZoneId?: string) {
  const localTime = new Date().toLocaleString('en-US', {
    timeZone: timeZoneId ?? GenerateTimeZoneId(),
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    timeZoneName: 'short',
  });

  return localTime;
}

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

export const formatISODateTime = (iso: string): string => {
  return dayjs(iso).format('MMMM D, YYYY h:mm A');
  // from : 2025-08-30T10:45:00
  // to : August 30, 2025 10:45 AM
};

// ----------
// "en-US" = U.S. English formatting.

// Example: August 25, 2025, 08:45:12 PM GMT+2

// "en-GB" = U.K. English formatting.

// Example: 25 August 2025 at 20:45:12 GMT+2

// "fr-FR" = French formatting.

// Example: 25 août 2025 à 20:45:12 UTC+2
