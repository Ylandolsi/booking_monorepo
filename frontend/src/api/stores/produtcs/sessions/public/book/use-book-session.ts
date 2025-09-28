import { useState } from 'react';
import { useMonthlyAvailability } from '@/api/stores/produtcs/sessions/public';
import { useBookSession, type BookSessionRequestType } from '@/api/stores/produtcs/sessions/public/book/book-session-api';
import type { SessionSlotType } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';
import type { DayAvailabilityType } from '@/api/stores/produtcs/sessions/public/availabilities/availability-types';
import type { Product } from '@/api/stores/produtcs/products-type';

export type BookingStep = 'select' | 'confirm' | 'success' | 'error';

export interface BookingHookState {
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  step: BookingStep;
  notes: string;
  title: string;
}

export function useBooking({ productSlug, storeSlug, product }: { productSlug?: string; storeSlug?: string; product: Product }) {
  const [state, setState] = useState<BookingHookState>({
    selectedDate: new Date(),
    selectedSlot: null,
    step: 'select',
    notes: '',
    title: '',
  });

  const monthlyAvailabilityQuery = useMonthlyAvailability(
    productSlug,
    state.selectedDate?.getFullYear(),
    state.selectedDate ? state.selectedDate.getMonth() + 1 : undefined, // why +1 ? because getMonth() returns 0-11
    { enabled: !!productSlug && !!state.selectedDate },
  );

  const bookSessionMutation = useBookSession();

  console.log('data ', monthlyAvailabilityQuery.data);
  // from the monthly data , get the selected day (user selects it from the calendar) data
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

  // available slots for that selected day
  const availableSlots = selectedDayData?.timeSlots.filter((slot: SessionSlotType) => slot.isAvailable && !slot.isBooked) || [];

  // Actions :

  // select date from calendar
  const setSelectedDate = (date: Date | undefined) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      selectedDate: date,
      selectedSlot: null, // Reset slot when date changes
    }));
  };

  // select time slot from available slots of that day
  const setSelectedSlot = (slot: SessionSlotType | null) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      selectedSlot: slot,
    }));
  };

  // change the step of the booking process : select -> confirm -> success / error
  const setStep = (step: BookingStep) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      step,
    }));
  };

  // set notes for the session
  const setNotes = (notes: string) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      notes,
    }));
  };

  // set title for the session
  const setTitle = (title: string) => {
    setState((prev: BookingHookState) => ({
      ...prev,
      title,
    }));
  };

  // reset the booking process
  const resetBooking = () => {
    setState({
      selectedDate: undefined,
      selectedSlot: null,
      step: 'select',
      notes: '',
      title: '',
    });
  };

  // Create booking summary
  const createBookingSummary = (): BookingSummaryType | null => {
    if (!state.selectedDate || !state.selectedSlot) {
      return null;
    }

    return {
      session: {
        date: state.selectedDate.toISOString().split('T')[0],
        time: state.selectedSlot.startTime,
        duration: 30, // TODO : for now only handle 30 min
        price: Number((product?.price / 2).toFixed(2)),
        currency: '$',
      },
    };
  };

  // Handle booking submission
  const handleBookSession = async () => {
    if (!state.selectedDate || !state.selectedSlot || !productSlug) {
      return;
    }

    // todo : add email ,  name and phone

    const bookingRequest: BookSessionRequestType = {
      date: state.selectedDate.toLocaleDateString('en-CA'),
      startTime: state.selectedSlot.startTime,
      endTime: state.selectedSlot.endTime,
      notes: state.notes ?? '',
      title: state.title,
    };

    try {
      setStep('confirm');
      await bookSessionMutation.mutateAsync({ booking: bookingRequest, productSlug });
      setStep('success');
    } catch (error) {
      console.error('Booking failed:', error);
      setStep('error');
    }
  };

  const bookingSummary = createBookingSummary();
  console.log(state.selectedDate);

  return {
    // State
    selectedDate: state.selectedDate,
    selectedSlot: state.selectedSlot,
    step: state.step,
    notes: state.notes,
    title: state.title,

    // Computed values
    selectedDayData,
    availableSlots,
    bookingSummary,

    // Queries
    monthlyAvailabilityQuery,
    bookSessionMutation,

    // Actions
    setSelectedDate,
    setSelectedSlot,
    setStep,
    setNotes,
    setTitle,
    resetBooking,
    handleBookSession,
  };
}

export type BookingSummaryType = {
  session: {
    date: string;
    time: string;
    duration: number;
    price: number;
    currency: string;
  };
};
