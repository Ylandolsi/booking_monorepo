import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api-client';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { PayoutKeys } from '@/features/app/payout/api/payout-keys';
import { WalletKeys } from '@/features/shared';

interface PayoutRequestData {
  Amount: number;
}

interface PayoutRequestResponse {
  success: boolean;
  message?: string;
}

export const useRequestPayout = () => {
  return useMutation({
    mutationFn: async (amount: number): Promise<PayoutRequestResponse> => {
      const response = await api.post<PayoutRequestResponse>(
        MentorshipEndpoints.Payouts.Payout,
        {
          Amount: amount,
        } satisfies PayoutRequestData,
      );
      return response;
    },
    meta: {
      // TODO : invalidate mentor-data-slug key
      invalidatesQuery: [PayoutKeys.history(), WalletKeys.wallet()],
      successMessage: 'Payou request successfully ',
      errorMessage: 'Failed to update profile',
    },
  });
};
