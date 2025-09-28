import { useMutation } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { AdminPayoutKeys } from './admin-payout-keys';
import { CatalogEndpoints } from '@/api/utils';

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
