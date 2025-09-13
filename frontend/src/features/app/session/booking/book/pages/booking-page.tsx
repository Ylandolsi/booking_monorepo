import { Calendar, User, CheckCircle } from 'lucide-react';
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
  Calendar as BookingCalendar,
  PageLoading2,
  alertIconMap,
} from '@/components/ui';
import { useBooking, TimeSlots } from '@/features/app/session/booking/book';
import { useIsMobile } from '@/hooks/use-mobile';
import { useAppNavigation } from '@/hooks/use-navigation';
import { formatDate } from '@/utils';
import { ErrorComponenet, IntegrationRequired } from '@/components';
import { BookingSummary } from '@/features/app/session/booking/book/components';
import React, { useEffect, useState } from 'react';
import { useParams } from '@tanstack/react-router';
import { useAuth } from '@/features/auth';
import { signalRService, type NotificationSignalR } from '@/services/notification-service'; // Assuming this is the correct import path; adjust if needed
import { toast } from 'sonner';

function BookingContent() {
  const nav = useAppNavigation();
  const isMobile = useIsMobile();
  const { mentorSlug } = useParams({ strict: false }) as {
    mentorSlug?: string;
  };

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
    // return (
    //   <div className="container mx-auto max-w-4xl">
    //     <div className="text-center space-y-6">
    //       <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto">
    //         <CheckCircle className="w-10 h-10 text-green-600" />
    //       </div>
    //       <div>
    //         <h1 className="text-3xl font-bold text-gray-900 mb-2">
    //           Session Booked Successfully!
    //         </h1>
    //         <p className="text-lg text-gray-600">
    //           Your mentoring session has been confirmed. You'll receive an email
    //           with the meeting details shortly.
    //         </p>
    //       </div>
    //       {bookingSummary && selectedDate && selectedSlot && (
    //         <div className="max-w-md mx-auto">
    //           <Card>
    //             <CardContent className="pt-6">
    //               <div className="space-y-3">
    //                 <div className="flex justify-between">
    //                   <span className="text-sm text-gray-600">Mentor:</span>
    //                   <span className="text-sm font-medium">
    //                     {bookingSummary.mentor.name}
    //                   </span>
    //                 </div>
    //                 <div className="flex justify-between">
    //                   <span className="text-sm text-gray-600">Date:</span>
    //                   <span className="text-sm font-medium">
    //                     {formatDate(selectedDate)}
    //                   </span>
    //                 </div>
    //                 <div className="flex justify-between">
    //                   <span className="text-sm text-gray-600">Time:</span>
    //                   <span className="text-sm font-medium">
    //                     {selectedSlot.startTime}
    //                   </span>
    //                 </div>
    //                 <div className="flex justify-between border-t pt-2">
    //                   <span className="font-medium">Total:</span>
    //                   <span className="font-bold text-green-600">
    //                     ${bookingSummary.total}
    //                   </span>
    //                 </div>
    //               </div>
    //             </CardContent>
    //           </Card>
    //         </div>
    //       )}
    //       <div className="flex flex-col sm:flex-row gap-4 justify-center">
    //         <Button onClick={() => nav.goToApp()}>View My Bookings</Button>
    //         <Button variant="outline" onClick={resetBooking}>
    //           Book Another Session
    //         </Button>
    //       </div>
    //     </div>
    //   </div>
    // );
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
    <div className="container mx-auto py-10 px-4 max-w-7xl">
      {/* Header */}
      <div className="text-center space-y-6 mb-8">
        <div>
          <h1 className="text-4xl font-bold text- mb-4">
            Book a Session with {mentorInfoQuery.data?.firstName} {mentorInfoQuery.data?.lastName}
          </h1>
        </div>

        {/* Progress Indicator */}
        <div className="flex items-center justify-center space-x-4">
          <div className="flex items-center space-x-2">
            <div
              className={`w-8 h-8 rounded-full flex items-center justify-center ${
                selectedDate ? 'bg-primary text-white' : 'bg-muted-200 text-black'
              }`}
            >
              1
            </div>
            <span className="text-sm font-medium">Select Date</span>
          </div>
          <div className="w-8 h-px bg-gray-300" />
          <div className="flex items-center space-x-2">
            <div
              className={`w-8 h-8 rounded-full flex items-center justify-center ${
                selectedSlot ? 'bg-primary text-white' : 'bg-muted-200 text-black'
              }`}
            >
              2
            </div>
            <span className="text-sm font-medium">Choose Time</span>
          </div>
          <div className="w-8 h-px bg-gray-300" />
          <div className="flex items-center space-x-2">
            <div
              className={`w-8 h-8 rounded-full flex items-center justify-center ${
                bookingSummary ? 'bg-primary text-white' : 'bg-muted-200 text-black'
              }`}
            >
              3
            </div>
            <span className="text-sm font-medium">Confirm Booking</span>
          </div>
        </div>
      </div>

      {/* Mentor Profile */}
      {mentorDetailsQuery.data && mentorInfoQuery.data && (
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
                <AvatarImage
                  src={mentorInfoQuery.data.profilePicture?.profilePictureLink}
                  alt={`${mentorInfoQuery.data.firstName} ${mentorInfoQuery.data.lastName}`}
                />
                <AvatarFallback className="bg-blue-100 text-blue-700 font-semibold text-lg">
                  {mentorInfoQuery.data.firstName[0]}
                  {mentorInfoQuery.data.lastName[0]}
                </AvatarFallback>
              </Avatar>
              <div className="flex-1">
                <h3 className="text-xl font-semibold mb-1">
                  {mentorInfoQuery.data.firstName} {mentorInfoQuery.data.lastName}
                </h3>
                <p className="text-gray-600 mb-2">Professional Mentor</p>
                <div className="flex flex-wrap gap-2 mb-3">
                  {['General Mentoring', 'Career Development'].map((skill, index) => (
                    <Badge key={index} variant="secondary">
                      {skill}
                    </Badge>
                  ))}
                </div>
                <div className="text-sm text-gray-600">
                  <span className="font-medium">Hourly Rate: </span>
                  <span className="text-lg font-semibold text-green-600">${mentorDetailsQuery.data.hourlyRate || 50}/hour</span>
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
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Calendar className="w-5 h-5" />
                Select Date
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex items-center justify-center">
                <BookingCalendar
                  mode="single"
                  // className="h-full w-full flex"
                  className="rounded-xl border-0 flex-1 max-h-fit"
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

        <div className="lg:col-span-1">
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
