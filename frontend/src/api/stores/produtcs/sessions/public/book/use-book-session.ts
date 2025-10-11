import { useMonthlyAvailability } from '@/api/stores/produtcs/sessions/public';
import { useBookSession, type BookSessionRequestType } from '@/api/stores/produtcs/sessions/public/book/book-session-api';
import type { DayAvailabilityType } from '@/api/stores/produtcs/sessions/public/availabilities/availability-types';
import type { Product } from '@/api/stores/produtcs/products-type';
import { toast } from 'sonner';
import z from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { logger } from '@/lib';

export const sessionSlotSchema = z.object({
  startTime: z.string(), // 16:00
  endTime: z.string(), // 16:00
  isAvailable: z.boolean(),
  isBooked: z.boolean(),
});

export type SessionSlotType = z.infer<typeof sessionSlotSchema>;

export const bookingStepSchema = z.enum(['select', 'confirm', 'success', 'error']);

export type BookingStep = z.infer<typeof bookingStepSchema>;

export const bookingHookStateSchema = z.object({
  selectedDate: z.date().optional(),
  selectedSlot: sessionSlotSchema.nullable(),
  step: bookingStepSchema,
  title: z.string().min(3, 'Title must be at least 3 characters long.').max(100, 'Title cannot exceed 100 characters.').optional().or(z.literal('')),
  email: z.string().email('Please enter a valid email address.'),
  name: z.string().min(3, 'Name must be at least 3 characters long.'),
  phone: z.string().regex(/^\d{8}$/, 'Please enter a valid phone number for Tunisia (+216xxxxxxxx).'),
  notes: z.string().max(500, 'Notes cannot exceed 500 characters.').optional(),
});

export type BookingHookState = z.infer<typeof bookingHookStateSchema>;

export function useBooking({ productSlug, storeSlug, product }: { productSlug?: string; storeSlug?: string; product: Product }) {
  const form = useForm<BookingHookState>({
    resolver: zodResolver(bookingHookStateSchema),
    defaultValues: {
      selectedDate: new Date(),
      selectedSlot: null,
      step: 'select',
      notes: '',
      title: '',
      email: '',
      name: '',
      phone: '',
    },
  });

  const { watch, setValue, reset } = form;
  const state = watch();

  const monthlyAvailabilityQuery = useMonthlyAvailability(
    productSlug,
    storeSlug,
    state.selectedDate?.getFullYear(),
    state.selectedDate ? state.selectedDate.getMonth() + 1 : undefined, // why +1 ? because getMonth() returns 0-11
    { enabled: !!productSlug && !!state.selectedDate },
  );

  const bookSessionMutation = useBookSession();

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
    setValue('selectedDate', date);
    setValue('selectedSlot', null); // Reset slot when date changes
  };

  // select time slot from available slots of that day
  const setSelectedSlot = (slot: SessionSlotType | null) => {
    setValue('selectedSlot', slot);
  };

  // change the step of the booking process : select -> confirm -> success / error
  const setStep = (step: BookingStep) => {
    setValue('step', step);
  };

  // reset the booking process
  const resetBooking = () => {
    reset({
      selectedDate: undefined,
      selectedSlot: null,
      step: 'select',
      notes: '',
      title: '',
      email: '',
      name: '',
      phone: '',
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
  const handleBookSession = form.handleSubmit(async (data: BookingHookState) => {
    if (!data.selectedDate || !data.selectedSlot || !productSlug) {
      toast.error('Please select a date and time slot.');
      return;
    }

    const bookingRequest: BookSessionRequestType = {
      date: data.selectedDate.toLocaleDateString('en-CA'),
      startTime: data.selectedSlot.startTime,
      endTime: data.selectedSlot.endTime,
      notes: data.notes ?? '',
      title: data.title || '',
      email: data.email,
      name: data.name,
      phone: data.phone,
      timeZoneId: Intl.DateTimeFormat().resolvedOptions().timeZone || 'Africa/Tunis', // get the user's timezone
    };

    try {
      setStep('confirm');
      await bookSessionMutation.mutateAsync({ booking: bookingRequest, productSlug, storeSlug: storeSlug! });
      setStep('success');
    } catch (error) {
      logger.error('Booking failed:', error);
      setStep('error');
    }
  });

  const bookingSummary = createBookingSummary();
  logger.info(state.selectedDate);

  return {
    // Form instance
    form,

    // State
    state,

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
