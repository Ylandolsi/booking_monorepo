import { Clock } from 'lucide-react';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from '@/components/ui';
import { formatDate, formatTime } from '@/utils';
import type { SessionSlotType } from '@/features/app/session/booking/shared';
import { cn } from '@/lib/cn';

interface TimeSlotsProps {
  selectedDate: Date | undefined;
  timeSlots: SessionSlotType[];
  selectedSlot?: SessionSlotType | null;
  onSlotSelect: (slot: SessionSlotType) => void;
  isLoading?: boolean;
  className?: string;
  mentorRate?: number;
}

export function TimeSlots({
  selectedDate,
  timeSlots,
  selectedSlot,
  onSlotSelect,
  isLoading = false,
  className,
  mentorRate = 50,
}: TimeSlotsProps) {
  const getEndTime = (startTime: string): string => {
    const [hours, minutes] = startTime.split(':');
    const time = new Date();
    time.setHours(parseInt(hours), parseInt(minutes));
    time.setMinutes(time.getMinutes() + 30); // Add 30 minutes

    return time.toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });
  };

  if (isLoading) {
    return (
      <Card className={cn('w-full', className)}>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Clock className="w-5 h-5" />
            Available Times
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            {[...Array(4)].map((_, i) => (
              <div
                key={i}
                className="h-16 bg-gray-100 animate-pulse rounded-lg"
              />
            ))}
          </div>
        </CardContent>
      </Card>
    );
  }

  if (!selectedDate) {
    return (
      <Card className={cn('w-full', className)}>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Clock className="w-5 h-5" />
            Available Times
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-center py-12 text-muted-foreground">
            <Clock className="w-16 h-16 mx-auto mb-4 text-gray-300" />
            <p className="text-lg font-medium mb-2">Select a date first</p>
            <p className="text-sm">
              Choose your preferred date from the calendar to see available time
              slots
            </p>
          </div>
        </CardContent>
      </Card>
    );
  }

  const availableSlots = timeSlots.filter(
    (slot) => slot.isAvailable && !slot.isBooked,
  );

  return (
    <Card className={cn('w-full', className)}>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Clock className="w-5 h-5" />
          Available Times
        </CardTitle>
        <p className="text-sm text-muted-foreground">
          {formatDate(selectedDate)}
        </p>
      </CardHeader>
      <CardContent>
        {availableSlots.length === 0 ? (
          <div className="text-center py-12 text-muted-foreground">
            <Clock className="w-16 h-16 mx-auto mb-4 text-gray-300" />
            <p className="text-lg font-medium mb-2">No available times</p>
            <p className="text-sm">
              This mentor has no available slots for the selected date.
              <br />
              Please choose a different date or check back later.
            </p>
          </div>
        ) : (
          <>
            <div
              className="space-y-3 max-h-80 overflow-y-auto pr-2 pb-5"
              style={{
                maskImage:
                  'linear-gradient(to bottom, black 90%, transparent 100%)',
                WebkitMaskImage:
                  'linear-gradient(to bottom, black 90%, transparent 100%)',
              }}
            >
              {availableSlots.map((slot, index) => {
                const isSelected = selectedSlot?.startTime === slot.startTime;
                const startTimeFormatted = formatTime(slot.startTime);
                const endTimeFormatted = getEndTime(slot.startTime);

                return (
                  <Button
                    key={`${slot.startTime}-${index}`}
                    variant={isSelected ? 'default' : 'outline'}
                    className={cn(
                      'w-full justify-between text-left h-auto p-4 transition-all duration-200',
                      isSelected &&
                        'bg-primary/90 text-white hover:bg-primary shadow-md',
                      !isSelected &&
                        'hover:bg-primary/20 hover:border-primary/20 hover:shadow-sm',
                    )}
                    onClick={() => onSlotSelect(slot)}
                  >
                    <div className="flex items-center gap-3">
                      <div
                        className={cn(
                          'w-2 h-2 rounded-full',
                          isSelected ? 'bg-white' : 'bg-green-500',
                        )}
                      />
                      <div>
                        <div className="font-semibold text-base">
                          {startTimeFormatted} - {endTimeFormatted}
                        </div>
                        <div
                          className={cn(
                            'text-sm',
                            isSelected
                              ? 'text-blue-100'
                              : 'text-muted-foreground',
                          )}
                        >
                          30 minutes session
                        </div>
                      </div>
                    </div>
                    <div className="text-right">
                      <div className="font-bold text-lg">${mentorRate}</div>
                      <div
                        className={cn(
                          'text-xs',
                          isSelected
                            ? 'text-blue-100'
                            : 'text-muted-foreground',
                        )}
                      >
                        per session
                      </div>
                    </div>
                  </Button>
                );
              })}
            </div>
          </>
        )}
      </CardContent>
    </Card>
  );
}
