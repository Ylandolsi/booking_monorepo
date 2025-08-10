import { Clock } from 'lucide-react';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from '@/components/ui';
import { cn } from '@/utils';
import type { TimeSlot } from '../types/availability-types';

interface TimeSlotsProps {
  selectedDate: string;
  timeSlots: TimeSlot[];
  selectedSlot?: TimeSlot | null;
  onSlotSelect: (slot: TimeSlot) => void;
  isLoading?: boolean;
  className?: string;
}

export function TimeSlots({
  selectedDate,
  timeSlots,
  selectedSlot,
  onSlotSelect,
  isLoading = false,
  className,
}: TimeSlotsProps) {
  const formatDate = (dateString: string): string => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  const formatTime = (timeString: string): string => {
    // Assuming timeString is in ISO format, extract time
    const date = new Date(timeString);
    return date.toLocaleTimeString('en-US', {
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
          <div className="space-y-2">
            {[...Array(4)].map((_, i) => (
              <div key={i} className="h-10 bg-gray-100 animate-pulse rounded" />
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
          <div className="text-center py-8 text-muted-foreground">
            Please select a date to see available time slots
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
          <div className="text-center py-8 text-muted-foreground">
            <Clock className="w-12 h-12 mx-auto mb-4 text-gray-400" />
            <p className="text-lg font-medium mb-2">No available times</p>
            <p className="text-sm">
              Please select a different date or check back later
            </p>
          </div>
        ) : (
          <div className="space-y-2 max-h-80 overflow-y-auto">
            {availableSlots.map((slot) => {
              const isSelected = selectedSlot?.id === slot.id;
              return (
                <Button
                  key={slot.id}
                  variant={isSelected ? 'default' : 'outline'}
                  className={cn(
                    'w-full justify-start text-left h-auto p-3',
                    isSelected && 'bg-blue-600 text-white hover:bg-blue-700',
                    !isSelected && 'hover:bg-blue-50 hover:border-blue-200',
                  )}
                  onClick={() => onSlotSelect(slot)}
                >
                  <div className="flex items-center justify-between w-full">
                    <div>
                      <div className="font-medium">
                        {formatTime(slot.start)} - {formatTime(slot.end)}
                      </div>
                      <div className="text-sm opacity-70">
                        30 minutes session
                      </div>
                    </div>
                    <div className="text-right">
                      <div className="text-sm font-medium">$50</div>
                      <div className="text-xs opacity-70">per session</div>
                    </div>
                  </div>
                </Button>
              );
            })}
          </div>
        )}

        {availableSlots.length > 0 && (
          <div className="mt-4 p-3 bg-blue-50 rounded-lg border border-blue-200">
            <div className="flex items-start gap-2">
              <Clock className="w-4 h-4 text-blue-600 mt-0.5" />
              <div className="text-sm text-blue-800">
                <p className="font-medium">Session Details:</p>
                <p>• Duration: 30 minutes (fixed)</p>
                <p>• Price: $50 per session</p>
                <p>• Online video call via meeting link</p>
              </div>
            </div>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
