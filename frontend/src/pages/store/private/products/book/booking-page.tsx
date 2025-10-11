import { Calendar } from 'lucide-react';
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Alert,
  AlertDescription,
  Calendar as BookingCalendar,
  alertIconMap,
  Form,
  FormField,
  FormItem,
  FormLabel,
  FormControl,
  Input,
  Textarea,
} from '@/components/ui';
import { useAppNavigation } from '@/hooks/use-navigation';
import { ErrorComponenet } from '@/components';
import React, { useEffect } from 'react';
import { useAuth } from '@/api/auth';
import { signalRService, type NotificationSignalR } from '@/services/notification-service'; // Assuming this is the correct import path; adjust if needed
import { toast } from 'sonner';
import { BookingSummary, TimeSlots } from '@/pages/store/private/products/book';
import { useBooking } from '@/api/stores/produtcs';
import { useParams } from '@tanstack/react-router';
import { useIsMobile } from '@/hooks/use-media-query';
import { logger } from '@/lib';

export function BookingPage({ product }: { product: { price: number } }) {
  const nav = useAppNavigation();
  const isMobile = useIsMobile();
  const { currentUser } = useAuth();

  const { productSlug, storeSlug } = useParams({ strict: false }) as Record<string, string | undefined>;

  const {
    form,
    // State
    state,
    // Computed values
    availableSlots,
    bookingSummary,

    // Queries
    monthlyAvailabilityQuery,
    bookSessionMutation,

    // Actions
    setSelectedDate,
    setSelectedSlot,
    // setStep,
    setStep,
    handleBookSession,
  } = useBooking({ productSlug, storeSlug, product });

  const { selectedDate, selectedSlot, step, title, notes } = state;

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
      signalRService.startConnection(currentUser.slug).catch((error) => logger.error('Failed to establish SignalR connection:', error));

      // Register callback
      signalRService.addCallback('session_confirmed', sessionConfirmedMentee);

      // Cleanup on unmount
      return () => {
        signalRService.removeCallback('session_confirmed', sessionConfirmedMentee);
        signalRService.stopConnection(currentUser.slug).catch((error) => logger.error('Error stopping SignalR connection:', error));
      };
    }
  }, [currentUser]);

  // if (isLoading) {
  //   return <PageLoading2 title="Loading booking page " description="Fetching mentor details and availability" />;
  // }

  // if (mentorDetailsQuery.error?.message?.includes('not found')) {
  //   return <ErrorComponenet title="Mentor not found" message="Mentor not found. Please check the link or select another mentor."></ErrorComponenet>;
  // }

  // if (hasError) {
  //   return (
  //     <Alert variant="destructive">
  //       {React.createElement(alertIconMap['destructive'])}
  //       <AlertDescription>
  //         {mentorDetailsQuery.isError
  //           ? 'Failed to load mentor details. Please try again later.'
  //           : 'Failed to load availability. Please refresh the page.'}
  //       </AlertDescription>
  //     </Alert>
  //   );
  // }

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
    <div className="">
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
          />
        </div>

        <div className="w-full">
          <BookingSummary
            booking={bookingSummary}
            selectedDate={selectedDate}
            selectedSlot={selectedSlot}
            onBookSession={handleBookSession}
            isBookingInProgress={bookSessionMutation.isPending || step === 'confirm'}
            isBookingDisabled={!selectedDate || !selectedSlot}
          />

          {/* {product.} */}

          {/* Input fields */}
          <Form {...form}>
            <form>
              <div className="mt-10 flex w-full flex-col gap-5">
                <FormField
                  control={form.control}
                  name="name"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground">Full Name</FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="Full Name" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
                <FormField
                  control={form.control}
                  name="phone"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground">Phone Number</FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="Phone Number" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground">Email Address</FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="falten@gmail.com" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
                <FormField
                  control={form.control}
                  name="title"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground mb-2 flex flex-col items-start justify-start gap-2">
                          <div>Session Title</div>
                        </FormLabel>
                        <FormControl>
                          <Input {...field} placeholder="Session Title" />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
                <FormField
                  control={form.control}
                  name="notes"
                  render={({ field }) => {
                    return (
                      <FormItem>
                        <FormLabel className="text-foreground mb-2 flex flex-col items-start justify-start gap-2">
                          <div>Additional notes</div>
                        </FormLabel>
                        <FormControl>
                          <Textarea
                            {...field}
                            placeholder="Add any specific topics you'd like to discuss or questions you have..."
                            className="min-h-[80px] w-full resize-none break-all"
                            maxLength={500}
                          />
                        </FormControl>
                      </FormItem>
                    );
                  }}
                />
              </div>

              <button
                onClick={handleBookSession}
                className="bg-primary shadow-primary/30 hover:bg-opacity-90 mt-10 h-14 w-full rounded-xl text-lg font-bold text-white shadow-lg transition-all duration-300"
              >
                Buy Now
              </button>
            </form>
          </Form>
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
