import {
  CheckCircle,
  ArrowLeft,
  Calendar,
  Clock,
  DollarSign,
} from 'lucide-react';
import { Button, Card, CardContent } from '@/components/ui';
import type { BookingSummary } from '../types/booking-types';
import type { SessionSlot } from '../types/availability-types';

interface BookingSuccessPageProps {
  bookingSummary: BookingSummary | null;
  selectedDate: Date | undefined;
  selectedSlot: SessionSlot | null;
  onBookAnother: () => void;
  onViewBookings: () => void;
}

export function BookingSuccessPage({
  bookingSummary,
  selectedDate,
  selectedSlot,
  onBookAnother,
  onViewBookings,
}: BookingSuccessPageProps) {
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
    <div className="container mx-auto py-10 px-4 max-w-4xl">
      <div className="text-center space-y-8">
        <div className="flex flex-col items-center space-y-4">
          <div className="w-24 h-24 bg-green-100 rounded-full flex items-center justify-center">
            <CheckCircle className="w-12 h-12 text-green-600" />
          </div>
          <div>
            <h1 className="text-4xl font-bold text-gray-900 mb-2">
              Session Booked Successfully!
            </h1>
            <p className="text-lg text-gray-600 max-w-2xl">
              Your mentoring session has been confirmed. You'll receive a
              confirmation email with the meeting details and calendar invite
              shortly.
            </p>
          </div>
        </div>

        {/* Booking Summary Card */}
        {bookingSummary && selectedDate && selectedSlot && (
          <Card className="max-w-md mx-auto">
            <CardContent className="pt-6">
              <div className="space-y-4">
                <div className="text-center pb-4 border-b">
                  <h3 className="font-semibold text-lg">
                    Booking Confirmation
                  </h3>
                  <p className="text-sm text-gray-600">Session Details</p>
                </div>

                <div className="space-y-3">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Calendar className="w-4 h-4 text-blue-600" />
                      <span className="text-sm text-gray-600">Mentor:</span>
                    </div>
                    <span className="text-sm font-medium">
                      {bookingSummary.mentor.name}
                    </span>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Calendar className="w-4 h-4 text-blue-600" />
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
                      <Clock className="w-4 h-4 text-green-600" />
                      <span className="text-sm text-gray-600">Time:</span>
                    </div>
                    <span className="text-sm font-medium">
                      {formatTime(selectedSlot.startTime)} (30 min)
                    </span>
                  </div>

                  <div className="flex items-center justify-between border-t pt-3">
                    <div className="flex items-center gap-2">
                      <DollarSign className="w-4 h-4 text-green-600" />
                      <span className="font-medium">Total Paid:</span>
                    </div>
                    <span className="font-bold text-lg text-green-600">
                      ${bookingSummary.total}
                    </span>
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>
        )}

        {/* Next Steps */}
        <div className="bg-blue-50 rounded-lg p-6 max-w-2xl mx-auto">
          <h3 className="font-semibold text-lg mb-3">What's Next?</h3>
          <ul className="text-left space-y-2 text-sm text-gray-700">
            <li>
              ✅ Check your email for the confirmation and meeting details
            </li>
            <li>
              ✅ Add the session to your calendar using the provided invite
            </li>
            <li>✅ Prepare any questions or topics you'd like to discuss</li>
            <li>✅ Join the video call 5 minutes before the scheduled time</li>
          </ul>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <Button onClick={onViewBookings} size="lg">
            View My Bookings
          </Button>
          <Button variant="outline" onClick={onBookAnother} size="lg">
            <ArrowLeft className="w-4 h-4 mr-2" />
            Book Another Session
          </Button>
        </div>
      </div>
    </div>
  );
}
