import { default as dayjs } from 'dayjs';

export const timeStampFormat = (date: number) =>
  dayjs(date).format('MMMM D, YYYY h:mm A');

export const formatDate = (date: Date | string): string => {
  const dateObj = date instanceof Date ? date : new Date(date);
  return dateObj.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
  });
}