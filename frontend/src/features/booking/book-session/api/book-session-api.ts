import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import type {
  BookingSession,
  SessionBookingRequest,
} from '../types/session-types.ts';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints.ts';
import { bookingQueryKeys } from './booking-keys.ts';

export const bookSession = async (
  booking: SessionBookingRequest,
): Promise<BookingSession> => {
  return await api.post<BookingSession>(
    MentorshipEndpoints.Sessions.Book,
    booking,
  );
};

export const cancelSession = async (sessionId: string): Promise<void> => {
  return await api.delete<void>(
    MentorshipEndpoints.Sessions.Cancel.replace('{sessionId}', sessionId),
  );
};

export const getSessionDetails = async (
  sessionId: string,
): Promise<BookingSession> => {
  return await api.get<BookingSession>(
    MentorshipEndpoints.Sessions.GetDetails.replace('{sessionId}', sessionId),
  );
};

export function useBookSession() {
  return useMutation({
    mutationFn: (booking: SessionBookingRequest) => bookSession(booking),
    meta: {
      invalidatesQuery: [
        bookingQueryKeys.myBookings(),
        bookingQueryKeys.availability(),
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
        bookingQueryKeys.availability(),
      ],
      successMessage: 'Session cancelled successfully',
      errorMessage: 'Failed to cancel session',
    },
  });
}
