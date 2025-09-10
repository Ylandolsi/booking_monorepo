import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import { MentorshipEndpoints } from '@/lib/api/mentor-endpoints.ts';
import { bookingQueryKeys } from './booking-keys.ts';
import type { BookSessionRequestType } from '@/features/app/session/booking/book/types/booking-types.ts';
import { WalletKeys } from '@/features/shared/get-wallet.tsx';
import { availabilityQueryKeys } from '@/features/app/session/booking/availability/index.ts';

// ISO 8601 format ("2025-08-20T15:00:00Z")

export type BookSessionResponseType = {
  payUrl: string;
};

export const bookSession = async (booking: BookSessionRequestType): Promise<BookSessionResponseType> => {
  return await api.post<BookSessionResponseType>(MentorshipEndpoints.Sessions.Book, booking);
};

export function useBookSession({ mentorSlug }: { mentorSlug: string }) {
  return useMutation({
    mutationFn: (booking: BookSessionRequestType) => bookSession(booking),
    meta: {
      invalidatesQuery: [bookingQueryKeys.myBookings(), WalletKeys.wallet(), availabilityQueryKeys.monthlyAvailability(mentorSlug)],
      successAction: (data: BookSessionResponseType) => {
        let isValidUrl: boolean = true;
        try {
          new URL(data.payUrl);
        } catch {
          isValidUrl = false;
        }
        if (isValidUrl) window.open(data.payUrl, '_blank');
      },
      successMessage: 'Session booked successfully!',
      errorMessage: 'Failed to book session. Please try again.',
    },
  });
}
