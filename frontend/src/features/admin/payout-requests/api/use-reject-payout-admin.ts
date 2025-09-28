import { useMutation } from '@tanstack/react-query';
import { api } from '@/lib/api/api-client';
import { AdminPayoutKeys } from './admin-payout-keys';
import { CatalogEndpoints } from '@/lib';

interface RejectPayoutRequest {
  PayoutId: number;
}

export const useRejectPayoutAdmin = () => {
  return useMutation({
    mutationFn: async (payoutId: number): Promise<void> => {
      await api.post<void>(CatalogEndpoints.Payouts.Admin.RejectPayout, {
        PayoutId: payoutId,
      } satisfies RejectPayoutRequest);
    },
    meta: {
      invalidatesQuery: [AdminPayoutKeys.allPayouts()],
      successMessage: 'Payout rejected successfully',
      // errorMessage: 'Failed to reject payout',
    },
  });
};
