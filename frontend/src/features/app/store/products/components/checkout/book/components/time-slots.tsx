import { Clock } from 'lucide-react';
import { Button, Card, CardContent, CardHeader, CardTitle } from '@/components/ui';
import { formatDate, formatTime } from '@/utils';
import { cn } from '@/lib/cn';
import type { SessionSlotType } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

interface TimeSlotsProps {
  selectedDate: Date | undefined;
  timeSlots: SessionSlotType[];
  selectedSlot?: SessionSlotType | null;
  onSlotSelect: (slot: SessionSlotType) => void;
  isLoading?: boolean;
  className?: string;
  mentorRate?: number;
}

export function TimeSlots({ selectedDate, timeSlots, selectedSlot, onSlotSelect, isLoading = false, className }: TimeSlotsProps) {
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
            <Clock className="h-5 w-5" />
            Available Times
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            {[...Array(4)].map((_, i) => (
              <div key={i} className="h-16 animate-pulse rounded-lg bg-gray-100" />
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
            <Clock className="h-5 w-5" />
            Available Times
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-muted-foreground py-12 text-center">
            <Clock className="mx-auto mb-4 h-16 w-16 text-gray-300" />
            <p className="mb-2 text-lg font-medium">Select a date first</p>
            <p className="text-sm">Choose your preferred date from the calendar to see available time slots</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  const availableSlots = timeSlots.filter((slot) => slot.isAvailable && !slot.isBooked);

  return (
    <Card className={cn('w-full', className)}>
      <CardHeader>
        <CardTitle className="flex flex-row flex-wrap items-start justify-start gap-5">
          <Clock className="h-5 w-5" />
          <div className="flex flex-col flex-wrap items-start gap-2">
            <p>Available Times</p>
            <p className="text-muted-foreground text-sm">{formatDate(selectedDate)}</p>
          </div>
        </CardTitle>
      </CardHeader>
      <CardContent>
        {availableSlots.length === 0 ? (
          <div className="text-muted-foreground py-12 text-center">
            <Clock className="mx-auto mb-4 h-16 w-16 text-gray-300" />
            <p className="mb-2 text-lg font-medium">No available times</p>
            <p className="text-sm">
              This mentor has no available slots for the selected date.
              <br />
              Please choose a different date or check back later.
            </p>
          </div>
        ) : (
          <>
            <div
              className="max-h-80 space-y-3 overflow-y-auto pr-2 pb-5"
              style={{
                maskImage: 'linear-gradient(to bottom, black 90%, transparent 100%)',
                WebkitMaskImage: 'linear-gradient(to bottom, black 90%, transparent 100%)',
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
                      'h-auto w-full justify-between p-4 text-left transition-all duration-200',
                      isSelected && 'bg-primary/90 hover:bg-primary text-white shadow-md',
                      !isSelected && 'hover:bg-primary/20 hover:border-primary/20 hover:shadow-sm',
                    )}
                    onClick={() => onSlotSelect(slot)}
                  >
                    <div className="flex items-center gap-3">
                      <div className={cn('h-2 w-2 rounded-full', isSelected ? 'bg-white' : 'bg-green-500')} />
                      <div>
                        <div className="text-base font-semibold">
                          {startTimeFormatted} - {endTimeFormatted}
                        </div>
                        <div className={cn('text-sm', isSelected ? 'text-blue-100' : 'text-muted-foreground')}>30 minutes session</div>
                      </div>
                    </div>
                    {/* <div className="text-right"> */}
                    {/* TODO : only for 30 minutes */}
                    {/* <div className="text-lg font-bold">$</div> */}
                    {/* <div className={cn('text-xs', isSelected ? 'text-blue-100' : 'text-muted-foreground')}>per session</div> */}
                    {/* </div> */}
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
