import {
  Calendar,
  Clock,
  DollarSign,
  MessageCircle,
  CheckCircle,
  Edit3,
} from 'lucide-react';
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Avatar,
  AvatarImage,
  AvatarFallback,
  Badge,
  Button,
  Textarea,
  Label,
} from '@/components/ui';
import { cn } from '@/utils';
import type { BookingSummaryType } from '../types/booking-types';
import type { SessionSlotType } from '../../availability/types/availability-types';
import { useState } from 'react';

interface BookingSummaryProps {
  booking: BookingSummaryType | null;
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  notes: string;
  onNotesChange: (notes: string) => void;
  onBookSession: () => void;
  isBookingInProgress: boolean;
  isBookingDisabled: boolean;
  className?: string;
}

export function BookingSummary({
  booking,
  selectedDate,
  selectedSlot,
  notes,
  onNotesChange,
  onBookSession,
  isBookingInProgress,
  isBookingDisabled,
  className,
}: BookingSummaryProps) {
  const [isEditingNotes, setIsEditingNotes] = useState(false);

  const formatDate = (date: Date): string => {
    return date.toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

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

  const getInitials = (name: string): string => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase();
  };

  if (!booking || !selectedDate || !selectedSlot) {
    return (
      <div className={cn('space-y-6', className)}>
        <Card>
          <CardHeader>
            <CardTitle>Your Booking Summary</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="text-center py-12 text-muted-foreground">
              <Calendar className="w-16 h-16 mx-auto mb-4 text-gray-300" />
              <p className="text-lg font-medium mb-2">Select a date and time</p>
              <p className="text-sm">
                Choose your preferred date and time slot to see the booking
                summary
              </p>
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
              <span className="text-sm text-gray-600">Duration:</span>
              <span className="text-sm font-medium">30 minutes</span>
            </div>
            <div className="flex justify-between">
              <span className="text-sm text-gray-600">Format:</span>
              <span className="text-sm font-medium">Online video call</span>
            </div>
            <div className="flex justify-between">
              <span className="text-sm text-gray-600">Meeting link:</span>
              <span className="text-sm font-medium">
                Provided after booking
              </span>
            </div>
            <div className="pt-2 border-t">
              <p className="text-xs text-gray-500">
                * All sessions are 30 minutes and conducted via video call
              </p>
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
            <Calendar className="w-5 h-5" />
            Booking Summary
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          {/* Mentor Info */}
          <div className="flex items-start gap-4 p-4 bg-gradient-to-r from-gray-50 to-gray-100 rounded-lg border">
            <Avatar className="w-14 h-14">
              <AvatarImage
                src={booking.mentor.avatar}
                alt={booking.mentor.name}
              />
              <AvatarFallback className="bg-blue-100 text-blue-700 font-semibold">
                {getInitials(booking.mentor.name)}
              </AvatarFallback>
            </Avatar>
            <div className="flex-1">
              <h3 className="font-semibold text-lg">{booking.mentor.name}</h3>
              {booking.mentor.title && (
                <p className="text-sm text-muted-foreground mb-2">
                  {booking.mentor.title}
                </p>
              )}
              <div className="flex flex-wrap gap-1 mb-2">
                {booking.mentor.expertise.slice(0, 3).map((skill, index) => (
                  <Badge key={index} variant="secondary" className="text-xs">
                    {skill}
                  </Badge>
                ))}
                {booking.mentor.expertise.length > 3 && (
                  <Badge variant="secondary" className="text-xs">
                    +{booking.mentor.expertise.length - 3} more
                  </Badge>
                )}
              </div>
              <div className="text-sm">
                <span className="text-muted-foreground">Hourly Rate: </span>
                <span className="font-semibold text-green-600">
                  ${booking.mentor.hourlyRate}/hour
                </span>
              </div>
            </div>
          </div>

          {/* Session Details */}
          <div className="space-y-4">
            <h4 className="font-medium text-base flex items-center gap-2">
              <Clock className="w-4 h-4" />
              Session Details
            </h4>

            <div className="grid gap-4">
              <div className="flex items-center justify-between p-3 bg-blue-50 rounded-lg">
                <div className="flex items-center gap-3">
                  <Calendar className="w-4 h-4 text-blue-600" />
                  <div>
                    <div className="font-medium">Date</div>
                    <div className="text-sm text-muted-foreground">
                      {formatDate(selectedDate)}
                    </div>
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-between p-3 bg-green-50 rounded-lg">
                <div className="flex items-center gap-3">
                  <Clock className="w-4 h-4 text-green-600" />
                  <div>
                    <div className="font-medium">Time</div>
                    <div className="text-sm text-muted-foreground">
                      {formatTime(selectedSlot.startTime)} -{' '}
                      {getEndTime(selectedSlot.startTime)}
                    </div>
                  </div>
                </div>
                <div className="text-right">
                  <div className="font-medium">30 min</div>
                  <div className="text-xs text-muted-foreground">duration</div>
                </div>
              </div>

              <div className="flex items-center justify-between p-3 bg-purple-50 rounded-lg">
                <div className="flex items-center gap-3">
                  <DollarSign className="w-4 h-4 text-purple-600" />
                  <div>
                    <div className="font-medium">Price</div>
                    <div className="text-sm text-muted-foreground">
                      Session fee
                    </div>
                  </div>
                </div>
                <div className="text-right">
                  <div className="font-bold text-lg text-purple-600">
                    ${booking.session.price}
                  </div>
                </div>
              </div>
            </div>

            {/* Total */}
            <div className="border-t pt-4">
              <div className="flex justify-between items-center">
                <span className="text-lg font-semibold">Total</span>
                <span className="text-2xl font-bold text-green-600">
                  ${booking.total}
                </span>
              </div>
            </div>
          </div>

          {/* Notes Section */}
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <Label className="text-sm font-medium flex items-center gap-2">
                <MessageCircle className="w-4 h-4" />
                Session Notes (Optional)
              </Label>
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setIsEditingNotes(!isEditingNotes)}
              >
                <Edit3 className="w-3 h-3 mr-1" />
                {isEditingNotes ? 'Save' : 'Edit'}
              </Button>
            </div>

            {isEditingNotes ? (
              <Textarea
                placeholder="Add any specific topics you'd like to discuss or questions you have..."
                value={notes}
                onChange={(e) => onNotesChange(e.target.value)}
                className="min-h-[80px] resize-none"
                maxLength={500}
              />
            ) : (
              <div className="p-3 bg-gray-50 rounded-lg min-h-[80px] flex items-center">
                <p className="text-sm text-muted-foreground">
                  {notes ||
                    "No specific notes added. Click Edit to add topics you'd like to discuss."}
                </p>
              </div>
            )}
            {isEditingNotes && (
              <p className="text-xs text-muted-foreground">
                {notes.length}/500 characters
              </p>
            )}
          </div>
        </CardContent>
      </Card>

      {/* Booking Action */}
      <Card>
        <CardContent className="pt-6">
          <Button
            className="w-full"
            size="lg"
            onClick={onBookSession}
            disabled={isBookingDisabled || isBookingInProgress}
          >
            {isBookingInProgress ? (
              <>
                <Clock className="w-4 h-4 mr-2 animate-spin" />
                Booking Session...
              </>
            ) : (
              <>
                <CheckCircle className="w-4 h-4 mr-2" />
                Book Session - ${booking.total}
              </>
            )}
          </Button>

          <div className="mt-4 text-center">
            <p className="text-xs text-muted-foreground">
              By clicking "Book Session", you agree to our Terms of Service and
              confirm your booking. You'll receive a confirmation email with the
              meeting link.
            </p>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
