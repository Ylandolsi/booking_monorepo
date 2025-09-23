import { Calendar } from 'lucide-react';
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Alert,
  AlertDescription,
  Calendar as BookingCalendar,
  PageLoading2,
  alertIconMap,
} from '@/components/ui';
import { useIsMobile } from '@/hooks/use-mobile';
import { useAppNavigation } from '@/hooks/use-navigation';
import { ErrorComponenet, IntegrationRequired } from '@/components';
import React, { useEffect, useState } from 'react';
import { useAuth } from '@/features/auth';
import { signalRService, type NotificationSignalR } from '@/services/notification-service'; // Assuming this is the correct import path; adjust if needed
import { toast } from 'sonner';
import { useBooking } from '@/features/app/store/products/components/checkout/book/hooks';
import { BookingSummary, TimeSlots } from '@/features/app/store/products/components/checkout/book';

function BookingContent() {
  const nav = useAppNavigation();
  const isMobile = useIsMobile();
  // const { mentorSlug } = useParams({ strict: false }) as {
  //   mentorSlug?: string;
  // };
  const mentorSlug = 'mohsen-3ifa';
  const { currentUser } = useAuth();

  const iamTheMentor = currentUser?.slug == mentorSlug;
  const {
    // State
    selectedDate,
    selectedSlot,
    step,
    notes,
    title,
    // Computed values
    isLoading,
    hasError,
    availableSlots,
    bookingSummary,

    // Queries
    mentorInfoQuery,
    mentorDetailsQuery,
    monthlyAvailabilityQuery,
    bookSessionMutation,

    // Actions
    setSelectedDate,
    setSelectedSlot,
    // setStep,
    setTitle,
    setNotes,
    resetBooking,
    handleBookSession,
  } = useBooking({ mentorSlug, iamTheMentor });

  const [googleRequired, setGoogleRequired] = useState<boolean>(false);

  const sessionConfirmedMentee = (data: NotificationSignalR) => {
    toast.success(data.title, {
      description: data.message,
      duration: 5000,
      action: {
        label: 'View',
        onClick: () => {
          nav.goToMeets();
        },
      },
    });
  };

  // Initialize SignalR connection
  useEffect(() => {
    if (currentUser != null) {
      // needs to be auth
      // Start connection
      signalRService.startConnection(currentUser.slug).catch((error) => console.error('Failed to establish SignalR connection:', error));

      // Register callback
      signalRService.addCallback('session_confirmed', sessionConfirmedMentee);

      // Cleanup on unmount
      return () => {
        signalRService.removeCallback('session_confirmed', sessionConfirmedMentee);
        signalRService.stopConnection(currentUser.slug).catch((error) => console.error('Error stopping SignalR connection:', error));
      };
    }
  }, [currentUser]);

  if (iamTheMentor) {
    return <ErrorComponenet title="You can not Book with youself" message="You can not Book with youself"></ErrorComponenet>;
  }

  if (isLoading) {
    return <PageLoading2 title="Loading booking page " description="Fetching mentor details and availability" />;
  }

  if (mentorDetailsQuery.error?.message?.includes('not found')) {
    return <ErrorComponenet title="Mentor not found" message="Mentor not found. Please check the link or select another mentor."></ErrorComponenet>;
  }

  if (hasError) {
    return (
      <Alert variant="destructive">
        {React.createElement(alertIconMap['destructive'])}
        <AlertDescription>
          {mentorDetailsQuery.isError
            ? 'Failed to load mentor details. Please try again later.'
            : 'Failed to load availability. Please refresh the page.'}
        </AlertDescription>
      </Alert>
    );
  }

  if (googleRequired) {
    return <IntegrationRequired message="You need to integrate your account with google calendar before you can book a session" />;
  }

  // Success page
  if (step === 'success') {
  }

  // Error page
  if (step === 'error') {
    return (
      <ErrorComponenet
        title="Booking Failed"
        message=" We encountered an issue while booking your session. Please try
               again."
      />
    );
  }

  return (
    <div className="mx-auto w-full px-4 py-10">
      {/* Header */}
      <div className="flex flex-col items-center justify-center gap-8">
        {/* Calendar and Time Slots */}
        <div className="w-full space-y-6">
          {/* Calendar */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Calendar className="h-5 w-5" />
                Select Date
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex items-center justify-center">
                <BookingCalendar
                  mode="single"
                  // className="h-full w-full flex"
                  className="max-h-fit flex-1 rounded-xl border-0"
                  captionLayout="label"
                  defaultMonth={selectedDate}
                  onSelect={setSelectedDate}
                  selected={selectedDate}
                  numberOfMonths={isMobile ? 1 : 1}
                  disabled={(date) => {
                    const today = new Date();
                    today.setHours(0, 0, 0, 0);
                    return date < today;
                  }}
                  showOutsideDays={false}
                />
              </div>
            </CardContent>
          </Card>

          {/* Time Slots */}
          <TimeSlots
            selectedDate={selectedDate}
            timeSlots={availableSlots}
            selectedSlot={selectedSlot}
            onSlotSelect={setSelectedSlot}
            isLoading={monthlyAvailabilityQuery.isLoading}
            mentorRate={mentorDetailsQuery.data?.hourlyRate}
          />
        </div>

        <div className="w-full">
          <BookingSummary
            booking={bookingSummary}
            selectedDate={selectedDate}
            selectedSlot={selectedSlot}
            notes={notes}
            onNotesChange={setNotes}
            title={title}
            onTitleChange={setTitle}
            onBookSession={() => {
              if (!currentUser?.integratedWithGoogle) {
                setGoogleRequired(true);
                return;
              }
              if (!googleRequired) handleBookSession();
            }}
            isBookingInProgress={bookSessionMutation.isPending || step === 'confirm'}
            isBookingDisabled={!selectedDate || !selectedSlot}
          />

          {bookSessionMutation.isError && (
            <Alert variant="destructive" className="mt-4">
              {React.createElement(alertIconMap['destructive'])}
              <AlertDescription>Failed to book session. Please try again or contact support if the issue persists.</AlertDescription>
            </Alert>
          )}
        </div>
      </div>
    </div>
  );
}

export function BookingPage() {
  return <BookingContent />;
}
