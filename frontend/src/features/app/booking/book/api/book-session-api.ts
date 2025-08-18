import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { bookingQueryKeys } from './booking-keys.ts';
import type { BookSessionRequestType } from '@/features/app/booking/book/types/booking-types.ts';

// ISO 8601 format ("2025-08-20T15:00:00Z")

export const bookSession = async (
  booking: BookSessionRequestType,
): Promise<number> => {
  return await api.post<number>(MentorshipEndpoints.Sessions.Book, booking);
};

export const cancelSession = async (sessionId: string): Promise<void> => {
  return await api.delete<void>(
    MentorshipEndpoints.Sessions.Cancel.replace('{sessionId}', sessionId),
  );
};

export const getSessionDetails = async (sessionId: string): Promise<number> => {
  return await api.get<number>(
    MentorshipEndpoints.Sessions.GetDetails.replace('{sessionId}', sessionId),
  );
};

export function useBookSession() {
  return useMutation({
    mutationFn: (booking: BookSessionRequestType) => bookSession(booking),
    meta: {
      invalidatesQuery: [
        bookingQueryKeys.myBookings(),
        // bookingQueryKeys.availability(),
      ],
      successMessage: 'Session booked successfully!',
      errorMessage: 'Failed to book session. Please try again.',
    },
  });
}

export function useCancelSession() {
  return useMutation({
    mutationFn: (sessionId: string) => cancelSession(sessionId),
    meta: {
      invalidatesQuery: [
        bookingQueryKeys.myBookings(),
        // bookingQueryKeys.availability(),
      ],
      successMessage: 'Session cancelled successfully',
      errorMessage: 'Failed to cancel session',
    },
  });
}
