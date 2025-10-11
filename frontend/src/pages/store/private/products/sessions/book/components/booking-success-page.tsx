import { CheckCircle, ArrowLeft, Calendar, Clock, DollarSign } from 'lucide-react';
import { Button, Card, CardContent } from '@/components/ui';
import type { BookingSummaryType } from '@/api/stores';
import type { SessionSlotType } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

interface BookingSuccessPageProps {
  bookingSummary: BookingSummaryType | null;
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  onBookAnother: () => void;
  onViewBookings: () => void;
}

export function BookingSuccessPage({ bookingSummary, selectedDate, selectedSlot, onBookAnother, onViewBookings }: BookingSuccessPageProps) {
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

  return (
    <div className="container mx-auto max-w-4xl px-4 py-10">
      <div className="space-y-8 text-center">
        <div className="flex flex-col items-center space-y-4">
          <div className="flex h-24 w-24 items-center justify-center rounded-full bg-green-100">
            <CheckCircle className="h-12 w-12 text-green-600" />
          </div>
          <div>
            <h1 className="mb-2 text-4xl font-bold text-gray-900">Session Booked Successfully!</h1>
            <p className="max-w-2xl text-lg text-gray-600">
              Your mentoring session has been confirmed. You'll receive a confirmation email with the meeting details and calendar invite shortly.
            </p>
          </div>
        </div>

        {/* Booking Summary Card */}
        {bookingSummary && selectedDate && selectedSlot && (
          <Card className="mx-auto max-w-md">
            <CardContent className="pt-6">
              <div className="space-y-4">
                <div className="border-b pb-4 text-center">
                  <h3 className="text-lg font-semibold">Booking Confirmation</h3>
                  <p className="text-sm text-gray-600">Session Details</p>
                </div>

                <div className="space-y-3">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-blue-600" />
                      <span className="text-sm text-gray-600">Mentor:</span>
                    </div>
                    {/* <span className="text-sm font-medium">{bookingSummary.mentor.name}</span> */}
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-blue-600" />
                      <span className="text-sm text-gray-600">Date:</span>
                    </div>
                    <span className="text-sm font-medium">
                      {selectedDate.toLocaleDateString('en-US', {
                        weekday: 'long',
                        month: 'long',
                        day: 'numeric',
                        year: 'numeric',
                      })}
                    </span>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Clock className="h-4 w-4 text-green-600" />
                      <span className="text-sm text-gray-600">Time:</span>
                    </div>
                    <span className="text-sm font-medium">{formatTime(selectedSlot.startTime)} (30 min)</span>
                  </div>

                  <div className="flex items-center justify-between border-t pt-3">
                    <div className="flex items-center gap-2">
                      <DollarSign className="h-4 w-4 text-green-600" />
                      <span className="font-medium">Total Paid:</span>
                    </div>
                    {/* <span className="text-lg font-bold text-green-600">${bookingSummary.total}</span> */}
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>
        )}

        {/* Next Steps */}
        <div className="mx-auto max-w-2xl rounded-lg bg-blue-50 p-6">
          <h3 className="mb-3 text-lg font-semibold">What's Next?</h3>
          <ul className="space-y-2 text-left text-sm text-gray-700">
            <li>✅ Check your email for the confirmation and meeting details</li>
            <li>✅ Add the session to your calendar using the provided invite</li>
            <li>✅ Prepare any questions or topics you'd like to discuss</li>
            <li>✅ Join the video call 5 minutes before the scheduled time</li>
          </ul>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col justify-center gap-4 sm:flex-row">
          <Button onClick={onViewBookings} size="lg">
            View My Bookings
          </Button>
          <Button variant="outline" onClick={onBookAnother} size="lg">
            <ArrowLeft className="mr-2 h-4 w-4" />
            Book Another Session
          </Button>
        </div>
      </div>
    </div>
  );
}
