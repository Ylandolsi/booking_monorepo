import { Calendar, Clock, DollarSign, MessageCircle, CheckCircle, Edit3 } from 'lucide-react';
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
  DrawerDialog,
  Input,
} from '@/components/ui';
import { cn } from '@/lib/cn';
import { formatDateLong } from '@/utils';
import type { BookingSummaryType } from '../types/booking-types';
import { useState } from 'react';
import type { SessionSlotType } from '@/features/app/session/booking/shared';

interface BookingSummaryProps {
  booking: BookingSummaryType | null;
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  notes: string;
  onNotesChange: (notes: string) => void;
  title: string;
  onTitleChange: (title: string) => void; // Fixed parameter name

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
  title,
  onTitleChange,
  onBookSession,
  isBookingInProgress,
  isBookingDisabled,
  className,
}: BookingSummaryProps) {
  const [isEditingNotes, setIsEditingNotes] = useState(false);
  const [isEditingTitle, setIsEditingTitle] = useState(false);
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

  const getInitials = (name: string): string => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase();
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
              <span className="text-sm text-gray-600">Duration:</span>
              <span className="text-sm font-medium">30 minutes</span>
            </div>
            <div className="flex justify-between">
              <span className="text-sm text-gray-600">Format:</span>
              <span className="text-sm font-medium">Online video call</span>
            </div>
            <div className="flex justify-between">
              <span className="text-sm text-gray-600">Meeting link:</span>
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
                <div className="flex items-center gap-3">
                  <Calendar className="h-4 w-4 text-blue-600" />
                  <div>
                    <div className="font-medium">Date</div>
                    <div className="text-muted-foreground text-sm">{formatDateLong(selectedDate)}</div>
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-between rounded-lg bg-green-50 p-3">
                <div className="flex items-center gap-3">
                  <Clock className="h-4 w-4 text-green-600" />
                  <div>
                    <div className="font-medium">Time</div>
                    <div className="text-muted-foreground text-sm">
                      {formatTime(selectedSlot.startTime)} - {getEndTime(selectedSlot.startTime)}
                    </div>
                  </div>
                </div>
                <div className="text-right">
                  <div className="font-medium">30 min</div>
                  <div className="text-muted-foreground text-xs">duration</div>
                </div>
              </div>

              <div className="flex items-center justify-between rounded-lg bg-purple-50 p-3">
                <div className="flex items-center gap-3">
                  <DollarSign className="h-4 w-4 text-purple-600" />
                  <div>
                    <div className="font-medium">Price</div>
                    <div className="text-muted-foreground text-sm">Session fee</div>
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
                <span className="text-2xl font-bold text-green-600">${booking.total}</span>
              </div>
            </div>
          </div>

          {/* Session Title Section */}

          {/* Notes Section */}
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <Label className="flex items-center gap-2 text-sm font-medium">
                <MessageCircle className="h-4 w-4" />
                Session Notes (Optional)
              </Label>
              <Button variant="ghost" size="sm" onClick={() => setIsEditingNotes(!isEditingNotes)}>
                <Edit3 className="mr-1 h-3 w-3" />
                {isEditingNotes ? 'Save' : 'Edit'}
              </Button>
            </div>

            {isEditingNotes ? (
              <Textarea
                placeholder="Add any specific topics you'd like to discuss or questions you have..."
                value={notes}
                onChange={(e) => onNotesChange(e.target.value)}
                className="min-h-[80px] max-w-[20px] resize-none"
                maxLength={500}
              />
            ) : (
              <div className="flex min-h-[80px] max-w-full min-w-0 items-center rounded-lg p-3">
                <p className="text-muted-foreground w-full text-sm text-wrap break-words">
                  {notes || "No specific notes added. Click Edit to add topics you'd like to discuss."}
                </p>
              </div>
            )}
            {isEditingNotes && <p className="text-muted-foreground text-xs">{notes.length}/500 characters</p>}
          </div>
        </CardContent>
      </Card>

      {/* Booking Action */}
      <Card>
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
                Book Session - ${booking.total}
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
      </Card>

      {/* Confirmation Modal */}
      <DrawerDialog
        open={isConfirmModalOpen}
        onOpenChange={setIsConfirmModalOpen}
        title="Confirm Booking"
        description="Please confirm the session title and notes before proceeding with the booking."
      >
        <div className="space-y-6 py-4">
          {/* Session Title in Modal */}
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <Label className="flex items-center gap-2 text-sm font-medium">
                <MessageCircle className="h-4 w-4" />
                Session Title
              </Label>
              <Button variant="ghost" size="sm" onClick={() => setIsEditingTitle(!isEditingTitle)}>
                <Edit3 className="mr-1 h-3 w-3" />
                {isEditingTitle ? 'Save' : 'Edit'}
              </Button>
            </div>

            {isEditingTitle ? (
              <Input placeholder="Enter a title for your session..." value={title} onChange={(e) => onTitleChange(e.target.value)} />
            ) : (
              <div className="flex min-h-[40px] items-center rounded-lg bg-gray-50 p-3">
                <p className="text-muted-foreground text-sm">{title || 'No title added. Click Edit to add a session title.'}</p>
              </div>
            )}
          </div>

          {/* Session Notes in Modal */}
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <Label className="flex items-center gap-2 text-sm font-medium">
                <MessageCircle className="h-4 w-4" />
                Session Notes (Optional)
              </Label>
              <Button variant="ghost" size="sm" onClick={() => setIsEditingNotes(!isEditingNotes)}>
                <Edit3 className="mr-1 h-3 w-3" />
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
              <div className="flex min-h-[80px] items-center rounded-lg bg-gray-50 p-3">
                <p className="text-muted-foreground text-sm">{notes || "No specific notes added. Click Edit to add topics you'd like to discuss."}</p>
              </div>
            )}
            {isEditingNotes && <p className="text-muted-foreground text-xs">{notes.length}/500 characters</p>}
          </div>
        </div>
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
