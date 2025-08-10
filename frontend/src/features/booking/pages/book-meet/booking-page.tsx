import { useState, useEffect } from 'react';
import { useParams } from '@tanstack/react-router';
import { Calendar, Clock, User, CheckCircle } from 'lucide-react';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Alert,
  AlertDescription,
  Badge,
  Avatar,
  AvatarImage,
  AvatarFallback,
  Spinner,
  Calendar as BookingCalendar,
} from '@/components/ui';
import { TimeSlots } from '../../components/time-slots';
import { BookingSummaryCard } from '../../components/booking-summary';
import { useBookSession } from '../../api/book-session-api';
import {
  useMonthlyAvailability,
  useDailyAvailability,
} from '../set-availability/api/availability-api';
import { useMentorDetails } from '@/features/mentor/api';
import type { TimeSlot } from '../../types/availability-types';
import type { BookingSummary } from '../../types/booking-types';

export function BookingPage() {
  const { mentorSlug } = useParams({ strict: false }) as {
    mentorSlug?: string;
  };

  // State management
  const [selectedDate, setSelectedDate] = useState<string | null>(null);
  const [selectedSlot, setSelectedSlot] = useState<TimeSlot | null>(null);
  const [bookingStep, setBookingStep] = useState<'select' | 'success'>(
    'select',
  );

  // Current month/year for calendar
  const currentDate = new Date();
  const currentMonth = currentDate.getMonth() + 1;
  const currentYear = currentDate.getFullYear();

  // API calls
  const mentorQuery = useMentorDetails(mentorSlug);
  const monthlyAvailabilityQuery = useMonthlyAvailability(
    mentorSlug,
    currentYear,
    currentMonth,
  );
  const dailyAvailabilityQuery = useDailyAvailability(
    mentorSlug,
    selectedDate || undefined,
  );
  const bookSessionMutation = useBookSession();

  // Extract available dates from monthly data
  const availableDates =
    monthlyAvailabilityQuery.data?.days
      ?.filter((day: any) =>
        day.slots.some((slot: any) => slot.isAvailable && !slot.isBooked),
      )
      ?.map((day: any) => day.date) || [];

  // Reset selected slot when date changes
  useEffect(() => {
    setSelectedSlot(null);
  }, [selectedDate]);

  // Handle booking confirmation
  const handleBookSession = async () => {
    if (!selectedDate || !selectedSlot || !mentorSlug) return;

    try {
      await bookSessionMutation.mutateAsync({
        mentorSlug,
        date: selectedDate,
        startTime: new Date(selectedSlot.start).toLocaleTimeString('en-US', {
          hour12: false,
          hour: '2-digit',
          minute: '2-digit',
        }),
        duration: 30,
        notes: '',
      });
      setBookingStep('success');
    } catch (error) {
      console.error('Booking failed:', error);
    }
  };

  // Create booking summary data
  const createBookingSummary = (): BookingSummary | null => {
    if (!mentorQuery.data || !selectedDate || !selectedSlot) return null;

    return {
      mentor: {
        slug: mentorSlug || '',
        name: `Professional Mentor`,
        avatar: undefined,
        title: 'Experienced Mentor',
        expertise: ['General Mentoring'],
        hourlyRate: mentorQuery.data.hourlyRate || 50,
      },
      session: {
        date: selectedDate,
        time: new Date(selectedSlot.start).toLocaleTimeString('en-US', {
          hour: '2-digit',
          minute: '2-digit',
          hour12: true,
        }),
        duration: 30,
        price: mentorQuery.data.hourlyRate || 50,
        currency: '$',
      },
      total: mentorQuery.data.hourlyRate || 50,
    };
  };

  const bookingSummary = createBookingSummary();

  // Loading states
  if (mentorQuery.isLoading || monthlyAvailabilityQuery.isLoading) {
    return (
      <div className="container mx-auto py-10 px-4 max-w-4xl">
        <div className="flex items-center justify-center min-h-[400px]">
          <div className="text-center space-y-4">
            <Spinner className="w-8 h-8 mx-auto" />
            <p className="text-lg text-gray-600">Loading booking page...</p>
          </div>
        </div>
      </div>
    );
  }

  // Error states
  if (mentorQuery.isError) {
    return (
      <div className="container mx-auto py-10 px-4 max-w-4xl">
        <Alert variant="destructive">
          <AlertDescription>
            Failed to load mentor details. Please try again later.
          </AlertDescription>
        </Alert>
      </div>
    );
  }

  // Success page
  if (bookingStep === 'success') {
    return (
      <div className="container mx-auto py-10 px-4 max-w-4xl">
        <div className="text-center space-y-6">
          <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto">
            <CheckCircle className="w-10 h-10 text-green-600" />
          </div>
          <div>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">
              Session Booked Successfully!
            </h1>
            <p className="text-lg text-gray-600">
              Your mentoring session has been confirmed. You'll receive an email
              with the meeting details.
            </p>
          </div>

          {bookingSummary && (
            <div className="max-w-md mx-auto">
              <BookingSummaryCard booking={bookingSummary} />
            </div>
          )}

          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Button onClick={() => (window.location.href = '/app/bookings')}>
              View My Bookings
            </Button>
            <Button
              variant="outline"
              onClick={() => {
                setBookingStep('select');
                setSelectedDate(null);
                setSelectedSlot(null);
              }}
            >
              Book Another Session
            </Button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-10 px-4 max-w-7xl">
      {/* Header */}
      <div className="text-center space-y-6 mb-8">
        <div>
          <h1 className="text-4xl font-bold text-gray-900 mb-4">
            Book a Session with Your Mentor
          </h1>
          <p className="text-lg text-gray-600 max-w-3xl mx-auto">
            Connect with experienced mentors across various fields and schedule
            personalized sessions to accelerate your growth and achieve your
            goals.
          </p>
        </div>

        {/* How it works */}
      </div>

      {/* Mentor Profile */}
      {mentorQuery.data && (
        <Card className="mb-8">
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <User className="w-5 h-5" />
              Mentor Profile
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-start gap-4">
              <Avatar className="w-16 h-16">
                <AvatarImage src={undefined} alt="Mentor Avatar" />
                <AvatarFallback>M</AvatarFallback>
              </Avatar>
              <div className="flex-1">
                <h3 className="text-xl font-semibold mb-1">
                  Professional Mentor
                </h3>
                <p className="text-gray-600 mb-2">Experienced Mentor</p>
                <div className="flex flex-wrap gap-2 mb-3">
                  {['General Mentoring'].map((skill, index) => (
                    <Badge key={index} variant="secondary">
                      {skill}
                    </Badge>
                  ))}
                </div>
                <div className="text-sm text-gray-600">
                  <span className="font-medium">Hourly Rate: </span>
                  <span className="text-lg font-semibold text-green-600">
                    ${mentorQuery.data.hourlyRate || 50}/hour
                  </span>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Calendar and Time Slots */}
        <div className="lg:col-span-2 space-y-6">
          {/* Calendar */}
          <BookingCalendar
            selectedDate={selectedDate || undefined}
            onDateSelect={setSelectedDate}
            availableDates={availableDates}
          />

          {/* Time Slots */}
          {selectedDate && (
            <div>
              {dailyAvailabilityQuery.isLoading ? (
                <Card>
                  <CardHeader>
                    <CardTitle className="flex items-center gap-2">
                      <Clock className="w-5 h-5" />
                      Available Times
                    </CardTitle>
                  </CardHeader>
                  <CardContent>
                    <div className="space-y-2">
                      {[...Array(4)].map((_, i) => (
                        <div
                          key={i}
                          className="h-10 bg-gray-100 animate-pulse rounded"
                        />
                      ))}
                    </div>
                  </CardContent>
                </Card>
              ) : (
                <TimeSlots
                  selectedDate={selectedDate}
                  timeSlots={dailyAvailabilityQuery.data?.slots || []}
                  selectedSlot={selectedSlot}
                  onSlotSelect={setSelectedSlot}
                />
              )}
            </div>
          )}
        </div>

        {/* Booking Summary */}
        <div className="space-y-6">
          {bookingSummary ? (
            <>
              <BookingSummaryCard booking={bookingSummary} />

              {/* Booking Actions */}
              <Card>
                <CardContent className="pt-6">
                  <Button
                    className="w-full"
                    size="lg"
                    onClick={handleBookSession}
                    disabled={
                      !selectedDate ||
                      !selectedSlot ||
                      bookSessionMutation.isPending
                    }
                  >
                    {bookSessionMutation.isPending ? (
                      <>
                        <Clock className="w-4 h-4 mr-2 animate-spin" />
                        Booking Session...
                      </>
                    ) : (
                      <>
                        <CheckCircle className="w-4 h-4 mr-2" />
                        Book Session
                      </>
                    )}
                  </Button>

                  {bookSessionMutation.isError && (
                    <Alert className="mt-4" variant="destructive">
                      <AlertDescription>
                        Failed to book session. Please try again.
                      </AlertDescription>
                    </Alert>
                  )}
                </CardContent>
              </Card>
            </>
          ) : (
            <Card>
              <CardHeader>
                <CardTitle>Your Booking Summary</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-center py-8 text-muted-foreground">
                  <Calendar className="w-12 h-12 mx-auto mb-4 text-gray-400" />
                  <p className="text-lg font-medium mb-2">
                    Select a date and time
                  </p>
                  <p className="text-sm">
                    Choose your preferred date and time slot to see the booking
                    summary
                  </p>
                </div>
              </CardContent>
            </Card>
          )}

          {/* Session Details Info */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Session Details</CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              <div className="flex justify-between">
                <span className="text-sm text-gray-600">Duration:</span>
                <span className="text-sm font-medium">30 minutes</span>
              </div>
              <div className="flex justify-between">
                <span className="text-sm text-gray-600">Price:</span>
                <span className="text-sm font-medium">
                  $50 (or mentor-specific pricing)
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-sm text-gray-600">Format:</span>
                <span className="text-sm font-medium">Online video call</span>
              </div>
              <div className="pt-2 border-t">
                <p className="text-xs text-gray-500">
                  * Duration is fixed at 30 minutes for now but will be
                  customizable in the future
                </p>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
