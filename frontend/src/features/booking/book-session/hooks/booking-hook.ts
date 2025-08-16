import { useEffect, useState } from 'react';
import { useParams } from '@tanstack/react-router';
import type { BookingSummary, TimeSlot } from '@/features/booking';
import { useQueryState } from '@/hooks';
import { useMentorDetails } from '@/features/mentor';
import { useBookSession, useMonthlyAvailability } from '@/features/booking';
import { useProfile } from '@/features/profile';

export function useBooking() {
  const { mentorSlug } = useParams({ strict: false }) as {
    mentorSlug?: string;
  };


  const [selectedDate, setSelectedDate] = useState<Date | undefined>(new Date());
  const [selectedSlot, setSelectedSlot] = useState<TimeSlot | null>(null);
  const [bookingStep, setBookingStep] = useState<'select' | 'success'>(
    'select',
  );

  const mentorInfoQuery = useProfile(mentorSlug!) ;
  const mentorDetailsQuery = useMentorDetails(mentorSlug, { enabled: !!mentorSlug });

  const monthlyAvailabilityQuery = useMonthlyAvailability(
    mentorSlug,
    selectedDate?.getFullYear(),
    selectedDate?.getMonth() + 1
  );

  const bookSessionMutation = useBookSession();

  useEffect(() => {
    setSelectedSlot(null);

  }, [selectedDate]);

  // Handle booking confirmation
  /* const handleBookSession = async () => {
     if (!selectedDate || !selectedSlot || !mentorSlug) return;

     try {
       await bookSessionMutation.mutateAsync({
         mentorSlug,
         date: selectedDate,
         startTime: new Date(selectedSlot.startTime).toLocaleTimeString(
           'en-US',
           {
             hour12: false,
             hour: '2-digit',
             minute: '2-digit',
           },
         ),
         duration: 30,
         notes: '',
       });
       setBookingStep('success');
     } catch (error) {
       console.error('Booking failed:', error);
     }
   };*/

  // Create booking summary data
  /*const createBookingSummary = (): BookingSummary | null => {
    if (!mentorDetailsQuery.data || !selectedDate || !selectedSlot) return null;

    return {
      mentor: {
        slug: mentorSlug || '',
/!*        name: `Professional Mentor`,
        avatar: undefined,
        title: 'Experienced Mentor',
        expertise: ['General Mentoring'],*!/
        hourlyRate: mentorDetailsQuery.data.hourlyRate || 50,
      },
      session: {
        date: selectedDate,
        time: new Date(selectedSlot.startTime).toLocaleTimeString('en-US', {
          hour: '2-digit',
          minute: '2-digit',
          hour12: true,
        }),
        duration: 30,
        price: mentorDetailsQuery.data.hourlyRate || 50,
        currency: '$',
      },
      total: mentorDetailsQuery.data.hourlyRate || 50,
    };
  };*/

  /*
    const bookingSummary = createBookingSummary();
  */

  return {
    mentorInfoQuery,
    mentorDetailsQuery,
    monthlyAvailabilityQuery,
    selectedDate,
    setSelectedDate,
    bookingStep,
    setBookingStep,
    bookSessionMutation,
    // handleBookSession,
    // bookingSummary,
  };

}