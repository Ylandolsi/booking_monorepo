import { useState } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from '@/components/ui';
import { cn } from '@/utils';

interface CalendarProps {
  selectedDate?: string; // YYYY-MM-DD format
  onDateSelect: (date: string) => void;
  availableDates?: string[]; // Array of available dates in YYYY-MM-DD format
  className?: string;
}

export function Calendar({
  selectedDate,
  onDateSelect,
  availableDates = [],
  className,
}: CalendarProps) {
  const [currentDate, setCurrentDate] = useState(new Date());

  const year = currentDate.getFullYear();
  const month = currentDate.getMonth();

  // Get first day of the month and number of days
  const firstDayOfMonth = new Date(year, month, 1);
  const lastDayOfMonth = new Date(year, month + 1, 0);
  const firstDayWeekday = firstDayOfMonth.getDay(); // 0 = Sunday, 1 = Monday, etc.
  const daysInMonth = lastDayOfMonth.getDate();

  // Generate calendar days
  const calendarDays = [];

  // Add empty cells for days before the month starts
  for (let i = 0; i < firstDayWeekday; i++) {
    calendarDays.push(null);
  }

  // Add days of the month
  for (let day = 1; day <= daysInMonth; day++) {
    calendarDays.push(day);
  }

  const monthNames = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];

  const weekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  const navigateMonth = (direction: 'prev' | 'next') => {
    setCurrentDate((prev) => {
      const newDate = new Date(prev);
      if (direction === 'prev') {
        newDate.setMonth(newDate.getMonth() - 1);
      } else {
        newDate.setMonth(newDate.getMonth() + 1);
      }
      return newDate;
    });
  };

  const formatDateString = (day: number): string => {
    return `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
  };

  const isDateAvailable = (day: number): boolean => {
    const dateString = formatDateString(day);
    return availableDates.includes(dateString);
  };

  const isDateSelected = (day: number): boolean => {
    const dateString = formatDateString(day);
    return dateString === selectedDate;
  };

  const isDateInPast = (day: number): boolean => {
    const dateString = formatDateString(day);
    const today = new Date().toISOString().split('T')[0];
    return dateString < today;
  };

  return (
    <Card className={cn('w-full max-w-md', className)}>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-4">
        <CardTitle className="text-lg font-semibold">
          {monthNames[month]} {year}
        </CardTitle>
        <div className="flex gap-1">
          <Button
            variant="outline"
            size="sm"
            onClick={() => navigateMonth('prev')}
          >
            <ChevronLeft className="w-4 h-4" />
          </Button>
          <Button
            variant="outline"
            size="sm"
            onClick={() => navigateMonth('next')}
          >
            <ChevronRight className="w-4 h-4" />
          </Button>
        </div>
      </CardHeader>
      <CardContent>
        {/* Week day headers */}
        <div className="grid grid-cols-7 gap-1 mb-2">
          {weekDays.map((day) => (
            <div
              key={day}
              className="h-8 flex items-center justify-center text-sm font-medium text-muted-foreground"
            >
              {day}
            </div>
          ))}
        </div>

        {/* Calendar grid */}
        <div className="grid grid-cols-7 gap-1">
          {calendarDays.map((day, index) => {
            if (day === null) {
              return <div key={index} className="h-10" />;
            }

            const dateString = formatDateString(day);
            const isAvailable = isDateAvailable(day);
            const isSelected = isDateSelected(day);
            const isPast = isDateInPast(day);

            return (
              <Button
                key={day}
                variant={isSelected ? 'default' : 'ghost'}
                size="sm"
                className={cn(
                  'h-10 p-0 font-normal',
                  isAvailable && !isPast
                    ? 'hover:bg-blue-100 hover:text-blue-900 border-blue-200'
                    : '',
                  isSelected && 'bg-blue-600 text-white hover:bg-blue-700',
                  isPast && 'text-gray-400 cursor-not-allowed',
                  !isAvailable && !isPast && 'text-gray-400 cursor-not-allowed',
                  isAvailable &&
                    !isPast &&
                    !isSelected &&
                    'bg-green-50 text-green-700 border border-green-200',
                )}
                disabled={!isAvailable || isPast}
                onClick={() => onDateSelect(dateString)}
              >
                {day}
              </Button>
            );
          })}
        </div>

        {/* Legend */}
        <div className="flex justify-center gap-4 mt-4 text-xs">
          <div className="flex items-center gap-1">
            <div className="w-3 h-3 bg-green-100 border border-green-200 rounded" />
            <span className="text-muted-foreground">Available</span>
          </div>
          <div className="flex items-center gap-1">
            <div className="w-3 h-3 bg-blue-600 rounded" />
            <span className="text-muted-foreground">Selected</span>
          </div>
          <div className="flex items-center gap-1">
            <div className="w-3 h-3 bg-gray-200 rounded" />
            <span className="text-muted-foreground">Unavailable</span>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
