import { api, CatalogEndpoints } from '@/lib';
import { useMutation } from '@tanstack/react-query';

// ISO 8601 format ("2025-08-20T15:00:00Z")

export type BookSessionRequestType = {
  date: string; // "YYYY-MM-DD"
  startTime: string; // HH:MM
  endTime: string;
  title: string;
  email: string;
  name: string;
  phone: string;
  notes?: string;
  timeZoneId?: string; // default to 'Africa/Tunis' if not provided
};

export type BookSessionResponseType = {
  payUrl: string;
};

export const bookSession = async ({
  booking,
  productSlug,
}: {
  booking: BookSessionRequestType;
  productSlug: string;
}): Promise<BookSessionResponseType> => {
  return await api.post<BookSessionResponseType>(CatalogEndpoints.Products.Sessions.Book(productSlug), booking);
};

export function useBookSession() {
  return useMutation<BookSessionResponseType, Error, { booking: BookSessionRequestType; productSlug: string }>({
    mutationFn: bookSession,
    meta: {
      // invalidatesQuery: [bookingQueryKeys.myBookings(), WalletKeys.wallet()],

      successAction: (data: BookSessionResponseType) => {
        // Open the payment URL in a new tab if it's a valid URL
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
