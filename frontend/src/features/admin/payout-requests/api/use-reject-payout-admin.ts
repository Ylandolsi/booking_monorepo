import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api-client';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { AdminPayoutKeys } from './admin-payout-keys';

interface RejectPayoutRequest {
  PayoutId: number;
}

export const useRejectPayoutAdmin = () => {
  return useMutation({
    mutationFn: async (payoutId: number): Promise<void> => {
      await api.post<void>(
        MentorshipEndpoints.Payouts.Admin.RejectPayout,
        {
          PayoutId: payoutId,
        } satisfies RejectPayoutRequest
      );
    },
    meta: {
      invalidatesQuery: [AdminPayoutKeys.allPayouts()],
      successMessage: 'Payout rejected successfully',
      // errorMessage: 'Failed to reject payout',
    },
  });
};