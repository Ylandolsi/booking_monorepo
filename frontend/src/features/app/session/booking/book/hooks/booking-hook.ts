import { useCallback, useState } from 'react';
import { useParams } from '@tanstack/react-router';
import type {
  BookingSummaryType,
  BookSessionRequestType,
  DayAvailabilityType,
  SessionSlotType,
} from '@/features/app/session/booking';
import { useMentorDetails } from '@/features/app/mentor/become';
import {
  useBookSession,
  useMonthlyAvailability,
} from '@/features/app/session/booking';
import { useProfile } from '@/features/app/profile';
import { useAuth } from '@/features/auth';

export type BookingStep = 'select' | 'confirm' | 'success' | 'error';

export interface BookingHookState {
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  step: BookingStep;
  notes: string;
}

export function useBooking({
  mentorSlug,
  iamTheMentor,
}: {
  mentorSlug?: string;
  iamTheMentor: boolean;
}) {
  const [state, setState] = useState<BookingHookState>({
    selectedDate: new Date(),
    selectedSlot: null,
    step: 'select',
    notes: '',
  });

  const mentorInfoQuery = useProfile(mentorSlug!);
  const mentorDetailsQuery = useMentorDetails(mentorSlug, {
    enabled: !!mentorSlug && !iamTheMentor,
  });

  const monthlyAvailabilityQuery = useMonthlyAvailability(
    mentorSlug,
    state.selectedDate?.getFullYear(),
    state.selectedDate ? state.selectedDate.getMonth() + 1 : undefined,
    { enabled: !!mentorSlug && !!state.selectedDate && !iamTheMentor },
  );

  const bookSessionMutation = useBookSession();

  const isLoading =
    mentorDetailsQuery.isLoading || monthlyAvailabilityQuery.isLoading;
  const hasError =
    mentorDetailsQuery.isError || monthlyAvailabilityQuery.isError;

  const selectedDayData: DayAvailabilityType | undefined =
    state.selectedDate && monthlyAvailabilityQuery.data
      ? monthlyAvailabilityQuery.data.days.find((day: DayAvailabilityType) => {
          const dayDate = new Date(day.date);
          return (
            dayDate.getDate() === state.selectedDate!.getDate() &&
            dayDate.getMonth() === state.selectedDate!.getMonth() &&
            dayDate.getFullYear() === state.selectedDate!.getFullYear()
          );
        })
      : undefined;

  const availableSlots =
    selectedDayData?.timeSlots.filter(
      (slot: SessionSlotType) => slot.isAvailable && !slot.isBooked,
    ) || [];

  // Actions
  const setSelectedDate = useCallback((date: Date | undefined) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      selectedDate: date,
      selectedSlot: null, // Reset slot when date changes
    }));
  }, []);

  const setSelectedSlot = useCallback((slot: SessionSlotType | null) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      selectedSlot: slot,
    }));
  }, []);

  const setStep = useCallback((step: BookingStep) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      step,
    }));
  }, []);

  const setNotes = useCallback((notes: string) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      notes,
    }));
  }, []);

  const resetBooking = useCallback(() => {
    setState({
      selectedDate: undefined,
      selectedSlot: null,
      step: 'select',
      notes: '',
    });
  }, []);

  // Create booking summary
  const createBookingSummary = useCallback((): BookingSummaryType | null => {
    if (
      !mentorDetailsQuery.data ||
      !mentorInfoQuery.data ||
      !state.selectedDate ||
      !state.selectedSlot
    ) {
      return null;
    }

    return {
      mentor: {
        slug: mentorSlug || '',
        name: `${mentorInfoQuery.data.firstName} ${mentorInfoQuery.data.lastName}`,
        avatar: mentorInfoQuery.data.profilePicture?.profilePictureLink,
        title: 'Professional Mentor',
        expertise: ['General Mentoring'],
        hourlyRate: mentorDetailsQuery.data.hourlyRate || 50,
      },
      session: {
        date: state.selectedDate.toISOString().split('T')[0],
        time: state.selectedSlot.startTime,
        duration: 30,
        price: mentorDetailsQuery.data.hourlyRate || 50,
        currency: '$',
      },
      total: mentorDetailsQuery.data.hourlyRate || 50,
    };
  }, [
    mentorDetailsQuery.data,
    mentorInfoQuery.data,
    state.selectedDate,
    state.selectedSlot,
    mentorSlug,
  ]);

  // Handle booking submission
  const handleBookSession = useCallback(async () => {
    if (!state.selectedDate || !state.selectedSlot || !mentorSlug) {
      return;
    }

    const bookingRequest: BookSessionRequestType = {
      mentorSlug,
      date: state.selectedDate.toLocaleDateString('en-CA'),
      startTime: state.selectedSlot.startTime,
      endTime: state.selectedSlot.endTime,
      notes: state.notes ?? '',
    };

    try {
      setStep('confirm');
      await bookSessionMutation.mutateAsync(bookingRequest);
      setStep('success');
    } catch (error) {
      console.error('Booking failed:', error);
      setStep('error');
    }
  }, [
    state.selectedDate,
    state.selectedSlot,
    state.notes,
    mentorSlug,
    bookSessionMutation,
    setStep,
  ]);

  const bookingSummary = createBookingSummary();
  console.log(state.selectedDate);

  return {
    // State
    selectedDate: state.selectedDate,
    selectedSlot: state.selectedSlot,
    step: state.step,
    notes: state.notes,

    // Computed values
    isLoading,
    hasError,
    selectedDayData,
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
    setStep,
    setNotes,
    resetBooking,
    handleBookSession,
  };
}
