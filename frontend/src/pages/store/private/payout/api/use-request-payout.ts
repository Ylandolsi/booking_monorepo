import { useMutation } from '@tanstack/react-query';
import { api } from '@/api/utils';
import { WalletKeys } from '@/api/stores';
import { CatalogEndpoints } from '@/api/utils';
import { PayoutKeys } from '@/pages/store';

type PayoutRequestData = {
  Amount: number;
};

export const useRequestPayout = () => {
  return useMutation({
    mutationFn: async (amount: number): Promise<void> => {
      await api.post<void>(CatalogEndpoints.Payouts.Payout, {
        Amount: amount,
      } satisfies PayoutRequestData);
    },
    meta: {
      // TODO : invalidate mentor-data-slug key
      invalidatesQuery: [PayoutKeys.history(), WalletKeys.wallet()],
      successMessage: 'Payout requested successfully ',
      // errorMessage: 'Failed to request to a payout',
    },
  });
};
