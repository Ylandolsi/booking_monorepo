import { Calendar, Clock, DollarSign } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle, Button, DrawerDialog } from '@/components/ui';
import { cn } from '@/lib/cn';
import { formatDateLong } from '@/lib';
import { useState } from 'react';
import type { BookingSummaryType } from '@/api/stores';
import type { SessionSlotType } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

interface BookingSummaryProps {
  booking: BookingSummaryType | null;
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  onBookSession: () => void;
  isBookingInProgress: boolean;
  isBookingDisabled: boolean;
  className?: string;
}

export function BookingSummary({ booking, selectedDate, selectedSlot, onBookSession, isBookingInProgress, className }: BookingSummaryProps) {
  const [isConfirmModalOpen, setIsConfirmModalOpen] = useState(false);

  const formatTime = (timeString: string): string => {
    const [hours, minutes] = timeString.split(':');
    const time = new Date();
    time.setHours(parseInt(hours), parseInt(minutes));

    return time.toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });
  };

  const getEndTime = (startTime: string): string => {
    const [hours, minutes] = startTime.split(':');
    const time = new Date();
    time.setHours(parseInt(hours), parseInt(minutes));
    time.setMinutes(time.getMinutes() + 30);

    return time.toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });
  };

  const handleBookSessionClick = () => {
    setIsConfirmModalOpen(true);
  };

  const handleConfirmBooking = () => {
    setIsConfirmModalOpen(false);
    onBookSession();
  };

  if (!booking || !selectedDate || !selectedSlot) {
    return (
      <div className={cn('space-y-6', className)}>
        <Card>
          <CardHeader>
            <CardTitle>Your Booking Summary</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-muted-foreground py-12 text-center">
              <Calendar className="mx-auto mb-4 h-16 w-16 text-gray-300" />
              <p className="mb-2 text-lg font-medium">Select a date and time</p>
              <p className="text-sm">Choose your preferred date and time slot to see the booking summary</p>
            </div>
          </CardContent>
        </Card>

        {/* Session Details Info */}
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Session Information</CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <div className="flex justify-between">
              <span className="text-primary text-sm">Duration:</span>
              <span className="text-sm font-medium">30 minutes</span>
            </div>
            <div className="flex justify-between">
              <span className="text-primary text-sm">Format:</span>
              <span className="text-sm font-medium">Online video call</span>
            </div>
            <div className="flex justify-between">
              <span className="text-primary text-sm">Meeting link:</span>
              <span className="text-sm font-medium">Provided after booking</span>
            </div>
            <div className="border-t pt-2">
              <p className="text-xs text-gray-500">* All sessions are 30 minutes and conducted via video call</p>
            </div>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className={cn('space-y-6', className)}>
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Calendar className="h-5 w-5" />
            Booking Summary
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          {/* Session Details */}
          <div className="space-y-4">
            <h4 className="flex items-center gap-2 text-base font-medium">
              <Clock className="h-4 w-4" />
              Session Details
            </h4>
            <div className="grid gap-4">
              <div className="flex items-center justify-between rounded-lg bg-blue-50 p-3">
                <div className="text-background flex items-center gap-3">
                  <Calendar className="h-4 w-4 text-blue-600" />
                  <div>
                    <div className="font-medium">Date</div>
                    <div className="text-sm">{formatDateLong(selectedDate)}</div>
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-between rounded-lg bg-green-50 p-3">
                <div className="text-background flex items-center gap-3">
                  <Clock className="h-4 w-4 text-green-600" />
                  <div>
                    <div className="font-medium">Time</div>
                    <div className="text-sm">
                      {formatTime(selectedSlot.startTime)} - {getEndTime(selectedSlot.startTime)}
                    </div>
                  </div>
                </div>
                <div className="text-background text-right">
                  <div className="font-medium">30 min</div>
                  <div className="text-xs">duration</div>
                </div>
              </div>

              <div className="flex items-center justify-between rounded-lg bg-purple-50 p-3">
                <div className="text-background flex items-center gap-3">
                  <DollarSign className="h-4 w-4 text-purple-600" />
                  <div>
                    <div className="font-medium">Price</div>
                    <div className="text-sm">Session fee</div>
                  </div>
                </div>
                <div className="text-right">
                  <div className="text-lg font-bold text-purple-600">${booking.session.price}</div>
                </div>
              </div>
            </div>
            {/* Total */}
            <div className="border-t pt-4">
              <div className="flex items-center justify-between">
                <span className="text-lg font-semibold">Total</span>
                <span className="text-2xl font-bold text-green-600">${booking.session.price}</span>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Booking Action */}
      {/* <Card>
        <CardContent className="pt-6">
          <Button className="w-full" size="lg" onClick={handleBookSessionClick} disabled={isBookingDisabled || isBookingInProgress}>
            {isBookingInProgress ? (
              <>
                <Clock className="mr-2 h-4 w-4 animate-spin" />
                Booking Session...
              </>
            ) : (
              <>
                <CheckCircle className="mr-2 h-4 w-4" />
                Book Session - ${booking.session.price}
              </>
            )}
          </Button>

          <div className="mt-4 text-center">
            <p className="text-muted-foreground text-xs">
              By clicking "Book Session", you agree to our Terms of Service and confirm your booking. You'll receive a confirmation email with the
              meeting link.
            </p>
          </div>
        </CardContent>
      </Card> */}

      {/* Confirmation Modal */}
      <DrawerDialog
        open={isConfirmModalOpen}
        onOpenChange={setIsConfirmModalOpen}
        title="Confirm Booking"
        description="Please confirm the session title and notes before proceeding with the booking."
      >
        <div className="flex gap-2">
          <Button variant="outline" onClick={() => setIsConfirmModalOpen(false)}>
            Cancel
          </Button>
          <Button onClick={handleConfirmBooking} disabled={isBookingInProgress}>
            Confirm Booking
          </Button>
        </div>
      </DrawerDialog>
    </div>
  );
}
